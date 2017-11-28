// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.Model;
using Alexandria.Utils;
using NUnit.Framework;

namespace Alexandria.Tests.AO3.Conformity
{
    [TestFixture]
    [Category( AlexandriaTestConstants.ConformityTestsCategory )]
    public partial class LanguagesTests
    {
        IReadOnlyList<AO3Language> _languages;

        [OneTimeSetUp]
        public void Setup()
        {
            _languages = PullDownLanguages();
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
            foreach ( LanguageInfo language in Languages.AllLanguages )
            {
                AO3Language ao3Language = _languages.FirstOrDefault( data => data.AlexandriaValue == language.Language );

                if ( ao3Language != null )
                {
                    Assert.That( ao3Language.AO3Id, Is.EqualTo( language.AO3Id ) );
                }
            }
        }

        [Test]
        public void AO3_Languages_NoExtraLanguages()
        {
            HashSet<Language> allDefinedAO3Languages = new HashSet<Language>( _languages.Select( data => data.AlexandriaValue )
                                                                                        .Where( lang => lang != null )
                                                                                        .Select( lang => lang.Value ) );
            IEnumerable<Language> alexandriaLanguages = Enum.GetValues( typeof( Language ) ).Cast<Language>();
            IEnumerable<Language> extraLanguages = alexandriaLanguages.Except( allDefinedAO3Languages );
            Assert.That( extraLanguages, Is.Empty );
        }
    }
}
