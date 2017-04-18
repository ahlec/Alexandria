using System;
using System.Text;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.Model;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Searching;
using Alexandria.AO3.Utils;
using Alexandria.Searching;
using Alexandria.Utils;

namespace Alexandria.AO3
{
	public class AO3Source : LibrarySource
	{
		public AO3Source( LibrarySourceConfig config ) : base( config )
		{
		}

		public override T MakeRequest<T>( IRequestHandle<T> request )
		{
			if ( request == null )
			{
				throw new ArgumentNullException( nameof( request ) );
			}

#pragma warning disable IDE0019 // Use pattern matching
			AO3FanficRequestHandle fanficRequest = request as AO3FanficRequestHandle;
			if ( fanficRequest != null )
			{
				String endpoint = $"http://www.archiveofourown.org/works/{fanficRequest.Handle}?view_adult=true";
				return (T) GetFanficInternal( fanficRequest.Handle, endpoint, true );
			}

			AO3AuthorRequestHandle authorRequest = request as AO3AuthorRequestHandle;
			if ( authorRequest != null )
			{
				return (T) GetAuthor( authorRequest.Username, authorRequest.Pseud );
			}

			AO3TagRequestHandle tagRequest = request as AO3TagRequestHandle;
			if ( tagRequest != null )
			{
				return (T) (Object) GetTag( tagRequest.Text );
			}

			AO3SeriesRequestHandle seriesRequest = request as AO3SeriesRequestHandle;
			if ( seriesRequest != null )
			{
				return (T) GetSeries( seriesRequest.Handle );
			}
#pragma warning restore IDE0019 // Use pattern matching

			throw new NotSupportedException( $"Unable to support {nameof( IRequestHandle<T> )} with an input `{nameof( request )}` of type {request.GetType().Name}" );
		}

		internal HtmlDocument RetrieveEndpoint( CacheableObjects objectType, String cacheHandle, String endpoint )
		{
			return GetWebPage( objectType, cacheHandle, endpoint, false, out Uri responseUrl );
		}

		IAuthor GetAuthor( String username, String pseud )
		{
			String endpoint = $"http://archiveofourown.org/users/{username}/profile";
			HtmlDocument document = GetWebPage( CacheableObjects.AuthorHtml, username, endpoint, false, out Uri responseUrl );
			return AO3Author.Parse( this, responseUrl, document );
		}

		IFanfic GetFanficInternal( String handle, String endpoint, Boolean isRetryingOnResponseUrl )
		{
			HtmlDocument document = GetWebPage( CacheableObjects.FanficHtml, handle, endpoint, !isRetryingOnResponseUrl, out Uri responseUrl );

			if ( document.DocumentNode.SelectSingleNode( "//div[@id='workskin']" ) != null )
			{
				return AO3Fanfic.Parse( responseUrl, document );
			}

			if ( isRetryingOnResponseUrl )
			{
				return GetFanficInternal( handle, responseUrl + "?view_adult=true", false );
			}

			throw new ApplicationException( "Could not get past the adult content wall!" );
		}

		AO3Tag GetTag( String tag )
		{
			tag = tag.Replace( "/", "*s*" );
			String endpoint = $"http://archiveofourown.org/tags/{tag}";
			HtmlDocument document = GetWebPage( CacheableObjects.TagHtml, tag, endpoint, false, out Uri responseUrl );
			return AO3Tag.Parse( this, responseUrl, document );
		}

		ISeries GetSeries( String handle )
		{
			String endpoint = $"http://archiveofourown.org/series/{handle}";
			HtmlDocument document = GetWebPage( CacheableObjects.SeriesHtml, handle, endpoint, false, out Uri responseUrl );
			return AO3Series.Parse( responseUrl, document );
		}

		/// <inheritdoc />
		public override IQueryResultsPage<IFanfic, IFanficRequestHandle> Search( LibrarySearch searchCriteria )
		{
			if ( searchCriteria == null )
			{
				throw new ArgumentNullException( nameof( searchCriteria ) );
			}

			String searchUrl = CreateSearchUrl( searchCriteria );

			HtmlDocument document = HtmlUtils.GetWebPage( searchUrl );

			return AO3FanficSearchResults.Parse( searchUrl, 1, document );
		}

		String CreateSearchUrl( LibrarySearch searchCriteria )
		{
			StringBuilder searchUrl = new StringBuilder( "http://www.archiveofourown.org/works/search?utf8=✓&commit=Search" );

			// Work Info
			if ( !String.IsNullOrWhiteSpace( searchCriteria.Title ) )
			{
				searchUrl.Append( "&work_search[title]=" );
				searchUrl.Append( searchCriteria.Title );
			}
			if ( !String.IsNullOrWhiteSpace( searchCriteria.Author ) )
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
				for ( Int32 index = 0; index < searchCriteria.Fandoms.Count; ++index )
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
			if ( searchCriteria.ContentWarnings != null )
			{
				foreach ( Int32 warningId in AO3ContentWarningUtils.GetIds( searchCriteria.ContentWarnings.Value ) )
				{
					searchUrl.Append( "&work_search[warning_ids][]=" );
					searchUrl.Append( warningId );
				}
			}
			if ( searchCriteria.CharacterNames?.Count > 0 )
			{
				searchUrl.Append( "&work_search[character_names]=" );
				for ( Int32 index = 0; index < searchCriteria.CharacterNames.Count; ++index )
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
				for ( Int32 index = 0; index < searchCriteria.Ships.Count; ++index )
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
				for ( Int32 index = 0; index < searchCriteria.Tags.Count; ++index )
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
