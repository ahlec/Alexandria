using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Alexandria.Searching;
using Bibliothecary.Data.Utils;

namespace Bibliothecary.Data
{
	public sealed class Project
	{
		const String DefaultProjectName = "Untitled";
		const Int32 DefaultUpdateFrequencyMinutes = 24 * 60; // 1 day
		public const Int32 MinimumUpdateFrequencyMinutes = 1; // I mean, really??

		Project( Int32 projectId )
		{
			_projectId = projectId;
		}

		public Boolean HasUnsavedChanges
		{
			get
			{
				if ( !String.Equals( _lastSavedName, Name, StringComparison.CurrentCulture ) )
				{
					return true;
				}

				if ( (Int32) _lastSavedUpdateFrequency.TotalMinutes != (UInt32) UpdateFrequency.TotalMinutes )
				{
					return true;
				}

				if ( !_lastSavedSearchQuery.Equals( SearchQuery ) )
				{
					return true;
				}

				return false;
			}
		}

		#region Project Fields

		public String Name { get; private set; }

		public TimeSpan UpdateFrequency { get; private set; }

		public LibrarySearch SearchQuery { get; private set; }

		#endregion

		public Boolean SetName( String name )
		{
			name = name ?? String.Empty;
			if ( String.Equals( Name, name ) )
			{
				return false;
			}
			Name = name;
			return true;
		}

		public Boolean SetUpdateFrequency( Int32 minutes )
		{
			if ( minutes < MinimumUpdateFrequencyMinutes )
			{
				throw new ArgumentOutOfRangeException( nameof( minutes ) );
			}

			if ( minutes == (UInt32) UpdateFrequency.TotalMinutes )
			{
				return false;
			}

			UpdateFrequency = TimeSpan.FromMinutes( minutes );
			return true;
		}

		internal static IEnumerable<Int32> GetAllProjectIds( SQLiteConnection connection )
		{
			SQLiteUtils.ValidateConnection( connection );

			using ( SQLiteCommand selectCommand = new SQLiteCommand( "SELECT project_id FROM projects", connection ) )
			{
				using ( SQLiteDataReader reader = selectCommand.ExecuteReader() )
				{
					while ( reader.Read() )
					{
						yield return reader.GetInt32( 0 );
					}
				}
			}
		}

		internal static Project Create( SQLiteConnection connection )
		{
			SQLiteUtils.ValidateConnection( connection );

			using ( SQLiteTransaction transaction = connection.BeginTransaction() )
			{
				try
				{
					using ( SQLiteCommand insertCommand = new SQLiteCommand( connection ) )
					{
						insertCommand.CommandText = "INSERT INTO projects( project_name, update_frequency_minutes ) VALUES( @name, @frequency )";
						insertCommand.Parameters.AddWithValue( "@name", DefaultProjectName );
						insertCommand.Parameters.AddWithValue( "@frequency", DefaultUpdateFrequencyMinutes );
						Int32 numberRowsAffected = insertCommand.ExecuteNonQuery();
						if ( numberRowsAffected != 1 )
						{
							throw new ApplicationException( "Unable to create the project in the database!" );
						}
					}

					Project newProject;
					using ( SQLiteCommand getProjectId = new SQLiteCommand( "SELECT last_insert_rowid()", connection ) )
					{
						using ( SQLiteDataReader reader = getProjectId.ExecuteReader() )
						{
							if ( !reader.Read() )
							{
								throw new ApplicationException( "No result from last_insert_rowid?" );
							}
							Int32 projectId = reader.GetInt32( 0 );
							newProject = new Project( projectId )
							{
								Name = DefaultProjectName,
								UpdateFrequency = TimeSpan.FromMinutes( DefaultUpdateFrequencyMinutes ),
								SearchQuery = new LibrarySearch()
							};
						}
					}

					transaction.Commit();

					newProject.MarkLastSavedData();

					return newProject;
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		internal static Project Read( SQLiteConnection connection, Int32 projectId )
		{
			SQLiteUtils.ValidateConnection( connection );

			Project parsed = new Project( projectId );
			using ( SQLiteCommand selectBasicDataCommand = new SQLiteCommand( "SELECT project_name, update_frequency_minutes FROM projects WHERE project_id = @projectId", connection ) )
			{
				selectBasicDataCommand.Parameters.AddWithValue( "@projectId", projectId );
				using ( SQLiteDataReader reader = selectBasicDataCommand.ExecuteReader() )
				{
					if ( !reader.Read() )
					{
						throw new ArgumentOutOfRangeException( nameof( projectId ), "No project exists with the specified projectId" );
					}

					parsed.Name = reader.GetString( 0 );
					Int32 updateFrequencyMinutes = Math.Max( MinimumUpdateFrequencyMinutes, reader.GetInt32( 1 ) );
					parsed.UpdateFrequency = TimeSpan.FromMinutes( updateFrequencyMinutes );
				}
			}
			parsed.SearchQuery = LibrarySearchUtils.ReadFromDatabase( connection, projectId );

			parsed.MarkLastSavedData();

			return parsed;
		}

		internal void Save( SQLiteConnection connection )
		{
			SQLiteUtils.ValidateConnection( connection );

			using ( SQLiteTransaction transaction = connection.BeginTransaction() )
			{
				try
				{
					using ( SQLiteCommand updateProjectCommand = new SQLiteCommand( connection ) )
					{
						updateProjectCommand.CommandText = "UPDATE projects SET project_name = @name, update_frequency_minutes = @frequency WHERE project_id = @projectId";
						updateProjectCommand.Parameters.AddWithValue( "@projectId", _projectId );
						updateProjectCommand.Parameters.AddWithValue( "@name", Name );
						updateProjectCommand.Parameters.AddWithValue( "@frequency", UpdateFrequency.TotalMinutes );
						Int32 numberRowsAffected = updateProjectCommand.ExecuteNonQuery();
						if ( numberRowsAffected != 1 )
						{
							throw new ApplicationException( "Unable to update the project in the database!" );
						}
					}

					HashSet<KeyValuePair<String, String>> lastSavedSearchSql = new HashSet<KeyValuePair<String, String>>( LibrarySearchUtils.GetProjectSearchFields( _lastSavedSearchQuery ) );
					foreach ( KeyValuePair<String, String> fieldSql in LibrarySearchUtils.GetProjectSearchFields( SearchQuery ) )
					{
						if ( lastSavedSearchSql.Remove( fieldSql ) )
						{
							// Was removed from the old list of fields, so that means that it was there before, so we don't need to update.
							continue;
						}

						using ( SQLiteCommand insertFieldCommand = new SQLiteCommand( connection ) )
						{
							insertFieldCommand.CommandText = "INSERT INTO project_search_fields( project_id, field_name, field_value ) VALUES( @projectId, @fieldName, @fieldValue )";
							insertFieldCommand.Parameters.AddWithValue( "@projectId", _projectId );
							insertFieldCommand.Parameters.AddWithValue( "@fieldName", fieldSql.Key );
							insertFieldCommand.Parameters.AddWithValue( "@fieldValue", fieldSql.Value );
							Int32 numberRowsAffected = insertFieldCommand.ExecuteNonQuery();
							if ( numberRowsAffected != 1 )
							{
								throw new ApplicationException( $"Unable to insert a project_search_field of key '{fieldSql.Key}' and value '{fieldSql.Value}' into project {_projectId}" );
							}
						}
					}

					foreach ( KeyValuePair<String, String> removedField in lastSavedSearchSql )
					{
						using ( SQLiteCommand deleteFieldCommand = new SQLiteCommand( connection ) )
						{
							deleteFieldCommand.CommandText = "DELETE FROM project_search_fields WHERE project_id = @projectId AND field_name = @fieldName AND field_value = @fieldValue";
							deleteFieldCommand.Parameters.AddWithValue( "@projectId", _projectId );
							deleteFieldCommand.Parameters.AddWithValue( "@fieldName", removedField.Key );
							deleteFieldCommand.Parameters.AddWithValue( "@fieldValue", removedField.Value );
							Int32 numberRowsAffected = deleteFieldCommand.ExecuteNonQuery();
							if ( numberRowsAffected != 1 )
							{
								throw new ApplicationException( $"Unable to delete a project_search_field of key '{removedField.Key}' and value '{removedField.Value}' from project {_projectId}" );
							}
						}
					}

					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}

			MarkLastSavedData();
		}

		void MarkLastSavedData()
		{
			_lastSavedName = Name;
			_lastSavedUpdateFrequency = UpdateFrequency;
			_lastSavedSearchQuery = SearchQuery.Clone();
		}

		readonly Int32 _projectId;
		String _lastSavedName;
		TimeSpan _lastSavedUpdateFrequency;
		LibrarySearch _lastSavedSearchQuery;
	}
}
