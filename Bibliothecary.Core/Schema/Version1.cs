using System;
using System.Data.SQLite;

namespace Bibliothecary.Core.Schema
{
	sealed class Version1 : SchemaVersion
	{
		/// <inheritdoc />
		public override Int32 VersionNumber => 1;

		/// <inheritdoc />
		protected override Boolean PerformUpdate( SQLiteConnection connection )
		{
			SQLiteCommand createProjects = new SQLiteCommand( @"CREATE TABLE projects (
					project_id                    INTEGER PRIMARY KEY,
					project_name                  VARCHAR NOT NULL,
					update_frequency_minutes      INTEGER NOT NULL CHECK ( update_frequency_minutes > 0 ),
					max_results_per_search        INTEGER CHECK ( max_results_per_search > 0 ) NOT NULL DEFAULT ( 10 ),
					search_ao3                    INTEGER NOT NULL DEFAULT ( 0 ),
					publishes_email               INTEGER NOT NULL DEFAULT ( 0 ),
					email_sender                  VARCHAR,
					email_sender_host             VARCHAR,
					email_sender_port             INTEGER CHECK ( email_sender_port >= 0 ),
					email_sender_uses_ssl         INTEGER DEFAULT (0),
					email_sender_uses_credentials INTEGER DEFAULT ( 0 ),
					email_sender_username         VARCHAR, email_sender_password VARCHAR,
					email_recipient               VARCHAR,
					publishes_tumblr              INTEGER NOT NULL DEFAULT (0),
					tumblr_consumer_key           VARCHAR,
					tumblr_consumer_secret        VARCHAR,
					tumblr_oauth_token            VARCHAR,
					tumblr_oauth_secret           VARCHAR,
					tumblr_blog_name              VARCHAR,
					tumblr_queue_posts            INTEGER );", connection );
			createProjects.ExecuteNonQuery();

			SQLiteCommand createProjectSearchFields = new SQLiteCommand( @"CREATE TABLE project_search_fields (
					project_id INTEGER REFERENCES projects (project_id), field_name VARCHAR NOT NULL, field_value VARCHAR NOT NULL );", connection );
			createProjectSearchFields.ExecuteNonQuery();

			SQLiteCommand createProjectReportedQueryResults = new SQLiteCommand( @"CREATE TABLE project_reported_query_results(
				project_id INTEGER REFERENCES projects( project_id ) NOT NULL, source VARCHAR NOT NULL, fanfic_handle VARCHAR NOT NULL UNIQUE );", connection );
			createProjectReportedQueryResults.ExecuteNonQuery();

			SQLiteCommand createTumblrTags = new SQLiteCommand( @"CREATE TABLE tumblr_tags (
					project_id INTEGER REFERENCES projects ( project_id ),
					tag        TEXT    NOT NULL );", connection );
			createTumblrTags.ExecuteNonQuery();

			return true;
		}
	}
}
