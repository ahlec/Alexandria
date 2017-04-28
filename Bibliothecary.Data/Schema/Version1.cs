using System;
using System.Data.SQLite;

namespace Bibliothecary.Data.Schema
{
	sealed class Version1 : SchemaVersion
	{
		/// <inheritdoc />
		public override Int32 VersionNumber => 1;

		/// <inheritdoc />
		protected override Boolean PerformUpdate( SQLiteConnection connection )
		{
			SQLiteCommand createProjects = new SQLiteCommand( @"CREATE TABLE projects ( project_id INTEGER PRIMARY KEY,
					project_name VARCHAR NOT NULL, update_frequency_minutes INT NOT NULL CHECK (update_frequency_minutes > 0) );", connection );
			createProjects.ExecuteNonQuery();

			SQLiteCommand createProjectSearchFields = new SQLiteCommand( @"CREATE TABLE project_search_fields (
					project_id  INTEGER REFERENCES projects (project_id), field_name  VARCHAR NOT NULL, field_value VARCHAR NOT NULL );", connection );
			createProjectSearchFields.ExecuteNonQuery();

			return true;
		}
	}
}
