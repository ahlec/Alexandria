// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.Model;
using Alexandria.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alexandria.Tests
{
    [TestClass]
    [TestCategory( UnitTestConstants.UtilTestsCategory )]
    public class Test_LanguageUtils
    {
        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ), AllowDerivedTypes = false )]
        public void BaseLanguageUtils_ParseThrowsOnNull()
        {
            LanguageUtils.Parse( null );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ), AllowDerivedTypes = false )]
        public void BaseLanguageUtils_ParseThrowsOnEmpty()
        {
            LanguageUtils.Parse( string.Empty );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ), AllowDerivedTypes = false )]
        public void BaseLanguageUtils_ParseThrowsOnWhitespace()
        {
            LanguageUtils.Parse( "   " );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ), AllowDerivedTypes = false )]
        public void BaseLanguageUtils_ThrowsOnInvalidLanguage()
        {
            LanguageUtils.Parse( "C#" );
        }

        [TestMethod]
        public void BaseLanguageUtils_ParsesAllEnumValues()
        {
            foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
            {
                Language parsed = LanguageUtils.Parse( language.ToString() );
                Assert.AreEqual( language, parsed );
            }
        }

        [TestMethod]
        public void BaseLanguageUtils_ParsesAllNativeNameValues()
        {
            foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
            {
                ILanguageInfo info = LanguageUtils.GetInfo( language );
                Language parsed = LanguageUtils.Parse( info.NativeName );
                Assert.AreEqual( language, parsed );
            }
        }

        [TestMethod]
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
