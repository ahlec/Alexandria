using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3QueryResults : IQueryResultsPage<IFanfic, IFanficRequestHandle>
	{
		AO3QueryResults( AO3Source source, CacheableObjects objectType, String endpointCategory, String endpointTag, Int32 page )
		{
			_source = source;
			_objectType = objectType;
			_endpointCategory = endpointCategory;
			_endpointQuery = endpointTag;
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

			return Retrieve( _source, _objectType, _endpointCategory, _endpointQuery, _page + 1 );
		}

		#endregion

		internal static AO3QueryResults Retrieve( AO3Source source, CacheableObjects objectType, String endpointCategory, String endpointTag, Int32 page )
		{
			AO3QueryResults results = new AO3QueryResults( source, objectType, endpointCategory, endpointTag, page );
			String endpoint = $"http://archiveofourown.org/{endpointCategory}/{endpointTag}/works";
			String cacheHandle = endpointTag;
			if ( page > 1 )
			{
				endpoint += $"?page={page}";
				cacheHandle += page.ToString();
			}

			HtmlDocument document = source.RetrieveEndpoint( objectType, cacheHandle, endpoint );
			HtmlNode worksIndexDiv = document.DocumentNode.SelectSingleNode( "//div[contains(@class, 'works-index')]" );

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
		readonly CacheableObjects _objectType;
		readonly String _endpointCategory;
		readonly String _endpointQuery;
		readonly Int32 _page;
	}
}
