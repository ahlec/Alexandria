// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.Languages;
using Alexandria.Net;
using HtmlAgilityPack;

namespace Alexandria.Tests.AO3.Nightly
{
    public partial class LanguagesTests
    {
        static IReadOnlyList<AO3Language> PullDownLanguages( HttpWebClient webClient, WebLanguageManager languageManager )
        {
            HtmlNode searchPageDocumentNode;
            using ( WebResult result = webClient.Get( "http://archiveofourown.org/works/search" ) )
            {
                Document searchPage = Document.ParseFromWebResult( Website.AO3, "%%%PullDownLanguages", result );
                searchPageDocumentNode = searchPage.Html;
            }

            HtmlNode languageSelect = searchPageDocumentNode.SelectSingleNode( "//select[@id='work_search_language_id']" );
            List<AO3Language> ao3Languages = new List<AO3Language>();
            foreach ( HtmlNode option in languageSelect.Elements( "option" ) )
            {
                string idStr = option.GetAttributeValue( "value", null );
                if ( string.IsNullOrWhiteSpace( idStr ) )
                {
                    continue;
                }

                int id = int.Parse( idStr );
                ao3Languages.Add( new AO3Language( languageManager, option.InnerText, id ) );
            }

            return ao3Languages;
        }

        class AO3Language
        {
            public AO3Language( WebLanguageManager languageManager, string ao3Name, int ao3Id )
            {
                if ( string.IsNullOrWhiteSpace( ao3Name ) )
                {
                    throw new ArgumentNullException( nameof( ao3Name ) );
                }

                AO3Name = ao3Name;
                AO3Id = ao3Id;
                AlexandriaValue = languageManager.GetLanguage( AO3Name );
            }

            public string AO3Name { get; }

            public int AO3Id { get; }

            public Language AlexandriaValue { get; }
        }
    }
}
