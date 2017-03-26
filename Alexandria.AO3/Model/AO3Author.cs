using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3Author : IAuthor
	{
		AO3Author()
		{
		}

		#region IAuthor

		public String Name { get; private set; }

		// public IReadOnlyList<String> Nicknames { get; private set; }

		public DateTime DateJoined { get; private set; }

		public String Location { get; private set; }

		public DateTime? Birthday { get; private set; }

		public String Biography { get; private set; }

		// public IReadOnlyList<IFanficRequestHandle> Works { get; private set; }

		#endregion IAuthor

		public static AO3Author Parse( HtmlDocument profileDocument )
		{
			AO3Author parsed = new AO3Author();

			HtmlNode userHomeProfile = profileDocument.DocumentNode.SelectSingleNode( "//div[@class='user home profile']" );
			parsed.Name = userHomeProfile.SelectSingleNode( "div[@class='primary header module']/h2[@class='heading']/a" ).ReadableInnerText().Trim();

			HtmlNode metaDl = userHomeProfile.SelectSingleNode( "//dl[@class='meta']" );
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

			return parsed;
		}
	}
}
