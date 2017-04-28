using System;
using System.Data.SQLite;
using Bibliothecary.Data.Utils;

namespace Bibliothecary.Data.Schema
{
	internal abstract class SchemaVersion
	{
		public abstract Int32 VersionNumber { get; }

		/// <param name="connection">Should already be validated via <seealso cref="SQLiteUtils.ValidateConnection"/>.</param>
		public Boolean Update( SQLiteConnection connection )
		{
			using ( SQLiteTransaction transaction = connection.BeginTransaction() )
			{
				try
				{
					Boolean succeeded = PerformUpdate( connection );
					if ( succeeded )
					{
						SQLiteCommand updateVersionNumber = new SQLiteCommand( String.Concat( "UPDATE db_version SET version_number = ", VersionNumber ), connection );
						Int32 numRowsAffected = updateVersionNumber.ExecuteNonQuery();
						if ( numRowsAffected != 1 )
						{
							succeeded = false;
						}
					}

					if ( succeeded )
					{
						transaction.Commit();
					}
					else
					{
						transaction.Rollback();
					}
					return succeeded;
				}
				catch
				{
					transaction.Rollback();
					return false;
				}
			}
		}

		protected abstract Boolean PerformUpdate( SQLiteConnection connection );
	}
}
