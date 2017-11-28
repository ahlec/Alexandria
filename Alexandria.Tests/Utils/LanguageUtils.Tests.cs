// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Exceptions.Input;
using Alexandria.Model;
using NUnit.Framework;

namespace Alexandria.Tests.Utils
{
    [TestFixture]
    public class LanguageUtilsTests
    {
        [Test]
        public void LanguageUtils_Parse_ThrowsOnNull()
        {
            Assert.That( () => Languages.Parse( null ), Throws.ArgumentNullException );
        }

        [Test]
        public void LanguageUtils_Parse_ThrowsOnInvalidLanguage()
        {
            Assert.That( () => Languages.Parse( "C#" ), Throws.Exception.TypeOf<NoSuchLanguageAlexandriaException>() );
            Assert.That( () => Languages.Parse( string.Empty ), Throws.Exception.TypeOf<NoSuchLanguageAlexandriaException>() );
            Assert.That( () => Languages.Parse( "      " ), Throws.Exception.TypeOf<NoSuchLanguageAlexandriaException>() );
            Assert.That( () => Languages.Parse( "Orrian" ), Throws.Exception.TypeOf<NoSuchLanguageAlexandriaException>() );
        }

        [Test]
        public void LanguageUtils_Parse_TrimsWhitespace()
        {
            LanguageInfo info = Languages.GetInfo( Language.English );
            Assert.That( Languages.Parse( "  English  " ), Is.EqualTo( info ) );
        }

        [Test]
        public void LanguageUtils_Parse_ParsesAllEnumValues()
        {
            foreach ( LanguageInfo info in Languages.AllLanguages )
            {
                Assert.That( Languages.Parse( info.Language.ToString() ), Is.EqualTo( info ) );
            }
        }

        [Test]
        public void LanguageUtils_Parse_ParsesAllNativeNameValues()
        {
            foreach ( LanguageInfo info in Languages.AllLanguages )
            {
                Assert.That( Languages.Parse( info.NativeName ), Is.EqualTo( info ) );
            }
        }
    }
}
