using System;
using System.Collections.Generic;
using Alexandria;
using Alexandria.AO3;
using Alexandria.Model;
using Alexandria.Searching;

namespace Bibliothecary
{
	public static class Config
	{
		static Config()
		{
			Sources = new List<LibrarySource>
			{
				new AO3Source( LibrarySourceConfig.Default )
			};
		}

		public static IReadOnlyList<LibrarySource> Sources { get; }
	}
}
