// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
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
    /// <summary>
    /// A concrete class for containing information about a particular <seealso cref="AO3Fanfic"/> that
    /// in relation to a particular <seealso cref="AO3Series"/>.
    /// </summary>
    internal sealed class AO3SeriesEntry : ISeriesEntry
    {
        AO3SeriesEntry()
        {
        }

        /// <inheritdoc />
        public ISeriesRequestHandle Series { get; private set; }

        /// <inheritdoc />
        public int EntryNumber { get; private set; }

        /// <inheritdoc />
        public IFanficRequestHandle PreviousEntry { get; private set; }

        /// <inheritdoc />
        public IFanficRequestHandle NextEntry { get; private set; }

        /// <summary>
        /// Parses an HTML page into an instance of an <seealso cref="AO3ChapterInfo"/>.
        /// </summary>
        /// <param name="source">The source that the HTML page came from, which is then stored for
        /// querying fanfics and also passed along to any nested request handles for them to parse
        /// data with as well.</param>
        /// <param name="seriesSpan">A valid &lt;span&gt; entry that serves as the HTML root node for all of the
        /// AO3 series entry structure.</param>
        /// <returns>This will return an instance of <seealso cref="AO3SeriesEntry"/>. This will never
        /// return null.</returns>
        public static AO3SeriesEntry Parse( AO3Source source, HtmlNode seriesSpan )
        {
            HtmlNode positionSpan = seriesSpan.SelectSingleNode( "span[@class='position']" );
            AO3SeriesEntry parsed = new AO3SeriesEntry
            {
                EntryNumber = GetEntryNumber(positionSpan),
                Series = GetSeriesRequestHandle(positionSpan, source),
                PreviousEntry = GetLinkedSeriesEntryRequestHandle(seriesSpan, source, "previous"),
                NextEntry = GetLinkedSeriesEntryRequestHandle(seriesSpan, source, "next")
            };

            return parsed;
        }

        static int GetEntryNumber( HtmlNode positionSpan )
        {
            // The text comes in the format of "Part XXX of the [name] series"
            string entryNumberText = positionSpan.InnerText.Substring( "Part ".Length, positionSpan.InnerText.IndexOf( "of", StringComparison.InvariantCultureIgnoreCase ) - "Part ".Length );
            int entryNumber = int.Parse( entryNumberText );
            return entryNumber;
        }

        static ISeriesRequestHandle GetSeriesRequestHandle( HtmlNode positionSpan, AO3Source source )
        {
            HtmlNode seriesA = positionSpan.Element( "a" );
            string[] seriesHrefPieces = seriesA.GetAttributeValue( "href", null ).Split( '/', '\\' );
            AO3SeriesRequestHandle requestHandle = new AO3SeriesRequestHandle( source, seriesHrefPieces[seriesHrefPieces.Length - 1] );
            return requestHandle;
        }

        static IFanficRequestHandle GetLinkedSeriesEntryRequestHandle( HtmlNode seriesSpan, AO3Source source, string direction )
        {
            HtmlNode nextLink = seriesSpan.SelectSingleNode( $"a[@class='{direction}']" );
            if ( nextLink == null )
            {
                return null;
            }

            string[] hrefPieces = nextLink.GetAttributeValue( "href", null ).Split( '/', '\\' );
            AO3FanficRequestHandle requestHandle = new AO3FanficRequestHandle( source, hrefPieces.Last() );
            return requestHandle;
        }
    }
}
