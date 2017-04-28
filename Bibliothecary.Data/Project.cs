using System;
using System.Data.SQLite;
using Alexandria.Searching;
using Bibliothecary.Data.Utils;

namespace Bibliothecary.Data
{
	public sealed class Project
	{
		const String DefaultProjectName = "Untitled";
		const Int32 DefaultUpdateFrequencyMinutes = 24 * 60; // 1 day
		const Int32 MinimumUpdateFrequencyMinutes = 1; // I mean, really??

		Project( Int32 projectId )
		{
			_projectId = projectId;
		}

		public String Name { get; private set; }

		public TimeSpan UpdateFrequency { get; private set; }

		public LibrarySearch SearchQuery { get; private set; }

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

		internal void Save()
		{
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
