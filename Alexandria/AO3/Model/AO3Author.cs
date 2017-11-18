// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.Caching;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    internal sealed class AO3Author : RequestableBase<AO3Source>, IAuthor
    {
        AO3Author( AO3Source source, Uri url )
            : base( source, url )
        {
        }

        public string Name { get; private set; }

        public IReadOnlyList<string> Nicknames { get; private set; }

        public DateTime DateJoined { get; private set; }

        public string Location { get; private set; }

        public DateTime? Birthday { get; private set; }

        public string Biography { get; private set; }

        public int NumberFanfics { get; private set; }

        public static AO3Author Parse( AO3Source source, HtmlCacheableDocument profileDocument )
        {
            AO3Author parsed = new AO3Author( source, profileDocument.Url );

            HtmlNode userHomeProfile = profileDocument.Html.SelectSingleNode( "//div[@class='user home profile']" );
            parsed.Name = userHomeProfile.SelectSingleNode( "div[@class='primary header module']/h2[@class='heading']/a" ).ReadableInnerText().Trim();

            HtmlNode metaDl = userHomeProfile.SelectSingleNode( ".//dl[@class='meta']" );
            string lastDtText = null;
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

            HtmlNode dashboardDiv = profileDocument.Html.SelectSingleNode( "//div[@id='dashboard']" );
            foreach ( HtmlNode dashboardA in dashboardDiv.SelectNodes( ".//a" ) )
            {
                if ( !dashboardA.InnerText.StartsWith( "Work" ) )
                {
                    continue;
                }

                int startIndex = dashboardA.InnerText.IndexOf( '(' );
                int endIndex = dashboardA.InnerText.IndexOf( ')' );

                parsed.NumberFanfics = int.Parse( dashboardA.InnerText.Substring( startIndex + 1, endIndex - startIndex - 1 ) );
                break;
            }

            return parsed;
        }

        public IQueryResultsPage<IFanfic, IFanficRequestHandle> QueryFanfics()
        {
            return AO3QueryResults.Retrieve( Source, CacheableObjects.AuthorFanficsHtml, "users", Name, 1 );
        }

        static IEnumerable<string> CollectPseuds( HtmlNode pseudsDd )
        {
            foreach ( HtmlNode pseudA in pseudsDd.Elements( "a" ) )
            {
                yield return pseudA.ReadableInnerText().Trim();
            }
        }
    }
}
