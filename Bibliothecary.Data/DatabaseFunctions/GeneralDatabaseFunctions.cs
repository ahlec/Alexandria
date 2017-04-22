using System;
using System.Data.SQLite;
using Bibliothecary.Data.Utils;

namespace Bibliothecary.Data.DatabaseFunctions
{
	public static class GeneralDatabaseFunctions
	{
		public static Int32 GetDatabaseVersionNumber( this SQLiteConnection connection )
		{
			SQLiteUtils.ValidateConnection( connection );

			try
			{
				SQLiteCommand command = new SQLiteCommand( "SELECT version_number FROM db_version", connection );
				Object versionObj = command.ExecuteScalar();
				return (Int32) (Int64) versionObj;
			}
			catch ( Exception ex )
			{
				if ( IsDbVersionTableNotExistingError( ex ) )
				{
					return -1;
				}

				throw;
			}
		}

		static Boolean IsDbVersionTableNotExistingError( Exception e )
		{
			SQLiteException ex = e as SQLiteException;
			if ( ex?.ErrorCode != (Int32) SQLiteErrorCode.Error )
			{
				return false;
			}

			return ex.Message.EndsWith( "no such table: db_version" );
		}
	}
}
