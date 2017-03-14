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

			String endpoint = $"http://archiveofourown.org/works/{handle}?view_adult=true";
			WebPageParseResult result = GetWebPage( handle, endpoint, out HtmlDocument document );
			if ( result != WebPageParseResult.Success )
			{
				throw new ApplicationException( result.ToString() );
			}

			HtmlNode primaryRoot = document.DocumentNode.SelectSingleNode( "/html/body/div/div[@id='inner']/div" );
			if ( primaryRoot.SelectSingleNode( "p[@class='caution']" ) != null )
			{
				throw new ApplicationException( "Could not get past the adult content wall!" );
			}

			return AO3Fanfic.Parse( primaryRoot );
		}
	}
}
