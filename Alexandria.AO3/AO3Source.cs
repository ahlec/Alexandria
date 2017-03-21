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
				return (T) GetFanfic( fanficRequest.Handle );
			}

			AO3AuthorRequestHandle authorRequest = request as AO3AuthorRequestHandle;
			if ( authorRequest != null )
			{
				return (T) GetAuthor( authorRequest.Username, authorRequest.Pseud );
			}
#pragma warning restore IDE0019 // Use pattern matching

			throw new NotSupportedException( $"Unable to support {nameof( IRequestHandle<T> )} with an input `{nameof( request )}` of type {request.GetType().Name}" );
		}

		IAuthor GetAuthor( String username, String pseud )
		{
			throw new NotImplementedException();
		}

		IFanfic GetFanfic( String handle )
		{
			if ( String.IsNullOrEmpty( handle ) )
			{
				throw new ArgumentNullException( nameof( handle ) );
			}

			String endpoint = $"http://www.archiveofourown.org/works/{handle}?view_adult=true";
			return GetFanficInternal( handle, endpoint, true );
		}

		IFanfic GetFanficInternal( String handle, String endpoint, Boolean isRetryingOnResponseUrl )
		{
			WebPageParseResult result = GetWebPage( handle, endpoint, !isRetryingOnResponseUrl, out Uri responseUrl, out HtmlDocument document );
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
	}
}
