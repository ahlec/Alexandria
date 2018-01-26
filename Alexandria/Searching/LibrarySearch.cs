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
    /// <summary>
    /// A base class for any search for fanfics of a repository. This class allows the user
    /// to search through all of the fanfics provided by a website and filter to a target
    /// set of fanfics based on a complex series of customizable properties.
    /// </summary>
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
        /// Gets the website that this search will be executed against.
        /// </summary>
        public abstract Website Website { get; }

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

        /// <summary>
        /// Gets or sets an optional set of content warning tags that can be requested of all fanfics
        /// that are included in the search results.
        /// <para />
        /// Keeping this property null means that content warnings will not be used to filter any results;
        /// that is, that fanfics with all, none, or any content warnings will be returned. To produce a
        /// search that does NOT contain ANY fanfic listed with content warnings, you can use
        /// <see cref="Model.ContentWarnings.None"/>.
        /// </summary>
        public ContentWarnings? ContentWarnings { get; set; }

        /// <summary>
        /// Gets a mutable list of characters that all fanfics must feature in order to be returned.
        /// The individual character strings do not support keywords or search operators; they must be
        /// a valid character name in full (partial names are not guaranteed to succeed).
        /// </summary>
        public List<string> CharacterNames { get; } = new List<string>();

        /// <summary>
        /// Gets a mutable list of relationships (ships) that all fanfics must feature in order to be
        /// returned. The individual ships do not support keywords or search operators; they must be
        /// a valid ship name (or alias for a ship) in full, as partial names are not guaranteed to
        /// succeed.
        /// </summary>
        public List<string> Ships { get; } = new List<string>();

        /// <summary>
        /// Gets a mutable list of tags (such as tropes, locations, AUs, etc) that all fanfics must
        /// include in order to be returned. The individual tags do not support keywords or search
        /// operators; t hey must be a valid tag name (or alias for a tag) in full, as partial tags
        /// are not guaranteed to succeed.
        /// </summary>
        public List<string> Tags { get; } = new List<string>();

        /// <summary>
        /// Gets or sets a numeric criteria for filtering fanfics by the number of likes that
        /// have been given to the fanfic.
        /// </summary>
        public NumberSearchCriteria NumberLikes { get; set; }

        /// <summary>
        /// Gets or sets a numeric criteria for filtering fanfics by the number of comments that
        /// are posted for the fanfic.
        /// </summary>
        public NumberSearchCriteria NumberComments { get; set; }

        /// <summary>
        /// Gets or sets the method of ordering that should be used for the results of this
        /// search.
        /// </summary>
        public SortField SortField { get; set; }

        /// <summary>
        /// Gets or sets the direction that the results should be ordered in based on the
        /// <see cref="SortField"/>.
        /// </summary>
        public SortDirection SortDirection { get; set; }

        /// <summary>
        /// Performs the search as configured by the properties above. After the search has
        /// been performed, the results returned can continue to query additional pages of
        /// results; the originating <see cref="LibrarySearch"/> can be reused for another
        /// search, or can be released, without affecting the any searches it has already
        /// began.
        /// </summary>
        /// <returns>Returns the first page of search results for the search as configured.</returns>
        public abstract IQueryResultsPage<IFanfic, IFanficRequestHandle> Search();

        /// <summary>
        /// Determines whether this search is equal to another search.
        /// </summary>
        /// <param name="other">The other <see cref="LibrarySearch"/> to compare this instance
        /// to.</param>
        /// <returns>Returns true if both instances have the same configuration of data, or false
        /// if there is some difference between them or if <paramref name="other"/> is null.</returns>
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

            if ( ( Language == null ) != ( other.Language == null ) || ( Language != null && !Language.Equals( other.Language ) ) )
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
