// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Alexandria.Languages;
using Alexandria.Net;
using NUnit.Framework;

namespace Alexandria.Tests.AO3.Nightly
{
    [TestFixture]
    [Category( AlexandriaTestConstants.NightlyTestsCategory )]
    public partial class LanguagesTests
    {
        IReadOnlyList<AO3Language> _languages;
        ILanguageManager _languageManager;

        [OneTimeSetUp]
        public void Setup()
        {
            HttpWebClient webClient = new HttpWebClient();
            _languageManager = new WebLanguageManager( webClient );
            _languages = PullDownLanguages( webClient, _languageManager );
        }

        [Test]
        public void AO3_Languages_AllLanguagesDefined()
        {
            foreach ( AO3Language language in _languages )
            {
                Assert.That( language.AlexandriaValue, Is.Not.Null );
            }
        }

        [Test]
        public void AO3_Languages_AllIdsMatchAO3()
        {
            foreach ( AO3Language language in _languages )
            {
                Assert.That( language.AO3Id, Is.EqualTo( language.AlexandriaValue.AO3Id ) );
            }
        }

        [Test]
        public void AO3_Languages_NoExtraLanguages()
        {
            HashSet<Language> allDefinedAO3Languages = new HashSet<Language>( _languages.Select( data => data.AlexandriaValue )
                                                                                        .Where( lang => lang != null ) );
            IEnumerable<Language> extraLanguages = _languageManager.AllLanguages.Except( allDefinedAO3Languages );
            Assert.That( extraLanguages, Is.Empty );
        }
    }
}
