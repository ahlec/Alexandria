using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Utils;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3Series : ISeries
	{
		AO3Series( Uri url )
		{
			Url = url;
		}

		#region IRequestable

		public Uri Url { get; }

		#endregion

		#region ISeries

		public IAuthorRequestHandle Author { get; private set; }

		public DateTime DateStarted { get; private set; }

		public DateTime DateLastUpdated { get; private set; }

		public Boolean IsCompleted { get; private set; }

		public IReadOnlyList<IFanficRequestHandle> Fanfics { get; private set; }

		#endregion

		public static AO3Series Parse( Uri url, HtmlDocument document )
		{
			AO3Series parsed = new AO3Series( url );

			HtmlNode mainDiv = document.DocumentNode.SelectSingleNode( "//div[@id='main']" );

			HtmlNode seriesMetaGroupDl = mainDiv.SelectSingleNode( ".//dl[@class='series meta group']" );
			Boolean hasDateLastUpdated = false;
			foreach ( Tuple<String, HtmlNode> row in seriesMetaGroupDl.EnumerateDlTable() )
			{
				switch ( row.Item1 )
				{
					case "Creator":
						{
							parsed.Author = AO3AuthorRequestHandle.Parse( row.Item2.Element( "a" ) );
							break;
						}
					case "Series Begun":
						{
							parsed.DateStarted = DateTime.Parse( row.Item2.InnerText );
							break;
						}
					case "Series Updated":
						{
							parsed.DateLastUpdated = DateTime.Parse( row.Item2.InnerText );
							hasDateLastUpdated = true;
							break;
						}
					case "Stats":
						{
							Tuple<String, HtmlNode> completeDd = row.Item2.EnumerateDlTable().FirstOrDefault( kvp => kvp.Item1.Equals( "Complete" ) );
							parsed.IsCompleted = ( completeDd?.Item2.InnerText.Equals( "Yes", StringComparison.InvariantCultureIgnoreCase ) == true );
							break;
						}
				}
			}
			if ( !hasDateLastUpdated )
			{
				parsed.DateLastUpdated = parsed.DateStarted;
			}

			HtmlNode seriesWorkUl = mainDiv.SelectSingleNode( ".//ul[contains(@class, 'series work' )]" );
			parsed.Fanfics = seriesWorkUl.Elements( "li" ).Select( AO3FanficRequestHandle.ParseFromWorkLi ).Cast<IFanficRequestHandle>().ToList();

			return parsed;
		}
	}
}
