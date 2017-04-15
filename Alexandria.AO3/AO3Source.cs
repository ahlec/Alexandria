using System;
using System.Text;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.Model;
using Alexandria.AO3.RequestHandles;
using Alexandria.Searching;

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
			return AO3Author.Parse( this, document );
		}

		IFanfic GetFanficInternal( String handle, String endpoint, Boolean isRetryingOnResponseUrl )
		{
			HtmlDocument document = GetWebPage( CacheableObjects.FanficHtml, handle, endpoint, !isRetryingOnResponseUrl, out Uri responseUrl );

			if ( document.DocumentNode.SelectSingleNode( "//div[@id='workskin']" ) != null )
			{
				return AO3Fanfic.Parse( document );
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
			return AO3Tag.Parse( document, this );
		}

		ISeries GetSeries( String handle )
		{
			String endpoint = $"http://archiveofourown.org/series/{handle}";
			HtmlDocument document = GetWebPage( CacheableObjects.SeriesHtml, handle, endpoint, false, out Uri responseUrl );
			return AO3Series.Parse( document );
		}

		/// <inheritdoc />
		public override IQueryResultsPage<IFanfic, IFanficRequestHandle> Search( LibrarySearch searchCriteria )
		{
			if ( searchCriteria == null )
			{
				throw new ArgumentNullException( nameof( searchCriteria ) );
			}

			StringBuilder searchUrl = new StringBuilder( "http://www.archiveofourown.org/works/search?utf8=✓&commit=Search" );
			if ( searchCriteria.OnlyIncludeCompleteFanfics )
			{
				searchUrl.Append( "&work_search[complete]=1" );
			}

			throw new NotImplementedException();
		}
	}
}
