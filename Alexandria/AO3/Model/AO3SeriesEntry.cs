// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Alexandria.AO3.RequestHandles;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    internal sealed class AO3SeriesEntry : ISeriesEntry
    {
        AO3SeriesEntry()
        {
        }

        public ISeriesRequestHandle Series { get; private set; }

        public int EntryNumber { get; private set; }

        public IFanficRequestHandle PreviousEntry { get; private set; }

        public IFanficRequestHandle NextEntry { get; private set; }

        public static AO3SeriesEntry Parse( AO3Source source, HtmlNode seriesSpan )
        {
            AO3SeriesEntry parsed = new AO3SeriesEntry();

            HtmlNode positionSpan = seriesSpan.SelectSingleNode( "span[@class='position']" );

            // The text comes in the format of "Part XXX of the [name] series"
            string entryNumberText = positionSpan.InnerText.Substring( "Part ".Length, positionSpan.InnerText.IndexOf( "of", StringComparison.InvariantCultureIgnoreCase ) - "Part ".Length );
            parsed.EntryNumber = int.Parse( entryNumberText );
            HtmlNode seriesA = positionSpan.Element( "a" );
            string[] seriesHrefPieces = seriesA.GetAttributeValue( "href", null ).Split( '/', '\\' );
            parsed.Series = new AO3SeriesRequestHandle( source, seriesHrefPieces[seriesHrefPieces.Length - 1] );

            HtmlNode previousLink = seriesSpan.SelectSingleNode( "a[@class='previous']" );
            if ( previousLink != null )
            {
                string[] hrefPieces = previousLink.GetAttributeValue( "href", null ).Split( '/', '\\' );
                parsed.PreviousEntry = new AO3FanficRequestHandle( source, hrefPieces.Last() );
            }

            HtmlNode nextLink = seriesSpan.SelectSingleNode( "a[@class='next']" );
            if ( nextLink != null )
            {
                string[] hrefPieces = nextLink.GetAttributeValue( "href", null ).Split( '/', '\\' );
                parsed.NextEntry = new AO3FanficRequestHandle( source, hrefPieces.Last() );
            }

            return parsed;
        }
    }
}
