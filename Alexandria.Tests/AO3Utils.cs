// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Alexandria.Net;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria.Tests
{
    public static class AO3Utils
    {
        public static IReadOnlyDictionary<string, int> GetAllLanguages()
        {
            IWebClient webClient = new HttpWebClient();
            HtmlDocument searchPage;
            using ( WebResult result = webClient.Get( "http://archiveofourown.org/works/search" ) )
            {
                searchPage = HtmlUtils.ParseHtmlDocument( result.ResponseText );
            }

            HtmlNode languageSelect = searchPage.DocumentNode.SelectSingleNode( "//select[@id='work_search_language_id']" );
            Dictionary<string, int> ao3Options = new Dictionary<string, int>();
            foreach ( HtmlNode option in languageSelect.Elements( HtmlUtils.OptionsHtmlTag ) )
            {
                string idStr = option.GetAttributeValue( "value", null );
                if ( string.IsNullOrWhiteSpace( idStr ) )
                {
                    continue;
                }

                int languageId = int.Parse( idStr );

                ao3Options.Add( option.InnerText, languageId );
            }

            return ao3Options;
        }
    }
}
