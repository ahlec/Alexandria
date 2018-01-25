// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.Languages;
using Alexandria.Model;
using Alexandria.Querying;
using Alexandria.RequestHandles;

namespace Alexandria.Searching
{
    public abstract class LibrarySearch : IEquatable<LibrarySearch>
    {
        static uint _nextInternalId = 1;
        readonly uint _internalId;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibrarySearch"/> class.
        /// </summary>
        protected LibrarySearch()
        {
            _internalId = _nextInternalId++;
        }

        /// <summary>
        /// Gets or sets text that should be compared against the title of fanfics
        /// to narrow down the search results. Different websites might support different
        /// operators and keywords in this field.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a string query that should be applied to authors of fanfics
        /// in order to narrow down the search results. Different websites might support
        /// different operators and keywords in this field.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets a date criteria for filtering fanfics by the date that it was last
        /// updated (or posted, if the fanfic has not been edited or added to since it was
        /// first posted).
        /// </summary>
        public DateSearchCriteria Date { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only fanfics that have been marked by
        /// the author as being completed should be the only fanfics returned (true) or if
        /// fanfics which are still in progress can also be included in the search results (false).
        /// </summary>
        public bool OnlyIncludeCompleteFanfics { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only fanfics which are only a single
        /// standalone chapter should be the only fanfics returned (true) or if fanfics which
        /// are more than one chapter can also be included in the search results (false).
        /// </summary>
        public bool OnlyIncludeSingleChapterFanfics { get; set; }

        /// <summary>
        /// Gets or sets a numeric search criteria for filtering fanfics by how many words they
        /// contain.
        /// </summary>
        public NumberSearchCriteria WordCount { get; set; }

        /// <summary>
        /// Gets or sets the specific language that fanfics should be filtered against.
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Gets a mutable list of fandoms that all fanfics must contain in order to be returned.
        /// The individual fandom strings do not support keywords or search operators; they must be
        /// a valid name of a fandom in full.
        /// </summary>
        public List<string> Fandoms { get; } = new List<string>();

        /// <summary>
        /// Gets or sets an optional rating that fanfics must be in order to be included in the
        /// search results. However, this is an EXACT rating requirement; specifying a higher
        /// maturity rating will not include more general/lower maturity ratings.
        /// </summary>
        public MaturityRating? Rating { get; set; }

        public ContentWarnings ContentWarnings { get; set; }

        public List<string> CharacterNames { get; private set; } = new List<string>();

        public List<string> Ships { get; private set; } = new List<string>();

        public List<string> Tags { get; private set; } = new List<string>();

        public NumberSearchCriteria NumberLikes { get; set; }

        public NumberSearchCriteria NumberComments { get; set; }

        public SearchField SortField { get; set; }

        public SortDirection SortDirection { get; set; }

        public abstract IQueryResultsPage<IFanfic, IFanficRequestHandle> Search();

        public bool Equals( LibrarySearch other )
        {
            if ( other == null )
            {
                return false;
            }

            if ( !string.Equals( Title, other.Title, StringComparison.CurrentCultureIgnoreCase ) )
            {
                return false;
            }

            if ( !string.Equals( Author, other.Author, StringComparison.CurrentCultureIgnoreCase ) )
            {
                return false;
            }

            if ( ( Date == null ) != ( other.Date == null ) || ( Date != null && !Date.Equals( other.Date ) ) )
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

            if ( ( WordCount == null ) != ( other.WordCount == null ) || ( WordCount != null && !WordCount.Equals( other.WordCount ) ) )
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

            if ( ( NumberLikes == null ) != ( other.NumberLikes == null ) || ( NumberLikes != null && !NumberLikes.Equals( other.NumberLikes ) ) )
            {
                return false;
            }

            if ( ( NumberComments == null ) != ( other.NumberComments == null ) || ( NumberComments != null && !NumberComments.Equals( other.NumberComments ) ) )
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
        public override bool Equals( object obj )
        {
            if ( obj == null )
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

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _internalId.GetHashCode();
        }
    }
}
