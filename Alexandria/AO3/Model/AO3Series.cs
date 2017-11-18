// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Utils;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    internal sealed class AO3Series : RequestableBase<AO3Source>, ISeries
    {
        AO3Series( AO3Source source, Uri url )
            : base( source, url )
        {
        }

        public IAuthorRequestHandle Author { get; private set; }

        public DateTime DateStarted { get; private set; }

        public DateTime DateLastUpdated { get; private set; }

        public bool IsCompleted { get; private set; }

        public IReadOnlyList<IFanficRequestHandle> Fanfics { get; private set; }

        public static AO3Series Parse( AO3Source source, HtmlCacheableDocument document )
        {
            AO3Series parsed = new AO3Series( source, document.Url );

            HtmlNode mainDiv = document.Html.SelectSingleNode( "//div[@id='main']" );

            HtmlNode seriesMetaGroupDl = mainDiv.SelectSingleNode( ".//dl[@class='series meta group']" );
            bool hasDateLastUpdated = false;
            foreach ( Tuple<string, HtmlNode> row in seriesMetaGroupDl.EnumerateDlTable() )
            {
                switch ( row.Item1 )
                {
                    case "Creator":
                        {
                            parsed.Author = AO3AuthorRequestHandle.Parse( source, row.Item2.Element( "a" ) );
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
                            Tuple<string, HtmlNode> completeDd = row.Item2.EnumerateDlTable().FirstOrDefault( kvp => kvp.Item1.Equals( "Complete" ) );
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
            parsed.Fanfics = seriesWorkUl.Elements( "li" ).Select( li => AO3FanficRequestHandle.ParseFromWorkLi( source, li ) ).Cast<IFanficRequestHandle>().ToList();

            return parsed;
        }
    }
}
