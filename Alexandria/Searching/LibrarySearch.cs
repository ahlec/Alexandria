using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.Model;

namespace Alexandria.Searching
{
	public sealed class LibrarySearch : IEquatable<LibrarySearch>
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

		public Boolean Equals( LibrarySearch other )
		{
			if ( other == null )
			{
				return false;
			}

			if ( !String.Equals( Title, other.Title, StringComparison.CurrentCultureIgnoreCase ) )
			{
				return false;
			}

			if ( !String.Equals( Author, other.Author, StringComparison.CurrentCultureIgnoreCase ) )
			{
				return false;
			}

			if ( ( Date == null ) != ( other.Date == null ) || Date?.Equals( other.Date ) != true )
			{
				return false;
			}

			if ( OnlyIncludeCompleteFanfics != other.OnlyIncludeCompleteFanfics )
			{
				return false;
			}

			if ( OnlyIncludeSingleChapterFanfics != other.OnlyIncludeSingleChapterFanfics )
			{
				return false;
			}

			if ( ( WordCount == null ) != ( other.WordCount == null ) || WordCount?.Equals( other.WordCount ) != true )
			{
				return false;
			}

			if ( Language != other.Language )
			{
				return false;
			}

			if ( Fandoms.Count != other.Fandoms.Count || Fandoms.Except( other.Fandoms, StringComparer.CurrentCultureIgnoreCase ).Any() )
			{
				return false;
			}

			if ( Rating != other.Rating )
			{
				return false;
			}

			if ( ContentWarnings != other.ContentWarnings )
			{
				return false;
			}

			if ( CharacterNames.Count != other.CharacterNames.Count || CharacterNames.Except( other.CharacterNames, StringComparer.CurrentCultureIgnoreCase ).Any() )
			{
				return false;
			}

			if ( Ships.Count != other.Ships.Count || Ships.Except( other.Ships, StringComparer.CurrentCultureIgnoreCase ).Any() )
			{
				return false;
			}

			if ( Tags.Count != other.Tags.Count || Tags.Except( other.Tags, StringComparer.CurrentCultureIgnoreCase ).Any() )
			{
				return false;
			}

			if ( ( NumberLikes == null ) != ( other.NumberLikes == null ) || NumberLikes?.Equals( other.NumberLikes ) != true )
			{
				return false;
			}

			if ( ( NumberComments == null ) != ( other.NumberComments == null ) || NumberComments?.Equals( other.NumberComments ) != true )
			{
				return false;
			}

			if ( SortField != other.SortField )
			{
				return false;
			}

			if ( SortDirection != other.SortDirection )
			{
				return false;
			}

			return true;
		}

		/// <inheritdoc />
		public override Boolean Equals( Object obj )
		{
			if ( ReferenceEquals( obj, null ) )
			{
				return false;
			}

			if ( ReferenceEquals( obj, this ) )
			{
				return true;
			}

			LibrarySearch other = obj as LibrarySearch;
			return Equals( other );
		}
	}
}
