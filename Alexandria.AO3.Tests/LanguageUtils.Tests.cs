using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.AO3.Utils;
using Alexandria.Model;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria.AO3.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.UtilTestsCategory )]
	public class Test_LanguageUtils
	{
		static Test_LanguageUtils()
		{
			List<Language> languages = new List<Language>();
			foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
			{
				languages.Add( language );
			}
			_languageEnumValues = languages;
		}

		public Test_LanguageUtils()
		{
			Dictionary<Language, Int32> languageIds = new Dictionary<Language, Int32>();

			HtmlDocument searchPage = HtmlUtils.GetWebPage( "http://archiveofourown.org/works/search" );
			HtmlNode languageSelect = searchPage.DocumentNode.SelectSingleNode( "//select[@id='work_search_language_id']" );
			foreach ( HtmlNode option in languageSelect.Elements( HtmlUtils.OptionsHtmlTag ) )
			{
				String idStr = option.GetAttributeValue( "value", null );
				if ( String.IsNullOrWhiteSpace( idStr ) )
				{
					continue;
				}
				Int32 languageId = Int32.Parse( idStr );

				String languageStr = option.InnerText;
				Language language = LanguageUtils.Parse( languageStr );
				languageIds.Add( language, languageId );
			}

			_languageIds = languageIds;
		}

		[TestMethod]
		public void AO3LanguageUtils_AllIdsMatchAO3()
		{
			foreach ( Language language in _languageEnumValues )
			{
				Int32 id = AO3LanguageUtils.GetId( language );
				Assert.AreEqual( _languageIds[language], id );
			}
		}

		[TestMethod]
		public void AO3LanguageUtils_NoExtraLanguages()
		{
			IEnumerable<Language> extraLanguages = _languageEnumValues.Except( _languageIds.Keys );
			Assert.IsFalse( extraLanguages.Any() );
		}

		static readonly IReadOnlyList<Language> _languageEnumValues;
		readonly IReadOnlyDictionary<Language, Int32> _languageIds;
	}
}
