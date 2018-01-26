// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.AO3.Data;
using Alexandria.AO3.Querying;
using Alexandria.Languages;
using Alexandria.Model;
using Alexandria.Querying;
using Alexandria.RequestHandles;
using Alexandria.Searching;
using HtmlAgilityPack;

namespace Alexandria.AO3.Searching
{
    public sealed class AO3Search : LibrarySearch
    {
        readonly AO3Source _source;

        internal AO3Search( AO3Source source )
        {
            _source = source ?? throw new ArgumentNullException( nameof( source ) );
        }

        delegate string ToSearchStringValueFunc<in T>( T value );

        delegate IEnumerable<string> ToManySearchStringValueFunc<in T>( T value );

        /// <inheritdoc />
        public override Website Website => Website.AO3;

        /// <inheritdoc />
        public override IQueryResultsPage<IFanfic, IFanficRequestHandle> Search()
        {
            string searchUrl = CreateSearchUrl();
            HtmlNode documentNode = _source.GetHtmlWebPage( searchUrl );
            return AO3FanficSearchResults.Parse( _source, searchUrl, 1, documentNode );
        }

        static void OpenSearchKey( StringBuilder builder, string key, bool isPartOfCheckboxArray )
        {
            builder.Append( "&work_search[" );
            builder.Append( key );
            builder.Append( "]" );

            if ( isPartOfCheckboxArray )
            {
                builder.Append( "[]" );
            }

            builder.Append( "=" );
        }

        static void AssignSearchKeyValue( StringBuilder builder, string key, bool isPartOfCheckboxArray, string valueStr )
        {
            OpenSearchKey( builder, key, isPartOfCheckboxArray );
            builder.Append( valueStr );
        }

        static void AddOptionalSearchField( StringBuilder builder, string key, string value )
        {
            if ( !string.IsNullOrWhiteSpace( key ) )
            {
                AssignSearchKeyValue( builder, key, false, value );
            }
        }

        static void AddOptionalSearchField( StringBuilder builder, string key, DateSearchCriteria date )
        {
            if ( date != null )
            {
                AssignSearchKeyValue( builder, key, false, date.ToString() );
            }
        }

        static void AddOptionalSearchField( StringBuilder builder, string key, NumberSearchCriteria number )
        {
            if ( number != null )
            {
                AssignSearchKeyValue( builder, key, false, number.ToString() );
            }
        }

        static void AddOptionalSearchField( StringBuilder builder, string key, bool value )
        {
            if ( value )
            {
                AssignSearchKeyValue( builder, key, false, "1" );
            }
        }

        static void AddOptionalSearchField( StringBuilder builder, string key, IReadOnlyList<string> value )
        {
            if ( value == null || value.Count == 0 )
            {
                return;
            }

            OpenSearchKey( builder, key, false );
            for ( int index = 0; index < value.Count; ++index )
            {
                if ( index > 0 )
                {
                    builder.Append( "," );
                }

                builder.Append( value[index] );
            }
        }

        static void AddOptionalSearchField<T>( StringBuilder builder, string key, T? value, ToSearchStringValueFunc<T> toStringFunc )
            where T : struct
        {
            if ( value != null )
            {
                AssignSearchKeyValue( builder, key, false, toStringFunc( value.Value ) );
            }
        }

        static void AddOptionalSearchField<T>( StringBuilder builder, string key, T value, ToSearchStringValueFunc<T> toStringFunc )
            where T : class
        {
            if ( value != null )
            {
                AssignSearchKeyValue( builder, key, false, toStringFunc( value ) );
            }
        }

        static void AddOptionalCheckboxSearchField<T>( StringBuilder builder, string key, T? value, ToManySearchStringValueFunc<T> toStringFunc )
            where T : struct
        {
            if ( value == null )
            {
                return;
            }

            foreach ( string strVal in toStringFunc( value.Value ) )
            {
                AssignSearchKeyValue( builder, key, true, strVal );
            }
        }

        static string GetLanguageId( Language language )
        {
            return language.AO3Id.ToString();
        }

        static string GetMaturityRatingId( MaturityRating rating )
        {
            return AO3Enums.MaturityRatings.First( def => def.EnumValue == rating ).Id;
        }

        static IEnumerable<string> GetContentWarningsId( ContentWarnings warning )
        {
            return AO3Enums.ContentWarnings.Where( def => warning.HasFlag( def.EnumValue ) ).Select( def => def.Id );
        }

        static string GetSearchFieldUrlValue( SortField field )
        {
            switch ( field )
            {
                case SortField.BestMatch:
                    return null;
                case SortField.Author:
                    return "authors_to_sort_on";
                case SortField.Title:
                    return "title_to_sort_on";
                case SortField.DatedPosted:
                    return "created_at";
                case SortField.DateLastUpdated:
                    return "revisted_at";
                case SortField.WordCount:
                    return "word_count";
                case SortField.NumberLikes:
                    return "kudos_count";
                case SortField.NumberComments:
                    return "comments_count";
                default:
                    throw new NotImplementedException();
            }
        }

        static string GetSortDirectionUrlValue( SortDirection direction )
        {
            switch ( direction )
            {
                case SortDirection.Ascending:
                    return "asc";
                case SortDirection.Descending:
                    return "desc";
                default:
                    throw new NotImplementedException();
            }
        }

        string CreateSearchUrl()
        {
            StringBuilder searchUrl = new StringBuilder( "http://www.archiveofourown.org/works/search?utf8=✓&commit=Search" );
            AddOptionalSearchField( searchUrl, "title", Title );
            AddOptionalSearchField( searchUrl, "creator", Author );
            AddOptionalSearchField( searchUrl, "revised_at", Date );
            AddOptionalSearchField( searchUrl, "complete", OnlyIncludeCompleteFanfics );
            AddOptionalSearchField( searchUrl, "single_chapter", OnlyIncludeSingleChapterFanfics );
            AddOptionalSearchField( searchUrl, "word_count", WordCount );
            AddOptionalSearchField( searchUrl, "language_id", Language, GetLanguageId );
            AddOptionalSearchField( searchUrl, "fandom_names", Fandoms );
            AddOptionalSearchField( searchUrl, "rating_ids", Rating, GetMaturityRatingId );
            AddOptionalCheckboxSearchField( searchUrl, "warning_ids", ContentWarnings, GetContentWarningsId );
            AddOptionalSearchField( searchUrl, "character_names", CharacterNames );
            AddOptionalSearchField( searchUrl, "relationship_names", Ships );
            AddOptionalSearchField( searchUrl, "freeform_names", Tags );
            AddOptionalSearchField( searchUrl, "kudos_count", NumberLikes );
            AddOptionalSearchField( searchUrl, "comments_count", NumberComments );
            AssignSearchKeyValue( searchUrl, "sort_column", false, GetSearchFieldUrlValue( SortField ) );
            AssignSearchKeyValue( searchUrl, "sort_direction", false, GetSortDirectionUrlValue( SortDirection ) );
            return searchUrl.ToString();
        }
    }
}
