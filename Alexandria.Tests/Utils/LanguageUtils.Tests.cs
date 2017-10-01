// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Exceptions.Input;
using Alexandria.Model;
using Alexandria.Utils;
using NUnit.Framework;

namespace Alexandria.Tests.Utils
{
    [TestFixture]
    public class LanguageUtilsTests
    {
        [Test]
        public void LanguageUtils_Parse_ThrowsOnNull()
        {
            Assert.That( () => LanguageUtils.Parse( null ), Throws.ArgumentNullException );
        }

        [Test]
        public void LanguageUtils_Parse_ThrowsOnInvalidLanguage()
        {
            Assert.That( () => LanguageUtils.Parse( "C#" ), Throws.Exception.TypeOf<NoSuchLanguageAlexandriaException>() );
            Assert.That( () => LanguageUtils.Parse( string.Empty ), Throws.Exception.TypeOf<NoSuchLanguageAlexandriaException>() );
            Assert.That( () => LanguageUtils.Parse( "      " ), Throws.Exception.TypeOf<NoSuchLanguageAlexandriaException>() );
            Assert.That( () => LanguageUtils.Parse( "Orrian" ), Throws.Exception.TypeOf<NoSuchLanguageAlexandriaException>() );
        }

        [Test]
        public void LanguageUtils_Parse_TrimsWhitespace()
        {
            Assert.That( LanguageUtils.Parse( "  English  " ), Is.EqualTo( Language.English ) );
        }

        [Test]
        public void LanguageUtils_Parse_ParsesAllEnumValues()
        {
            foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
            {
                Assert.That( LanguageUtils.Parse( language.ToString() ), Is.EqualTo( language ) );
            }
        }

        [Test]
        public void LanguageUtils_Parse_ParsesAllNativeNameValues()
        {
            foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
            {
                ILanguageInfo info = LanguageUtils.GetInfo( language );
                Assert.That( LanguageUtils.Parse( info.NativeName ), Is.EqualTo( language ) );
            }
        }
    }
}
