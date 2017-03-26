using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3Author : IAuthor
	{
		AO3Author( AO3Source source )
		{
			_source = source;
		}

		#region IAuthor

		public String Name { get; private set; }

		public IReadOnlyList<String> Nicknames { get; private set; }

		public DateTime DateJoined { get; private set; }

		public String Location { get; private set; }

		public DateTime? Birthday { get; private set; }

		public String Biography { get; private set; }

		public Int32 NumberFanfics { get; private set; }

		public IQueryResultsPage<IFanfic, IFanficRequestHandle> QueryFanfics()
		{
			throw new NotImplementedException();
		}

		#endregion IAuthor

		static IEnumerable<String> CollectPseuds( HtmlNode pseudsDD )
		{
			foreach ( HtmlNode pseudA in pseudsDD.Elements( "a" ) )
			{
				yield return pseudA.ReadableInnerText().Trim();
			}
		}

		public static AO3Author Parse( AO3Source source, HtmlDocument profileDocument )
		{
			AO3Author parsed = new AO3Author( source );

			HtmlNode userHomeProfile = profileDocument.DocumentNode.SelectSingleNode( "//div[@class='user home profile']" );
			parsed.Name = userHomeProfile.SelectSingleNode( "div[@class='primary header module']/h2[@class='heading']/a" ).ReadableInnerText().Trim();

			HtmlNode metaDl = userHomeProfile.SelectSingleNode( ".//dl[@class='meta']" );
			String lastDtText = null;
			foreach ( HtmlNode child in metaDl.ChildNodes )
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
					case "My pseuds:":
						{
							parsed.Nicknames = CollectPseuds( child ).ToList();
							break;
						}
					case "I joined on:":
						{
							parsed.DateJoined = DateTime.Parse( child.InnerText );
							break;
						}
					case "I live in:":
						{
							parsed.Location = child.ReadableInnerText().Trim();
							break;
						}
					case "My birthday:":
						{
							parsed.Birthday = DateTime.Parse( child.InnerText );
							break;
						}
				}
			}

			parsed.Biography = userHomeProfile.SelectSingleNode( "div[@class='bio module']/blockquote" )?.ReadableInnerText().Trim();

			HtmlNode dashboardDiv = profileDocument.DocumentNode.SelectSingleNode( "//div[@id='dashboard']" );
			foreach ( HtmlNode dashboardA in dashboardDiv.SelectNodes( ".//a" ) )
			{
				if ( !dashboardA.InnerText.StartsWith( "Work" ) )
				{
					continue;
				}

				Int32 startIndex = dashboardA.InnerText.IndexOf( '(' );
				Int32 endIndex = dashboardA.InnerText.IndexOf( ')' );

				parsed.NumberFanfics = Int32.Parse( dashboardA.InnerText.Substring( startIndex + 1, endIndex - startIndex - 1 ) );
				break;
			}

			return parsed;
		}

		readonly AO3Source _source;
	}
}
