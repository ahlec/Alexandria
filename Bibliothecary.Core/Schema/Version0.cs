using System;
using System.Data.SQLite;

namespace Bibliothecary.Core.Schema
{
	sealed class Version0 : SchemaVersion
	{
		/// <inheritdoc />
		public override Int32 VersionNumber => 0;

		/// <inheritdoc />
		protected override Boolean PerformUpdate( SQLiteConnection connection )
		{
			SQLiteCommand createDbVersion = new SQLiteCommand( "CREATE TABLE db_version( version_number INT )", connection );
			createDbVersion.ExecuteNonQuery();

			SQLiteCommand addDbVersion = new SQLiteCommand( "INSERT INTO db_version( version_number ) VALUES( 0 )", connection );
			Int32 numRowsAffected = addDbVersion.ExecuteNonQuery();
			if ( numRowsAffected != 1 )
			{
				throw new ApplicationException( "Could not insert initial db_version value" );
			}

			return true;
		}
	}
}
