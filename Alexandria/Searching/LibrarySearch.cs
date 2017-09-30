// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.Model;
using Alexandria.RequestHandles;

namespace Alexandria.Searching
{
    public abstract class LibrarySearch : IEquatable<LibrarySearch>
    {
        protected LibrarySearch()
        {
            _internalId = _nextInternalId++;
        }

        public string Title { get; set; }

        public string Author { get; set; }

        public DateSearchCriteria Date { get; set; }

        public bool OnlyIncludeCompleteFanfics { get; set; }

        public bool OnlyIncludeSingleChapterFanfics { get; set; }

        public NumberSearchCriteria WordCount { get; set; }

        public Language? Language { get; set; }

        public List<string> Fandoms { get; private set; } = new List<string>();

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

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _internalId.GetHashCode();
        }

        static uint _nextInternalId = 1;
        readonly uint _internalId;
    }
}
