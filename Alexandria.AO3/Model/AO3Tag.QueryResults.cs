using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3.Model
{
	internal sealed partial class AO3Tag
	{
		class QueryResults : IQueryResultsPage<IFanfic, IFanficRequestHandle>
		{
			QueryResults( AO3Source source, String endpointTag, Int32 page )
			{
				_source = source;
				_endpointTag = endpointTag;
				_page = page;
			}

			#region IQueryResultsPage<IFanfic, IFanficRequestHandle>

			public IReadOnlyList<IFanficRequestHandle> Results { get; private set; }

			public Boolean HasMoreResults { get; private set; }

			public IQueryResultsPage<IFanfic, IFanficRequestHandle> RetrieveNextPage()
			{
				if ( !HasMoreResults )
				{
					throw new InvalidOperationException();
				}

				return Retrieve( _source, _endpointTag, _page + 1 );
			}

			#endregion

			internal static QueryResults Retrieve( AO3Source source, String endpointTag, Int32 page )
			{
				QueryResults results = new QueryResults( source, endpointTag, page );
				String endpoint = $"http://archiveofourown.org/tags/{endpointTag}/works";
				String cacheHandle = endpointTag;
				if ( page > 1 )
				{
					endpoint += $"?page={page}";
					cacheHandle += page.ToString();
				}

				HtmlDocument document = source.RetrieveEndpoint( CacheableObjects.TagFanficsHtml, cacheHandle, endpoint );
				HtmlNode worksIndexDiv = document.DocumentNode.SelectSingleNode( "//div[@class='works-index filtered region']" );

				HtmlNode paginationOl = worksIndexDiv.SelectSingleNode( ".//ol[@class='pagination actions']" );
				HtmlNode nextA = paginationOl?.SelectSingleNode( ".//a[@rel='next']" );
				results.HasMoreResults = ( nextA != null );

				List<IFanficRequestHandle> handles = new List<IFanficRequestHandle>();
				HtmlNode worksOl = worksIndexDiv.SelectSingleNode( ".//ol[@class='work index group']" );
				foreach ( HtmlNode li in worksOl.Elements( "li" ) )
				{
					String fanficHandle = li.GetAttributeValue( "id", null ).Substring( "work_".Length );
					handles.Add( new AO3FanficRequestHandle( fanficHandle ) );
				}
				results.Results = handles;

				return results;
			}

			readonly AO3Source _source;
			readonly String _endpointTag;
			readonly Int32 _page;
		}
	}
}
