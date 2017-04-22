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

			SearchQuery = new LibrarySearch
			{
				Language = Language.English,
				Ships = new List<String>
				{
					"Hiccup Horrendous Haddock III/Jack Frost (Guardians of Childhood)"
				},
				SortField = SearchField.DateLastUpdated,
				SortDirection = SortDirection.Descending
			};
		}

		public static IReadOnlyList<LibrarySource> Sources { get; }

		public static LibrarySearch SearchQuery { get; }
	}
}
