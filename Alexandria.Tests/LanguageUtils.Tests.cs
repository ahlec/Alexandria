// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.Model;
using Alexandria.Utils;
using NUnit.Framework;

namespace Alexandria.Tests
{
    [TestFixture]
    [Category( UnitTestConstants.UtilTestsCategory )]
    public class LanguageUtilsTests
    {
        [Test]
        public void BaseLanguageUtils_ParseThrowsOnInvalidArguments()
        {
            Assert.Throws<ArgumentNullException>( () => LanguageUtils.Parse( null ) );
            Assert.Throws<ArgumentNullException>( () => LanguageUtils.Parse( string.Empty ) );
            Assert.Throws<ArgumentNullException>( () => LanguageUtils.Parse( "      " ) );
        }

        [Test]
        public void BaseLanguageUtils_ThrowsOnInvalidLanguage()
        {
            Assert.Throws<ArgumentException>( () => LanguageUtils.Parse( "C#" ) );
        }

        [Test]
        public void BaseLanguageUtils_ParsesAllEnumValues()
        {
            foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
            {
                Language parsed = LanguageUtils.Parse( language.ToString() );
                Assert.AreEqual( language, parsed );
            }
        }

        [Test]
        public void BaseLanguageUtils_ParsesAllNativeNameValues()
        {
            foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
            {
                ILanguageInfo info = LanguageUtils.GetInfo( language );
                Language parsed = LanguageUtils.Parse( info.NativeName );
                Assert.AreEqual( language, parsed );
            }
        }

        [Test]
        public void BaseLanguageUtils_AllAO3LanguagesDefined()
        {
            IReadOnlyDictionary<string, int> ao3Languages = AO3Utils.GetAllLanguages();
            foreach ( string language in ao3Languages.Keys )
            {
                LanguageUtils.Parse( language );
            }
        }
    }
}
