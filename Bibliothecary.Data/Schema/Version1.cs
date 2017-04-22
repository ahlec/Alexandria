using System;
using System.Data.SQLite;

namespace Bibliothecary.Data.Schema
{
	sealed class Version1 : SchemaVersion
	{
		/// <inheritdoc />
		public override Int32 VersionNumber => 1;

		/// <inheritdoc />
		protected override Boolean PerformUpdate( SQLiteConnection connection )
		{
			return true;
		}
	}
}
