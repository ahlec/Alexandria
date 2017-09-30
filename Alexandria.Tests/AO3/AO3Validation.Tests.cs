// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3;
using NUnit.Framework;

namespace Alexandria.Tests.AO3
{
    [TestFixture]
    public sealed class AO3ValidationTests
    {
        [Test]
        public void IsValidAuthorNameDetectsNull()
        {
            Assert.IsFalse( AO3Validation.IsValidAuthorName( null, out string reason ) );
            Asserts.IsStringNotNullOrEmpty( reason );
        }

        [Test]
        public void IsValidAuthorNameDetectsBadLengths()
        {
            Assert.IsFalse( AO3Validation.IsValidAuthorName( string.Empty, out string reason ) );
            Asserts.IsStringNotNullOrEmpty( reason );

            Assert.IsFalse( AO3Validation.IsValidAuthorName( "12345678901234567890123456789012345678901", out reason ) );
            Asserts.IsStringNotNullOrEmpty( reason );
        }

        [Test]
        public void IsValidAuthorNameDetectsInvalidCharacters()
        {
            Assert.IsFalse( AO3Validation.IsValidAuthorName( "&", out string reason ) );
            Asserts.IsStringNotNullOrEmpty( reason );

            Assert.IsFalse( AO3Validation.IsValidAuthorName( "hello #world", out reason ) );
            Asserts.IsStringNotNullOrEmpty( reason );

            Assert.IsFalse( AO3Validation.IsValidAuthorName( "漢字◎", out reason ) );
            Asserts.IsStringNotNullOrEmpty( reason );
        }

        [Test]
        public void IsValidAuthorNameDetectsMissingLeterOrDigit()
        {
            Assert.IsFalse( AO3Validation.IsValidAuthorName( "-------------", out string reason ) );
            Asserts.IsStringNotNullOrEmpty( reason );
        }

        [Test]
        public void IsValidAuthorNameValidNames()
        {
            Assert.IsTrue( AO3Validation.IsValidAuthorName( "Hello World", out string reason ) );
            Assert.IsNull( reason );

            Assert.IsTrue( AO3Validation.IsValidAuthorName( "京介", out reason ) );
            Assert.IsNull( reason );

            Assert.IsTrue( AO3Validation.IsValidAuthorName( "Cloak", out reason ) );
            Assert.IsNull( reason );

            Assert.IsTrue( AO3Validation.IsValidAuthorName( "12345-67890", out reason ) );
            Assert.IsNull( reason );
        }
    }
}
