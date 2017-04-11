using System;
using System.Collections.Generic;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Utils;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3Series : ISeries
	{
		AO3Series()
		{
		}

		#region ISeries

		public IAuthorRequestHandle Author { get; private set; }

		public DateTime DateStarted { get; private set; }

		public Boolean IsCompleted { get; private set; }

		public IReadOnlyList<IFanficRequestHandle> Fanfics { get; private set; }

		#endregion

		public static AO3Series Parse( HtmlDocument document )
		{
			AO3Series parsed = new AO3Series();

			HtmlNode mainDiv = document.DocumentNode.SelectSingleNode( "//div[@id='main']" );

			HtmlNode seriesMetaGroupDl = mainDiv.SelectSingleNode( ".//dl[@class='series meta group']" );
			String lastDtText = null;
			foreach ( HtmlNode child in seriesMetaGroupDl.ChildNodes )
			{
				if ( child.Name.Equals( "dt" ) )
				{
					lastDtText = child.InnerText.Trim();
					continue;
				}

				if ( !child.Name.Equals( "dd" ) )
				{
					continue;
				}

				switch ( lastDtText )
				{
					case "Creator:":
						{
							parsed.Author = AO3AuthorRequestHandle.Parse( child.Element( "a" ) );
							break;
						}
					case "Series Begun:":
						{
							parsed.DateStarted = DateTime.Parse( child.InnerText );
							break;
						}
				}
			}

			return parsed;
		}
	}
}
