using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bibliothecary.Database.DatabaseFunctions;
using Bibliothecary.Database.Utils;

namespace Bibliothecary.Database.Schema
{
	internal static class SchemaUpdater
	{
		public static void ApplyBaseSchema( SQLiteConnection connection )
		{
			SQLiteUtils.ValidateConnection( connection );
			String sql = GetSqlForVersion( 0 );
			SQLiteCommand command = new SQLiteCommand( sql, connection );
			command.ExecuteNonQuery();
		}

		public static void Update( SQLiteConnection connection )
		{
			SQLiteUtils.ValidateConnection( connection );
			Int32 currentVersion = connection.GetDatabaseVersionNumber();
			if ( currentVersion > CurrentVersionNumber )
			{
				throw new ArgumentException( $"Database is at a higher version than this library is at (database version = {currentVersion}, library version = {CurrentVersionNumber})" );
			}

			SQLiteCommand command = new SQLiteCommand( connection );
			for ( Int32 version = currentVersion + 1; version <= CurrentVersionNumber; ++version )
			{
				String sql = GetSqlForVersion( version );
				command.CommandText = sql;
				command.ExecuteNonQuery();
			}
		}

		static String GetSqlForVersion( Int32 versionNumber )
		{
			if ( versionNumber < 0 || versionNumber > CurrentVersionNumber )
			{
				throw new ArgumentOutOfRangeException( nameof( versionNumber ) );
			}

			String resourceName = String.Concat( "Bibliothecary.Database.Schema.Version", versionNumber, ".sql" );
			using ( Stream resourceStream = _assembly.GetManifestResourceStream( resourceName ) )
			{
				if ( resourceStream == null )
				{
					throw new ApplicationException( $"Could not find the database update script for version {versionNumber}!" );
				}

				using ( StreamReader reader = new StreamReader( resourceStream ) )
				{
					return reader.ReadToEnd();
				}
			}
		}

		const Int32 CurrentVersionNumber = 1;

		static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
	}
}
