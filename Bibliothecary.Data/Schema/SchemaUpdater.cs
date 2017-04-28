using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Bibliothecary.Data.DatabaseFunctions;
using Bibliothecary.Data.Utils;

namespace Bibliothecary.Data.Schema
{
	internal static class SchemaUpdater
	{
		public static Boolean Update( SQLiteConnection connection )
		{
			SQLiteUtils.ValidateConnection( connection );
			Int32 currentVersion = connection.GetDatabaseVersionNumber();
			if ( currentVersion < -1 )
			{
				throw new ArgumentOutOfRangeException( nameof( currentVersion ) );
			}
			if ( currentVersion > CurrentVersionNumber )
			{
				throw new ArgumentException( $"Database is at a higher version than this library is at (database version = {currentVersion}, library version = {CurrentVersionNumber})" );
			}

			for ( Int32 version = currentVersion + 1; version <= CurrentVersionNumber; ++version )
			{
				if ( !_versions[version].Update( connection ) )
				{
					return false;
				}
			}
			return true;
		}

		const Int32 CurrentVersionNumber = 1;
		static readonly IReadOnlyList<SchemaVersion> _versions = new List<SchemaVersion>
		{
			new Version0(),
			new Version1()
		};
	}
}
