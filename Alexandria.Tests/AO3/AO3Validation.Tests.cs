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
            Assert.That( reason, Is.Not.Null.Or.Empty );
        }

        [Test]
        public void IsValidAuthorNameDetectsBadLengths()
        {
            Assert.IsFalse( AO3Validation.IsValidAuthorName( string.Empty, out string reason ) );
            Assert.That( reason, Is.Not.Null.Or.Empty );

            Assert.IsFalse( AO3Validation.IsValidAuthorName( "12345678901234567890123456789012345678901", out reason ) );
            Assert.That( reason, Is.Not.Null.Or.Empty );
        }

        [Test]
        public void IsValidAuthorNameDetectsInvalidCharacters()
        {
            Assert.IsFalse( AO3Validation.IsValidAuthorName( "&", out string reason ) );
            Assert.That( reason, Is.Not.Null.Or.Empty );

            Assert.IsFalse( AO3Validation.IsValidAuthorName( "hello #world", out reason ) );
            Assert.That( reason, Is.Not.Null.Or.Empty );

            Assert.IsFalse( AO3Validation.IsValidAuthorName( "漢字◎", out reason ) );
            Assert.That( reason, Is.Not.Null.Or.Empty );
        }

        [Test]
        public void IsValidAuthorNameDetectsMissingLeterOrDigit()
        {
            Assert.IsFalse( AO3Validation.IsValidAuthorName( "-------------", out string reason ) );
            Assert.That( reason, Is.Not.Null.Or.Empty );
        }

        [Test]
        public void IsValidAuthorNameValidNames()
        {
            Assert.IsTrue( AO3Validation.IsValidAuthorName( "Hello World", out string reason ) );
            Assert.That( reason, Is.Null );

            Assert.IsTrue( AO3Validation.IsValidAuthorName( "京介", out reason ) );
            Assert.That( reason, Is.Null );

            Assert.IsTrue( AO3Validation.IsValidAuthorName( "Cloak", out reason ) );
            Assert.That( reason, Is.Null );

            Assert.IsTrue( AO3Validation.IsValidAuthorName( "12345-67890", out reason ) );
            Assert.That( reason, Is.Null );
        }

        [Test]
        public void IsValidTagDetectsNull()
        {
            Assert.IsFalse( AO3Validation.IsValidTag( null, out string reason ) );
            Assert.That( reason, Is.Not.Null.Or.Empty );
        }

        [Test]
        public void IsValidTagDetectsBadLengths()
        {
            Assert.IsFalse( AO3Validation.IsValidTag( string.Empty, out string reason ) );
            Assert.That( reason, Is.Not.Null.Or.Empty );

            Assert.IsFalse( AO3Validation.IsValidTag( new string( 'a', 101 ), out reason ) );
            Assert.That( reason, Is.Not.Null.Or.Empty );
        }

        public void IsValidTagBoundaryLengthsPass()
        {
            Assert.IsTrue( AO3Validation.IsValidTag( "1", out string reason ) );
            Assert.That( reason, Is.Null );

            Assert.IsTrue( AO3Validation.IsValidTag( new string( 'a', 100 ), out reason ) );
            Assert.That( reason, Is.Null );
        }

        [Test]
        public void IsValidTagDetectsInvalidCharacters()
        {
            Assert.IsFalse( AO3Validation.IsValidAuthorName( ",", out string reason ) );
            Assert.That( reason, Is.Not.Null.Or.Empty );

            Assert.IsFalse( AO3Validation.IsValidAuthorName( "\\ ^ {", out reason ) );
            Assert.That( reason, Is.Not.Null.Or.Empty );

            Assert.IsFalse( AO3Validation.IsValidAuthorName( "`=`", out reason ) );
            Assert.That( reason, Is.Not.Null.Or.Empty );
        }

        [Test]
        public void IsValidTagValidTags()
        {
            Assert.IsTrue( AO3Validation.IsValidTag( "Hello World", out string reason ) );
            Assert.That( reason, Is.Null );

            Assert.IsTrue( AO3Validation.IsValidTag( "京介", out reason ) );
            Assert.That( reason, Is.Null );

            Assert.IsTrue( AO3Validation.IsValidTag( "this is a really long tag that's got some \"basic\" punctuation in it.", out reason ) );
            Assert.That( reason, Is.Null );

            Assert.IsTrue( AO3Validation.IsValidTag( "12345-67890", out reason ) );
            Assert.That( reason, Is.Null );
        }
    }
}
