using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.Model;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3SeriesEntry : ISeriesEntry
	{
		AO3SeriesEntry()
		{
		}

		#region ISeriesEntry

		public ISeries Series { get; private set; }

		public Int32 EntryNumber { get; private set; }

		public String PreviousEntryHandle { get; private set; }

		public String NextEntryHandle { get; private set; }

		#endregion

		public static AO3SeriesEntry Parse( HtmlNode seriesSpan )
		{
			AO3SeriesEntry parsed = new AO3SeriesEntry();

			HtmlNode positionSpan = seriesSpan.SelectSingleNode( "span[@class='position']" );
			// The text comes in the format of "Part XXX of the [name] series"
			String entryNumberText = positionSpan.InnerText.Substring( "Part ".Length, positionSpan.InnerText.IndexOf( "of" ) - "Part ".Length );
			parsed.EntryNumber = Int32.Parse( entryNumberText );

			HtmlNode previousLink = seriesSpan.SelectSingleNode( "a[@class='previous']" );
			if ( previousLink != null )
			{
				String[] hrefPieces = previousLink.GetAttributeValue( "href", null ).Split( '/', '\\' );
				parsed.PreviousEntryHandle = hrefPieces.Last();
			}

			HtmlNode nextLink = seriesSpan.SelectSingleNode( "a[@class='next']" );
			if ( nextLink != null )
			{
				String[] hrefPieces = nextLink.GetAttributeValue( "href", null ).Split( '/', '\\' );
				parsed.NextEntryHandle = hrefPieces.Last();
			}

			return parsed;
		}
	}
}
