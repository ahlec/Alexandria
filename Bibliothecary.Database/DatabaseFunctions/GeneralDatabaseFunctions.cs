using System;
using System.Data.SQLite;
using Bibliothecary.Database.Utils;

namespace Bibliothecary.Database.DatabaseFunctions
{
	public static class GeneralDatabaseFunctions
	{
		public static Int32 GetDatabaseVersionNumber( this SQLiteConnection connection )
		{
			SQLiteUtils.ValidateConnection( connection );
			SQLiteCommand command = new SQLiteCommand( "SELECT version_number FROM db_version", connection );
			Object versionObj = command.ExecuteScalar();
			return (Int32) (Int64) versionObj;
		}
	}
}
