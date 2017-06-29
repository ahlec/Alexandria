using System;
using System.Linq;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3SeriesEntry : ISeriesEntry
	{
		AO3SeriesEntry()
		{
		}

		#region ISeriesEntry

		public ISeriesRequestHandle Series { get; private set; }

		public Int32 EntryNumber { get; private set; }

		public IFanficRequestHandle PreviousEntry { get; private set; }

		public IFanficRequestHandle NextEntry { get; private set; }

		#endregion

		public static AO3SeriesEntry Parse( HtmlNode seriesSpan )
		{
			AO3SeriesEntry parsed = new AO3SeriesEntry();

			HtmlNode positionSpan = seriesSpan.SelectSingleNode( "span[@class='position']" );
			// The text comes in the format of "Part XXX of the [name] series"
			String entryNumberText = positionSpan.InnerText.Substring( "Part ".Length, positionSpan.InnerText.IndexOf( "of", StringComparison.InvariantCultureIgnoreCase ) - "Part ".Length );
			parsed.EntryNumber = Int32.Parse( entryNumberText );
			HtmlNode seriesA = positionSpan.Element( "a" );
			String[] seriesHrefPieces = seriesA.GetAttributeValue( "href", null ).Split( '/', '\\' );
			parsed.Series = new AO3SeriesRequestHandle( seriesHrefPieces[seriesHrefPieces.Length - 1] );

			HtmlNode previousLink = seriesSpan.SelectSingleNode( "a[@class='previous']" );
			if ( previousLink != null )
			{
				String[] hrefPieces = previousLink.GetAttributeValue( "href", null ).Split( '/', '\\' );
				parsed.PreviousEntry = new AO3FanficRequestHandle( hrefPieces.Last() );
			}

			HtmlNode nextLink = seriesSpan.SelectSingleNode( "a[@class='next']" );
			if ( nextLink != null )
			{
				String[] hrefPieces = nextLink.GetAttributeValue( "href", null ).Split( '/', '\\' );
				parsed.NextEntry = new AO3FanficRequestHandle( hrefPieces.Last() );
			}

			return parsed;
		}
	}
}
