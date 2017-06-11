using System;
using System.Data.SQLite;

namespace Bibliothecary.Core.DatabaseFunctions
{
	internal static class DataReaderExtensions
	{
		public static String GetStringSafe( this SQLiteDataReader reader, Int32 i )
		{
			if ( reader.IsDBNull( i ) )
			{
				return null;
			}

			return reader.GetString( i );
		}
	}
}
