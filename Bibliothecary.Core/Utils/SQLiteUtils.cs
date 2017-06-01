using System;
using System.Data.SQLite;

namespace Bibliothecary.Core.Utils
{
	// ReSharper disable once InconsistentNaming
	// It's the name of a library
	public static class SQLiteUtils
	{
		public static void ValidateConnection( SQLiteConnection connection )
		{
			if ( connection == null )
			{
				throw new ArgumentNullException( nameof( connection ) );
			}
		}
	}
}
