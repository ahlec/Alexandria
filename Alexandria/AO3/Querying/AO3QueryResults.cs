﻿// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.AO3.RequestHandles;
using Alexandria.Model;
using Alexandria.Querying;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Querying
{
    internal sealed class AO3QueryResults : IQueryResultsPage<IFanfic, IFanficRequestHandle>
    {
        readonly AO3Source _source;
        readonly string _endpointCategory;
        readonly string _endpointQuery;
        readonly int _page;

        AO3QueryResults( AO3Source source, string endpointCategory, string endpointTag, int page )
        {
            _source = source;
            _endpointCategory = endpointCategory;
            _endpointQuery = endpointTag;
            _page = page;
        }

        public IReadOnlyList<IFanficRequestHandle> Results { get; private set; }

        public bool HasMoreResults { get; private set; }

        public IQueryResultsPage<IFanfic, IFanficRequestHandle> RetrieveNextPage()
        {
            if ( !HasMoreResults )
            {
                throw new InvalidOperationException();
            }

            return Retrieve( _source, _endpointCategory, _endpointQuery, _page + 1 );
        }

        internal static AO3QueryResults Retrieve( AO3Source source, string endpointCategory, string endpointTag, int page )
        {
            AO3QueryResults results = new AO3QueryResults( source, endpointCategory, endpointTag, page );
            string endpoint = $"http://archiveofourown.org/{endpointCategory}/{endpointTag}/works";
            if ( page > 1 )
            {
                endpoint += $"?page={page}";
            }

            HtmlNode documentNode = source.GetHtmlWebPage( endpoint );
            HtmlNode worksIndexDiv = documentNode.SelectSingleNode( "//div[contains(@class, 'works-index')]" );

            HtmlNode paginationOl = worksIndexDiv.SelectSingleNode( ".//ol[@class='pagination actions']" );
            HtmlNode nextA = paginationOl?.SelectSingleNode( ".//a[@rel='next']" );
            results.HasMoreResults = ( nextA != null );

            HtmlNode worksOl = worksIndexDiv.SelectSingleNode( ".//ol[@class='work index group']" );
            results.Results = AO3FanficRequestHandle.ParseFanficLiList( source, worksOl );

            return results;
        }
    }
}
