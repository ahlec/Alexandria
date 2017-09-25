// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Searching;
using Alexandria.AO3.Utils;
using Alexandria.Caching;
using Alexandria.Model;
using Alexandria.Net;
using Alexandria.RequestHandles;
using Alexandria.Searching;
using HtmlAgilityPack;

namespace Alexandria.AO3
{
    public class AO3Source : LibrarySource
    {
        public AO3Source( IWebClient webClient, Cache cache )
            : base( webClient, cache )
        {
        }

        /// <inheritdoc />
        public override string SourceHandle => "ao3";

        /// <inheritdoc />
        public override IQueryResultsPage<IFanfic, IFanficRequestHandle> Search( LibrarySearch searchCriteria )
        {
            if ( searchCriteria == null )
            {
                throw new ArgumentNullException( nameof( searchCriteria ) );
            }

            string searchUrl = CreateSearchUrl( searchCriteria );

            HtmlDocument document = GetHtmlWebPage( searchUrl );

            return AO3FanficSearchResults.Parse( this, searchUrl, 1, document );
        }

        /// <inheritdoc />
        public override IAuthorRequestHandle MakeAuthorRequest( string username )
        {
            return MakeAuthorRequest( username, null );
        }

        public IAuthorRequestHandle MakeAuthorRequest( string username, string pseud )
        {
            if ( string.IsNullOrEmpty( username ) )
            {
                throw new ArgumentNullException( nameof( username ) );
            }

            if ( string.IsNullOrWhiteSpace( pseud ) )
            {
                pseud = null;
            }

            return new AO3AuthorRequestHandle( this, username, pseud );
        }

        /// <inheritdoc />
        public override ICharacterRequestHandle MakeCharacterRequest( string fullName )
        {
            if ( string.IsNullOrEmpty( fullName ) )
            {
                throw new ArgumentNullException( nameof( fullName ) );
            }

            return new AO3CharacterRequestHandle( this, fullName );
        }

        /// <inheritdoc />
        public override IFanficRequestHandle MakeFanficRequest( string handle )
        {
            if ( string.IsNullOrEmpty( handle ) )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            if ( handle.Any( character => !char.IsDigit( character ) ) )
            {
                throw new ArgumentException( "Handles to fanfics on AO3 may only consist of numbers.", nameof( handle ) );
            }

            return new AO3FanficRequestHandle( this, handle );
        }

        /// <inheritdoc />
        public override ISeriesRequestHandle MakeSeriesRequest( string handle )
        {
            if ( string.IsNullOrEmpty( handle ) )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            if ( handle.Any( character => !char.IsDigit( character ) ) )
            {
                throw new ArgumentException( "Handles for series on AO3 may only consist of numbers.", nameof( handle ) );
            }

            return new AO3SeriesRequestHandle( this, handle );
        }

        /// <inheritdoc />
        public override IShipRequestHandle MakeShipRequest( string tag )
        {
            if ( string.IsNullOrEmpty( tag ) )
            {
                throw new ArgumentNullException( nameof( tag ) );
            }

            return new AO3ShipRequestHandle( this, tag );
        }

        /// <inheritdoc />
        public override ITagRequestHandle MakeTagRequest( string tag )
        {
            if ( string.IsNullOrEmpty( tag ) )
            {
                throw new ArgumentNullException( nameof( tag ) );
            }

            return new AO3TagRequestHandle( this, tag );
        }

        static string CreateSearchUrl( LibrarySearch searchCriteria )
        {
            StringBuilder searchUrl = new StringBuilder( "http://www.archiveofourown.org/works/search?utf8=✓&commit=Search" );

            // Work Info
            if ( !string.IsNullOrWhiteSpace( searchCriteria.Title ) )
            {
                searchUrl.Append( "&work_search[title]=" );
                searchUrl.Append( searchCriteria.Title );
            }

            if ( !string.IsNullOrWhiteSpace( searchCriteria.Author ) )
            {
                searchUrl.Append( "&work_search[creator]=" );
                searchUrl.Append( searchCriteria.Author );
            }

            if ( searchCriteria.Date != null )
            {
                searchUrl.Append( "&work_search[revised_at]=" );
                searchUrl.Append( searchCriteria.Date );
            }

            if ( searchCriteria.OnlyIncludeCompleteFanfics )
            {
                searchUrl.Append( "&work_search[complete]=1" );
            }

            if ( searchCriteria.OnlyIncludeSingleChapterFanfics )
            {
                searchUrl.Append( "&work_search[single_chapter]=1" );
            }

            if ( searchCriteria.WordCount != null )
            {
                searchUrl.Append( "&work_search[word_count]=" );
                searchUrl.Append( searchCriteria.WordCount );
            }

            if ( searchCriteria.Language != null )
            {
                searchUrl.Append( "&work_search[language_id]=" );
                searchUrl.Append( AO3LanguageUtils.GetId( searchCriteria.Language.Value ) );
            }

            // Work Tags
            if ( searchCriteria.Fandoms?.Count > 0 )
            {
                searchUrl.Append( "&work_search[fandom_names]=" );
                for ( int index = 0; index < searchCriteria.Fandoms.Count; ++index )
                {
                    if ( index > 0 )
                    {
                        searchUrl.Append( "," );
                    }

                    searchUrl.Append( searchCriteria.Fandoms[index] );
                }
            }

            if ( searchCriteria.Rating != null )
            {
                searchUrl.Append( "&work_search[rating_ids]=" );
                searchUrl.Append( AO3MaturityRatingUtils.GetId( searchCriteria.Rating.Value ) );
            }

            if ( searchCriteria.ContentWarnings != ContentWarnings.None )
            {
                foreach ( int warningId in AO3ContentWarningUtils.GetIds( searchCriteria.ContentWarnings ) )
                {
                    searchUrl.Append( "&work_search[warning_ids][]=" );
                    searchUrl.Append( warningId );
                }
            }

            if ( searchCriteria.CharacterNames?.Count > 0 )
            {
                searchUrl.Append( "&work_search[character_names]=" );
                for ( int index = 0; index < searchCriteria.CharacterNames.Count; ++index )
                {
                    if ( index > 0 )
                    {
                        searchUrl.Append( "," );
                    }

                    searchUrl.Append( searchCriteria.CharacterNames[index] );
                }
            }

            if ( searchCriteria.Ships?.Count > 0 )
            {
                searchUrl.Append( "&work_search[relationship_names]=" );
                for ( int index = 0; index < searchCriteria.Ships.Count; ++index )
                {
                    if ( index > 0 )
                    {
                        searchUrl.Append( "," );
                    }

                    searchUrl.Append( searchCriteria.Ships[index] );
                }
            }

            if ( searchCriteria.Tags?.Count > 0 )
            {
                searchUrl.Append( "&work_search[freeform_names]=" );
                for ( int index = 0; index < searchCriteria.Tags.Count; ++index )
                {
                    if ( index > 0 )
                    {
                        searchUrl.Append( "," );
                    }

                    searchUrl.Append( searchCriteria.Tags[index] );
                }
            }

            // Work Stats
            if ( searchCriteria.NumberLikes != null )
            {
                searchUrl.Append( "&work_search[kudos_count]=" );
                searchUrl.Append( searchCriteria.NumberLikes );
            }

            if ( searchCriteria.NumberComments != null )
            {
                searchUrl.Append( "&work_search[comments_count]=" );
                searchUrl.Append( searchCriteria.NumberComments );
            }

            // Search
            searchUrl.Append( "&work_search[sort_column]=" );
            switch ( searchCriteria.SortField )
            {
                case SearchField.BestMatch:
                    {
                        // Apparently doesn't have a search value?
                        break;
                    }

                case SearchField.Author:
                    {
                        searchUrl.Append( "authors_to_sort_on" );
                        break;
                    }

                case SearchField.Title:
                    {
                        searchUrl.Append( "title_to_sort_on" );
                        break;
                    }

                case SearchField.DatedPosted:
                    {
                        searchUrl.Append( "created_at" );
                        break;
                    }

                case SearchField.DateLastUpdated:
                    {
                        searchUrl.Append( "revised_at" );
                        break;
                    }

                case SearchField.WordCount:
                    {
                        searchUrl.Append( "word_count" );
                        break;
                    }

                case SearchField.NumberLikes:
                    {
                        searchUrl.Append( "kudos_count" );
                        break;
                    }

                case SearchField.NumberComments:
                    {
                        searchUrl.Append( "comments_count" );
                        break;
                    }

                default:
                    throw new NotImplementedException();
            }

            searchUrl.Append( "&work_search[sort_direction]=" );
            switch ( searchCriteria.SortDirection )
            {
                case SortDirection.Ascending:
                    {
                        searchUrl.Append( "asc" );
                        break;
                    }

                case SortDirection.Descending:
                    {
                        searchUrl.Append( "desc" );
                        break;
                    }

                default:
                    throw new NotImplementedException();
            }

            return searchUrl.ToString();
        }
    }
}
