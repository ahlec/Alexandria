using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Bibliothecary.Core.Schema;

namespace Bibliothecary.Core
{
	public sealed class Database : IDisposable
	{
		Database( String filename, SQLiteConnection connection )
		{
			Filename = filename;
			_connection = connection;
		}

		#region IDisposable

		public void Dispose()
		{
			SQLiteConnection connection = Connection;
			connection.Close();
			_connection = null;
		}

		#endregion

		public String Filename { get; }

		public static Database Open( String filename )
		{
			if ( String.IsNullOrWhiteSpace( filename ) )
			{
				throw new ArgumentNullException( nameof( filename ) );
			}

			if ( !File.Exists( filename ) )
			{
				SQLiteConnection.CreateFile( filename );
			}

			String connectionString = String.Concat( "Data Source=", filename, ";Version=3" );
			SQLiteConnection connection = new SQLiteConnection( connectionString );
			connection.Open();

			SchemaUpdater.Update( connection );

			return new Database( filename, connection );
		}

		public IEnumerable<Int32> GetAllProjectIds()
		{
			return Project.GetAllProjectIds( Connection );
		}

		internal SQLiteConnection Connection
		{
			get
			{
				if ( _connection == null )
				{
					throw new InvalidOperationException( $"This {nameof( Database )} has already been disposed!" );
				}

				return _connection;
			}
		}

		SQLiteConnection _connection;
	}
}
