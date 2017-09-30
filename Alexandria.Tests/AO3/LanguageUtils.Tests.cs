// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.AO3.Utils;
using Alexandria.Model;
using Alexandria.Utils;
using NUnit.Framework;

namespace Alexandria.Tests.AO3
{
    [TestFixture]
    [Category( UnitTestConstants.UtilTestsCategory )]
    public class LanguageUtilsTests
    {
        static LanguageUtilsTests()
        {
            List<Language> languages = new List<Language>();
            foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
            {
                languages.Add( language );
            }

            _languageEnumValues = languages;

            _ao3LanguageOptions = AO3Utils.GetAllLanguages().ToDictionary( kvp => LanguageUtils.Parse( kvp.Key ), kvp => kvp.Value );
        }

        [Test]
        public void AO3LanguageUtils_AllIdsMatchAO3()
        {
            foreach ( Language language in _languageEnumValues )
            {
                string id = AO3LanguageUtils.GetId( language );
                Assert.AreEqual( _ao3LanguageOptions[language], id );
            }
        }

        [Test]
        public void AO3LanguageUtils_NoExtraLanguages()
        {
            IEnumerable<Language> extraLanguages = _languageEnumValues.Except( _ao3LanguageOptions.Keys );
            Assert.IsFalse( extraLanguages.Any() );
        }

        static readonly IReadOnlyList<Language> _languageEnumValues;
        static readonly IReadOnlyDictionary<Language, string> _ao3LanguageOptions;
    }
}
