using System;
using System.Collections.Generic;
using Alexandria.Model;

namespace Alexandria.Searching
{
	public sealed class LibrarySearch
	{
		public String Title { get; set; }

		public String Author { get; set; }

		public DateSearchCriteria Date { get; set; }

		public Boolean OnlyIncludeCompleteFanfics { get; set; }

		public Boolean OnlyIncludeSingleChapterFanfics { get; set; }

		public NumberSearchCriteria WordCount { get; set; }

		public Language? Language { get; set; }

		public IReadOnlyList<String> Fandoms { get; set; }

		public MaturityRating? Rating { get; set; }

		public ContentWarnings? ContentWarnings { get; set; }

		public IReadOnlyList<String> CharacterNames { get; set; }

		public IReadOnlyList<String> Ships { get; set; }

		public IReadOnlyList<String> Tags { get; set; }

		public NumberSearchCriteria NumberLikes { get; set; }

		public NumberSearchCriteria NumberComments { get; set; }

		public SearchField SortField { get; set; }

		public SortDirection SortDirection { get; set; }
	}
}
