// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using NUnit.Framework;

namespace Alexandria.Tests
{
    public static class Asserts
    {
        public static void IsStringNotNullOrEmpty( string str, string message = null, params object[] args )
        {
            Assert.IsNotNull( str, message, args );
            Assert.IsNotEmpty( str, message, args );
        }
    }
}
