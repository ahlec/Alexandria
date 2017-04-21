using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bibliothecary.Database.Schema;

namespace Bibliothecary.Database
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

			Boolean isCreatingFile = !File.Exists( filename );
			if ( isCreatingFile )
			{
				SQLiteConnection.CreateFile( filename );
			}

			String connectionString = String.Concat( "Data Source=", filename, ";Version=3" );
			SQLiteConnection connection = new SQLiteConnection( connectionString );
			connection.Open();

			if ( isCreatingFile )
			{
				SchemaUpdater.ApplyBaseSchema( connection );
			}
			SchemaUpdater.Update( connection );

			return new Database( filename, connection );
		}

		SQLiteConnection Connection
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
