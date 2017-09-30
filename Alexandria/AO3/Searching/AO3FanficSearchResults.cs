// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.AO3.RequestHandles;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria.AO3.Searching
{
    internal sealed class AO3FanficSearchResults : IQueryResultsPage<IFanfic, IFanficRequestHandle>
    {
        public AO3FanficSearchResults( AO3Source source, string baseEndpoint, int currentPage )
        {
            _source = source;
            _baseEndpoint = baseEndpoint;
            _currentPage = currentPage;
        }

        public IReadOnlyList<IFanficRequestHandle> Results { get; private set; }

        public bool HasMoreResults { get; private set; }

        public static AO3FanficSearchResults Parse( AO3Source source, string baseEndpoint, int currentPage, HtmlDocument document )
        {
            AO3FanficSearchResults parsed = new AO3FanficSearchResults( source, baseEndpoint, currentPage );

            HtmlNode paginationOl = document.DocumentNode.SelectSingleNode( "//ol[@class='pagination actions']" );
            HtmlNode nextA = paginationOl?.SelectSingleNode( ".//a[@rel='next']" );
            parsed.HasMoreResults = ( nextA != null );

            HtmlNode workIndexGroupOl = document.DocumentNode.SelectSingleNode( "//ol[@class='work index group']" );
            parsed.Results = workIndexGroupOl.Elements( "li" ).Select( li => AO3FanficRequestHandle.ParseFromWorkLi( source, li ) ).Cast<IFanficRequestHandle>().ToList();

            return parsed;
        }

        public IQueryResultsPage<IFanfic, IFanficRequestHandle> RetrieveNextPage()
        {
            if ( !HasMoreResults )
            {
                throw new InvalidOperationException();
            }

            string endpoint = string.Concat( _baseEndpoint, "&page=", ( _currentPage + 1 ) );
            HtmlDocument document = _source.GetHtmlWebPage( endpoint );
            return Parse( _source, _baseEndpoint, _currentPage + 1, document );
        }

        readonly AO3Source _source;
        readonly string _baseEndpoint;
        readonly int _currentPage;
    }
}
