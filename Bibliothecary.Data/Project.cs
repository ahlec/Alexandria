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

		Project( Database database, Int32 projectId )
		{
			_database = database;
			ProjectId = projectId;
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

		public Int32 ProjectId { get; }

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

		public static Project Create( Database database )
		{
			if ( database == null )
			{
				throw new ArgumentNullException( nameof( database ) );
			}
			SQLiteUtils.ValidateConnection( database.Connection );

			using ( SQLiteTransaction transaction = database.Connection.BeginTransaction() )
			{
				try
				{
					using ( SQLiteCommand insertCommand = new SQLiteCommand( database.Connection ) )
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
					using ( SQLiteCommand getProjectId = new SQLiteCommand( "SELECT last_insert_rowid()", database.Connection ) )
					{
						using ( SQLiteDataReader reader = getProjectId.ExecuteReader() )
						{
							if ( !reader.Read() )
							{
								throw new ApplicationException( "No result from last_insert_rowid?" );
							}
							Int32 projectId = reader.GetInt32( 0 );
							newProject = new Project( database, projectId )
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

		public static Project Read( Database database, Int32 projectId )
		{
			if ( database == null )
			{
				throw new ArgumentNullException( nameof( database ) );
			}
			SQLiteUtils.ValidateConnection( database.Connection );

			Project parsed = new Project( database, projectId );
			using ( SQLiteCommand selectBasicDataCommand = new SQLiteCommand( "SELECT project_name, update_frequency_minutes FROM projects WHERE project_id = @projectId", database.Connection ) )
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
			parsed.SearchQuery = LibrarySearchUtils.ReadFromDatabase( database.Connection, projectId );

			parsed.MarkLastSavedData();

			return parsed;
		}

		public void Save()
		{
			using ( SQLiteTransaction transaction = _database.Connection.BeginTransaction() )
			{
				try
				{
					using ( SQLiteCommand updateProjectCommand = new SQLiteCommand( _database.Connection ) )
					{
						updateProjectCommand.CommandText = "UPDATE projects SET project_name = @name, update_frequency_minutes = @frequency WHERE project_id = @projectId";
						updateProjectCommand.Parameters.AddWithValue( "@projectId", ProjectId );
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

						using ( SQLiteCommand insertFieldCommand = new SQLiteCommand( _database.Connection ) )
						{
							insertFieldCommand.CommandText = "INSERT INTO project_search_fields( project_id, field_name, field_value ) VALUES( @projectId, @fieldName, @fieldValue )";
							insertFieldCommand.Parameters.AddWithValue( "@projectId", ProjectId );
							insertFieldCommand.Parameters.AddWithValue( "@fieldName", fieldSql.Key );
							insertFieldCommand.Parameters.AddWithValue( "@fieldValue", fieldSql.Value );
							Int32 numberRowsAffected = insertFieldCommand.ExecuteNonQuery();
							if ( numberRowsAffected != 1 )
							{
								throw new ApplicationException( $"Unable to insert a project_search_field of key '{fieldSql.Key}' and value '{fieldSql.Value}' into project {ProjectId}" );
							}
						}
					}

					foreach ( KeyValuePair<String, String> removedField in lastSavedSearchSql )
					{
						using ( SQLiteCommand deleteFieldCommand = new SQLiteCommand( _database.Connection ) )
						{
							deleteFieldCommand.CommandText = "DELETE FROM project_search_fields WHERE project_id = @projectId AND field_name = @fieldName AND field_value = @fieldValue";
							deleteFieldCommand.Parameters.AddWithValue( "@projectId", ProjectId );
							deleteFieldCommand.Parameters.AddWithValue( "@fieldName", removedField.Key );
							deleteFieldCommand.Parameters.AddWithValue( "@fieldValue", removedField.Value );
							Int32 numberRowsAffected = deleteFieldCommand.ExecuteNonQuery();
							if ( numberRowsAffected != 1 )
							{
								throw new ApplicationException( $"Unable to delete a project_search_field of key '{removedField.Key}' and value '{removedField.Value}' from project {ProjectId}" );
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

		public Boolean Delete()
		{
			using ( SQLiteTransaction transaction = _database.Connection.BeginTransaction() )
			{
				try
				{
					using ( SQLiteCommand deleteFromProjectsCommand = new SQLiteCommand( _database.Connection ) )
					{
						deleteFromProjectsCommand.CommandText = "DELETE FROM projects WHERE project_id = @projectId";
						deleteFromProjectsCommand.Parameters.AddWithValue( "@projectId", ProjectId );
						Int32 numberRowsAffected = deleteFromProjectsCommand.ExecuteNonQuery();
						if ( numberRowsAffected != 1 )
						{
							throw new ApplicationException( "Unable to update the project in the database!" );
						}
					}

					using ( SQLiteCommand deleteFromProjectSearchFieldsCommand = new SQLiteCommand( _database.Connection ) )
					{
						deleteFromProjectSearchFieldsCommand.CommandText = "DELETE FROM project_search_fields WHERE project_id = @projectId";
						deleteFromProjectSearchFieldsCommand.Parameters.AddWithValue( "@projectId", ProjectId );
						deleteFromProjectSearchFieldsCommand.ExecuteNonQuery();
					}

					transaction.Commit();
					return true;
				}
				catch
				{
					transaction.Rollback();
					return false;
				}
			}
		}

		void MarkLastSavedData()
		{
			_lastSavedName = Name;
			_lastSavedUpdateFrequency = UpdateFrequency;
			_lastSavedSearchQuery = SearchQuery.Clone();
		}

		readonly Database _database;
		String _lastSavedName;
		TimeSpan _lastSavedUpdateFrequency;
		LibrarySearch _lastSavedSearchQuery;
	}
}
