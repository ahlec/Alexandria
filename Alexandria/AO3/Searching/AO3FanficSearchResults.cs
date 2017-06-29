using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.AO3.RequestHandles;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria.AO3.Searching
{
	internal sealed class AO3FanficSearchResults : IQueryResultsPage<IFanfic, IFanficRequestHandle>
	{
		public AO3FanficSearchResults( String baseEndpoint, Int32 currentPage )
		{
			_baseEndpoint = baseEndpoint;
			_currentPage = currentPage;
		}

		#region IQueryResultsPage<IFanfic, IFanficRequestHandle

		public IReadOnlyList<IFanficRequestHandle> Results { get; private set; }

		public Boolean HasMoreResults { get; private set; }

		public IQueryResultsPage<IFanfic, IFanficRequestHandle> RetrieveNextPage()
		{
			if ( !HasMoreResults )
			{
				throw new InvalidOperationException();
			}

			String endpoint = String.Concat( _baseEndpoint, "&page=", ( _currentPage + 1 ) );
			HtmlDocument document = HtmlUtils.GetWebPage( endpoint );
			return Parse( _baseEndpoint, _currentPage + 1, document );
		}

		#endregion

		public static AO3FanficSearchResults Parse( String baseEndpoint, Int32 currentPage, HtmlDocument document )
		{
			AO3FanficSearchResults parsed = new AO3FanficSearchResults( baseEndpoint, currentPage );

			HtmlNode paginationOl = document.DocumentNode.SelectSingleNode( "//ol[@class='pagination actions']" );
			HtmlNode nextA = paginationOl?.SelectSingleNode( ".//a[@rel='next']" );
			parsed.HasMoreResults = ( nextA != null );

			HtmlNode workIndexGroupOl = document.DocumentNode.SelectSingleNode( "//ol[@class='work index group']" );
			parsed.Results = workIndexGroupOl.Elements( "li" ).Select( AO3FanficRequestHandle.ParseFromWorkLi ).Cast<IFanficRequestHandle>().ToList();

			return parsed;
		}

		readonly String _baseEndpoint;
		readonly Int32 _currentPage;
	}
}
