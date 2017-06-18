using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using Alexandria.RequestHandles;
using Alexandria.Searching;
using Bibliothecary.Core.DatabaseFunctions;
using Bibliothecary.Core.Publishing;
using Bibliothecary.Core.Utils;

namespace Bibliothecary.Core
{
	public sealed class Project
	{
		const String DefaultProjectName = "Untitled";
		const Int32 DefaultUpdateFrequencyMinutes = 24 * 60; // 1 day
		const Int32 DefaultSearchAO3 = 1; // true, in SQLite
		const Int32 DefaultMaxResultsPerSearch = 10;
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

				if ( _lastSavedMaxResultsPerSearch != MaxResultsPerSearch )
				{
					return true;
				}

				if ( _lastSavedSearchAO3 != SearchAO3 )
				{
					return true;
				}

				if ( !_lastSavedSearchQuery.Equals( SearchQuery ) )
				{
					return true;
				}

				if ( !_lastSavedPublishingInfo.Equals( PublishingInfo ) )
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

		public Int32 MaxResultsPerSearch { get; private set; }

		public Boolean SearchAO3 { get; private set; }

		public LibrarySearch SearchQuery { get; private set; }

		public PublishingInfo PublishingInfo { get; private set; }

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

		public Boolean SetMaxResultsPerSearch( Int32 numberResults )
		{
			if ( numberResults <= 0 )
			{
				throw new ArgumentOutOfRangeException( nameof( numberResults ) );
			}

			if ( numberResults == MaxResultsPerSearch )
			{
				return false;
			}

			MaxResultsPerSearch = numberResults;
			return true;
		}

		public Boolean SetSearchAO3( Boolean value )
		{
			if ( value == SearchAO3 )
			{
				return false;
			}

			SearchAO3 = value;
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
						insertCommand.CommandText = "INSERT INTO projects( project_name, update_frequency_minutes, max_results_per_search, search_ao3 ) VALUES( @name, @frequency, @maxResultsPerSearch, @searchAO3 )";
						insertCommand.Parameters.AddWithValue( "@name", DefaultProjectName );
						insertCommand.Parameters.AddWithValue( "@frequency", DefaultUpdateFrequencyMinutes );
						insertCommand.Parameters.AddWithValue( "@maxResultsPerSearch", DefaultMaxResultsPerSearch );
						insertCommand.Parameters.AddWithValue( "@searchAO3", DefaultSearchAO3 );
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
								MaxResultsPerSearch = DefaultMaxResultsPerSearch,
								SearchAO3 = ( DefaultSearchAO3 != 0 ),
								SearchQuery = new LibrarySearch(),
								PublishingInfo = new PublishingInfo( projectId )
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
			using ( SQLiteCommand selectBasicDataCommand = new SQLiteCommand( @"SELECT
				project_name,
				update_frequency_minutes,
				max_results_per_search,
				search_ao3,
				publishes_email,
				email_sender,
				email_sender_host,
				email_sender_port,
				email_sender_uses_ssl,
				email_sender_uses_credentials,
				email_sender_username,
				email_sender_password,
				email_recipient,
				publishes_tumblr,
				tumblr_consumer_key,
				tumblr_consumer_secret,
				tumblr_oauth_token,
				tumblr_oauth_secret,
				tumblr_blog_name,
				tumblr_queue_posts
				FROM projects WHERE project_id = @projectId", database.Connection ) )
			{
				selectBasicDataCommand.Parameters.AddWithValue( "@projectId", projectId );
				using ( SQLiteDataReader reader = selectBasicDataCommand.ExecuteReader() )
				{
					if ( !reader.Read() )
					{
						throw new ArgumentOutOfRangeException( nameof( projectId ), "No project exists with the specified projectId" );
					}

					parsed.Name = reader.GetStringSafe( 0 );
					Int32 updateFrequencyMinutes = Math.Max( MinimumUpdateFrequencyMinutes, reader.GetInt32( 1 ) );
					parsed.UpdateFrequency = TimeSpan.FromMinutes( updateFrequencyMinutes );
					parsed.MaxResultsPerSearch = reader.GetInt32( 2 );
					parsed.SearchAO3 = ( reader.GetInt32( 3 ) != 0 );

					parsed.PublishingInfo = new PublishingInfo( projectId )
					{
						UsesEmail = ( reader.GetInt32( 4 ) != 0 ),
						UsesTumblr = ( reader.GetInt32( 13 ) != 0 )
					};
					if ( parsed.PublishingInfo.UsesEmail )
					{
						parsed.PublishingInfo.SenderEmail = reader.GetStringSafe( 5 );
						parsed.PublishingInfo.SenderHost = reader.GetStringSafe( 6 );
						parsed.PublishingInfo.SenderPort = reader.GetInt32( 7 );
						parsed.PublishingInfo.DoesSenderUseSsl = ( reader.GetInt32( 8 ) != 0 );
						parsed.PublishingInfo.DoesSenderRequireCredentials = ( reader.GetInt32( 9 ) != 0 );
						if ( parsed.PublishingInfo.DoesSenderRequireCredentials )
						{
							parsed.PublishingInfo.SenderUsername = CryptographyUtils.DecryptString( reader.GetStringSafe( 10 ) );
							parsed.PublishingInfo.SenderPassword = CryptographyUtils.DecryptSecureString( reader.GetStringSafe( 11 ) );
						}
						parsed.PublishingInfo.RecipientEmail = reader.GetStringSafe( 12 );
					}
					if ( parsed.PublishingInfo.UsesTumblr )
					{
						parsed.PublishingInfo.TumblrConsumerKey = reader.GetStringSafe( 14 );
						parsed.PublishingInfo.TumblrConsumerSecret = reader.GetStringSafe( 15 );
						parsed.PublishingInfo.TumblrOauthToken = reader.GetStringSafe( 16 );
						parsed.PublishingInfo.TumblrOauthSecret = reader.GetStringSafe( 17 );
						parsed.PublishingInfo.TumblrBlogName = reader.GetStringSafe( 18 );
						parsed.PublishingInfo.AreTumblrPostsQueued = ( reader.GetInt32( 19 ) != 0 );
					}
				}
			}
			parsed.SearchQuery = LibrarySearchUtils.ReadFromDatabase( database.Connection, projectId );

			if ( parsed.PublishingInfo.UsesTumblr )
			{
				using ( SQLiteCommand selectTumblrTagsCommand = new SQLiteCommand( "SELECT tag FROM tumblr_tags WHERE project_id = @projectId", database.Connection ) )
				{
					selectTumblrTagsCommand.Parameters.AddWithValue( "@projectId", projectId );
					using ( SQLiteDataReader reader = selectTumblrTagsCommand.ExecuteReader() )
					{
						while ( reader.Read() )
						{
							TumblrTagRule tag = new TumblrTagRule( reader.GetStringSafe( 0 ) );
							parsed.PublishingInfo.ConcreteTumblrTags.Add( tag );
						}
					}
				}
			}

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
						updateProjectCommand.CommandText = @"UPDATE projects SET
							project_name = @name,
							update_frequency_minutes = @frequency,
							max_results_per_search = @maxResultsPerSearch,
							search_ao3 = @searchAO3,
							publishes_email = @publishesEmail,
							email_sender = @emailSender,
							email_sender_host = @emailSenderHost,
							email_sender_port = @emailSenderPort,
							email_sender_uses_ssl = @emailSenderUsesSsl,
							email_sender_uses_credentials = @emailSenderUsesCredentials,
							email_sender_username = @emailSenderUsername,
							email_sender_password = @emailSenderPassword,
							email_recipient = @emailRecipient,
							publishes_tumblr = @publishesTumblr,
							tumblr_consumer_key = @tumblrConsumerKey,
							tumblr_consumer_secret = @tumblrConsumerSecret,
							tumblr_oauth_token = @tumblrOauthToken,
							tumblr_oauth_secret = @tumblrOauthSecret,
							tumblr_blog_name = @tumblrBlogName,
							tumblr_queue_posts = @tumblrQueuePosts
							WHERE project_id = @projectId";
						updateProjectCommand.Parameters.AddWithValue( "@projectId", ProjectId );
						updateProjectCommand.Parameters.AddWithValue( "@name", Name );
						updateProjectCommand.Parameters.AddWithValue( "@frequency", UpdateFrequency.TotalMinutes );
						updateProjectCommand.Parameters.AddWithValue( "@maxResultsPerSearch", MaxResultsPerSearch );
						updateProjectCommand.Parameters.AddWithValue( "@searchAO3", ( SearchAO3 ? 1 : 0 ) );
						updateProjectCommand.Parameters.AddWithValue( "@publishesEmail", ( PublishingInfo.UsesEmail ? 1 : 0 ) );
						updateProjectCommand.Parameters.AddWithValue( "@publishesTumblr", ( PublishingInfo.UsesTumblr ? 1 : 0 ) );

						String emailSender = null;
						String emailSenderHost = null;
						Int32 emailSenderPort = 0;
						Int32 emailSenderUsesSsl = 0;
						Int32 emailSenderUsesCredentials = 0;
						String emailSenderUsernameEncrypted = null;
						String emailSenderPasswordEncrpyted = null;
						String emailRecipient = null;
						if ( PublishingInfo.UsesEmail )
						{
							emailSender = PublishingInfo.SenderEmail;
							emailSenderHost = PublishingInfo.SenderHost;
							emailSenderPort = PublishingInfo.SenderPort;
							emailSenderUsesSsl = ( PublishingInfo.DoesSenderUseSsl ? 1 : 0 );
							emailSenderUsesCredentials = ( PublishingInfo.DoesSenderRequireCredentials ? 1 : 0 );
							if ( PublishingInfo.DoesSenderRequireCredentials )
							{
								emailSenderUsernameEncrypted = CryptographyUtils.EncryptString( PublishingInfo.SenderUsername );
								emailSenderPasswordEncrpyted = CryptographyUtils.EncryptSecureString( PublishingInfo.SenderPassword );
							}
							emailRecipient = PublishingInfo.RecipientEmail;
						}

						String tumblrConsumerKey = null;
						String tumblrConsumerSecret = null;
						String tumblrOauthToken = null;
						String tumblrOauthSecret = null;
						String tumblrBlogName = null;
						Int32 tumblrQueuePosts = 0;
						if ( PublishingInfo.UsesTumblr )
						{
							tumblrConsumerKey = PublishingInfo.TumblrConsumerKey;
							tumblrConsumerSecret = PublishingInfo.TumblrConsumerSecret;
							tumblrOauthToken = PublishingInfo.TumblrOauthToken;
							tumblrOauthSecret = PublishingInfo.TumblrOauthSecret;
							tumblrBlogName = PublishingInfo.TumblrBlogName;
							tumblrQueuePosts = ( PublishingInfo.AreTumblrPostsQueued ? 1 : 0 );
						}

						updateProjectCommand.Parameters.AddWithValue( "@emailSender", emailSender );
						updateProjectCommand.Parameters.AddWithValue( "@emailSenderHost", emailSenderHost );
						updateProjectCommand.Parameters.AddWithValue( "@emailSenderPort", emailSenderPort );
						updateProjectCommand.Parameters.AddWithValue( "@emailSenderUsesSsl", emailSenderUsesSsl );
						updateProjectCommand.Parameters.AddWithValue( "@emailSenderUsesCredentials", emailSenderUsesCredentials );
						updateProjectCommand.Parameters.AddWithValue( "@emailSenderUsername", emailSenderUsernameEncrypted );
						updateProjectCommand.Parameters.AddWithValue( "@emailSenderPassword", emailSenderPasswordEncrpyted );
						updateProjectCommand.Parameters.AddWithValue( "@emailRecipient", emailRecipient );
						updateProjectCommand.Parameters.AddWithValue( "@tumblrConsumerKey", tumblrConsumerKey );
						updateProjectCommand.Parameters.AddWithValue( "@tumblrConsumerSecret", tumblrConsumerSecret );
						updateProjectCommand.Parameters.AddWithValue( "@tumblrOauthToken", tumblrOauthToken );
						updateProjectCommand.Parameters.AddWithValue( "@tumblrOauthSecret", tumblrOauthSecret );
						updateProjectCommand.Parameters.AddWithValue( "@tumblrBlogName", tumblrBlogName );
						updateProjectCommand.Parameters.AddWithValue( "@tumblrQueuePosts", tumblrQueuePosts );

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

					HashSet<TumblrTagRule> lastSavedTumblrTags = new HashSet<TumblrTagRule>( _lastSavedPublishingInfo.TumblrTags, new TumblrTagRuleContentEqualityComparer() );
					if ( PublishingInfo.UsesTumblr )
					{
						foreach ( TumblrTagRule tag in PublishingInfo.TumblrTags )
						{
							if ( lastSavedTumblrTags.Remove( tag ) )
							{
								// This configuration is already in the database
								continue;
							}

							using ( SQLiteCommand insertTagCommand = new SQLiteCommand( _database.Connection ) )
							{
								insertTagCommand.CommandText = "INSERT INTO tumblr_tags( project_id, tag ) VALUES( @projectId, @tagText )";
								insertTagCommand.Parameters.AddWithValue( "@projectId", ProjectId );
								insertTagCommand.Parameters.AddWithValue( "@tagText", tag.Tag );
								Int32 numberRowsAffected = insertTagCommand.ExecuteNonQuery();
								if ( numberRowsAffected != 1 )
								{
									throw new ApplicationException( $"Unable to insert a {nameof( TumblrTagRule )} into tumblr_tags ( tag = {tag.Tag} )" );
								}
							}
						}
					}

					foreach ( TumblrTagRule removedTag in lastSavedTumblrTags )
					{
						using ( SQLiteCommand deleteTagCommand = new SQLiteCommand( _database.Connection ) )
						{
							deleteTagCommand.CommandText = "DELETE FROM tumblr_tags WHERE project_id = @projectId AND tag = @tagText";
							deleteTagCommand.Parameters.AddWithValue( "@projectId", ProjectId );
							deleteTagCommand.Parameters.AddWithValue( "@tagText", removedTag.Tag );
							Int32 numberRowsAffected = deleteTagCommand.ExecuteNonQuery();
							if ( numberRowsAffected != 1 )
							{
								throw new ApplicationException( $"Unable to delete a {nameof( TumblrTagRule )} from tumblr_tags ( tag = {removedTag.Tag}, project_id = {ProjectId} )" );
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
					using ( SQLiteCommand deleteFromTumblrTagsCommand = new SQLiteCommand( _database.Connection ) )
					{
						deleteFromTumblrTagsCommand.CommandText = "DELETE FROM tumblr_tags WHERE project_id = @projectId";
						deleteFromTumblrTagsCommand.Parameters.AddWithValue( "@projectId", ProjectId );
						deleteFromTumblrTagsCommand.ExecuteNonQuery();
					}

					using ( SQLiteCommand deleteFromReportedQueryResultsCommand = new SQLiteCommand( _database.Connection ) )
					{
						deleteFromReportedQueryResultsCommand.CommandText = "DELETE FROM project_reported_query_results WHERE project_id = @projectId";
						deleteFromReportedQueryResultsCommand.Parameters.AddWithValue( "@projectId", ProjectId );
						deleteFromReportedQueryResultsCommand.ExecuteNonQuery();
					}

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

		/// <summary>
		/// Filters through <paramref name="fanfics"/> and returns only the fanfics which have not previously been marked as reported
		/// through this project.
		/// </summary>
		/// <param name="fanfics">An enumeration of fanfic handles</param>
		/// <param name="source">A string constant representation that identifies which source (which website) these fanfics are from.</param>
		public IEnumerable<IFanficRequestHandle> FilterUnreportedQueryResults( IEnumerable<IFanficRequestHandle> fanfics, String source )
		{
			StringBuilder valuesList = new StringBuilder();
			Boolean isFirstFanfic = true;
			Dictionary<String, IFanficRequestHandle> fanficsLookup = new Dictionary<String, IFanficRequestHandle>();
			foreach ( IFanficRequestHandle fanfic in fanfics )
			{
				if ( isFirstFanfic )
				{
					isFirstFanfic = false;
				}
				else
				{
					valuesList.Append( ", " );
				}
				valuesList.Append( "( '" );
				valuesList.Append( fanfic.Handle );
				valuesList.Append( "' )" );

				fanficsLookup.Add( fanfic.Handle, fanfic );
			}

			using ( SQLiteCommand selectUnreportedCommand = new SQLiteCommand( _database.Connection ) )
			{
				selectUnreportedCommand.CommandText = String.Concat( "SELECT * FROM ( VALUES ", valuesList,
					" ) EXCEPT SELECT fanfic_handle FROM project_reported_query_results WHERE project_id = @projectId AND source = @source;" );
				selectUnreportedCommand.Parameters.AddWithValue( "@projectId", ProjectId );
				selectUnreportedCommand.Parameters.AddWithValue( "@source", source );

				using ( SQLiteDataReader reader = selectUnreportedCommand.ExecuteReader() )
				{
					while ( reader.Read() )
					{
						String fanficHandle = reader.GetStringSafe( 0 );
						yield return fanficsLookup[fanficHandle];
					}
				}
			}
		}

		public void MarkFanficsAsReported( IEnumerable<IFanficRequestHandle> fanfics, String source )
		{
			using ( SQLiteTransaction transaction = _database.Connection.BeginTransaction() )
			{
				try
				{
					foreach ( IFanficRequestHandle fanfic in fanfics )
					{
						using ( SQLiteCommand insertCommand = new SQLiteCommand( _database.Connection ) )
						{
							insertCommand.CommandText = "INSERT INTO project_reported_query_results( project_id, source, fanfic_handle ) VALUES( @projectId, @fanficSource, @fanficHandle )";
							insertCommand.Parameters.AddWithValue( "@projectId", ProjectId );
							insertCommand.Parameters.AddWithValue( "@fanficSource", source );
							insertCommand.Parameters.AddWithValue( "@fanficHandle", fanfic.Handle );
							Int32 numberRowsAffected = insertCommand.ExecuteNonQuery();
							if ( numberRowsAffected != 1 )
							{
								throw new ApplicationException( "Unable to update the project in the database!" );
							}
						}
					}

					transaction.Commit();
				}
				catch ( Exception )
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		void MarkLastSavedData()
		{
			_lastSavedName = Name;
			_lastSavedUpdateFrequency = UpdateFrequency;
			_lastSavedMaxResultsPerSearch = MaxResultsPerSearch;
			_lastSavedSearchAO3 = SearchAO3;
			_lastSavedSearchQuery = SearchQuery.Clone();
			_lastSavedPublishingInfo = PublishingInfo.Clone();
		}

		readonly Database _database;
		String _lastSavedName;
		TimeSpan _lastSavedUpdateFrequency;
		Int32 _lastSavedMaxResultsPerSearch;
		Boolean _lastSavedSearchAO3;
		LibrarySearch _lastSavedSearchQuery;
		PublishingInfo _lastSavedPublishingInfo;
	}
}
