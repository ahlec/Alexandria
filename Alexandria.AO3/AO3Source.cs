using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.AO3.Model;

namespace Alexandria.AO3
{
	public class AO3Source : LibrarySource
	{
		public override IAuthor GetAuthor( String name )
		{
			throw new NotImplementedException();
		}

		public override IFanfic GetFanfic( String handle )
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
