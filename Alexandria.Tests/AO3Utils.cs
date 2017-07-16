using System;
using System.Collections.Generic;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria.Tests
{
	public static class AO3Utils
	{
		public static IReadOnlyDictionary<String, Int32> GetAllLanguages()
		{
			HtmlDocument searchPage = HtmlUtils.GetWebPage( "http://archiveofourown.org/works/search" );
			HtmlNode languageSelect = searchPage.DocumentNode.SelectSingleNode( "//select[@id='work_search_language_id']" );
			Dictionary<String, Int32> ao3Options = new Dictionary<String, Int32>();
			foreach ( HtmlNode option in languageSelect.Elements( HtmlUtils.OptionsHtmlTag ) )
			{
				String idStr = option.GetAttributeValue( "value", null );
				if ( String.IsNullOrWhiteSpace( idStr ) )
				{
					continue;
				}
				Int32 languageId = Int32.Parse( idStr );

				ao3Options.Add( option.InnerText, languageId );
			}
			return ao3Options;
		}
	}
}
