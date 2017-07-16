using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.AO3.Utils;
using Alexandria.Model;
using Alexandria.Utils;

namespace Alexandria.Tests.AO3
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

			_ao3LanguageOptions = AO3Utils.GetAllLanguages().ToDictionary( kvp => LanguageUtils.Parse( kvp.Key ), kvp => kvp.Value );
		}

		[TestMethod]
		public void AO3LanguageUtils_AllIdsMatchAO3()
		{
			foreach ( Language language in _languageEnumValues )
			{
				Int32 id = AO3LanguageUtils.GetId( language );
				Assert.AreEqual( _ao3LanguageOptions[language], id );
			}
		}

		[TestMethod]
		public void AO3LanguageUtils_NoExtraLanguages()
		{
			IEnumerable<Language> extraLanguages = _languageEnumValues.Except( _ao3LanguageOptions.Keys );
			Assert.IsFalse( extraLanguages.Any() );
		}

		static readonly IReadOnlyList<Language> _languageEnumValues;
		static readonly IReadOnlyDictionary<Language, Int32> _ao3LanguageOptions;
	}
}
