using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.Model;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3
{
	public class AO3Source : LibrarySource
	{
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
				return (T)(Object) GetTag( tagRequest.Text );
			}
#pragma warning restore IDE0019 // Use pattern matching

			throw new NotSupportedException( $"Unable to support {nameof( IRequestHandle<T> )} with an input `{nameof( request )}` of type {request.GetType().Name}" );
		}

		IAuthor GetAuthor( String username, String pseud )
		{
			String endpoint = $"http://archiveofourown.org/users/{username}/profile";
			WebPageParseResult result = GetWebPage( CacheableObjects.AuthorHtml, username, endpoint, false, out Uri responseUrl, out HtmlDocument document );
			if ( result != WebPageParseResult.Success )
			{
				throw new ApplicationException( result.ToString() );
			}

			return AO3Author.Parse( this, document );
		}

		IFanfic GetFanficInternal( String handle, String endpoint, Boolean isRetryingOnResponseUrl )
		{
			WebPageParseResult result = GetWebPage( CacheableObjects.FanficHtml, handle, endpoint, !isRetryingOnResponseUrl, out Uri responseUrl, out HtmlDocument document );
			if ( result != WebPageParseResult.Success )
			{
				throw new ApplicationException( result.ToString() );
			}

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
			WebPageParseResult result = GetWebPage( CacheableObjects.TagHtml, tag, endpoint, false, out Uri responseUrl, out HtmlDocument document );
			if ( result != WebPageParseResult.Success )
			{
				throw new ApplicationException( result.ToString() );
			}

			return AO3Tag.Parse( document );
		}
	}
}
