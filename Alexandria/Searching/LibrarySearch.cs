using System;
using System.Collections.Generic;
using System.Linq;
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

		public List<String> Fandoms { get; private set; } = new List<String>();

		public MaturityRating? Rating { get; set; }

		public ContentWarnings? ContentWarnings { get; set; }

		public List<String> CharacterNames { get; private set; } = new List<String>();

		public List<String> Ships { get; private set; } = new List<String>();

		public List<String> Tags { get; private set; } = new List<String>();

		public NumberSearchCriteria NumberLikes { get; set; }

		public NumberSearchCriteria NumberComments { get; set; }

		public SearchField SortField { get; set; }

		public SortDirection SortDirection { get; set; }

		public LibrarySearch Clone()
		{
			return new LibrarySearch
			{
				Title = Title,
				Author = Author,
				Date = Date?.Clone(),
				OnlyIncludeCompleteFanfics = OnlyIncludeCompleteFanfics,
				OnlyIncludeSingleChapterFanfics = OnlyIncludeSingleChapterFanfics,
				WordCount = WordCount?.Clone(),
				Language = Language,
				Fandoms = Fandoms.ToList(),
				Rating = Rating,
				ContentWarnings = ContentWarnings,
				CharacterNames = CharacterNames.ToList(),
				Ships = Ships.ToList(),
				Tags = Tags.ToList(),
				NumberLikes = NumberLikes?.Clone(),
				NumberComments = NumberComments?.Clone(),
				SortField = SortField,
				SortDirection = SortDirection
			};
		}
	}
}
