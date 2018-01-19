// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Alexandria.Languages;
using NUnit.Framework;

namespace Alexandria.Tests.Mocks
{
    /// <summary>
    /// A <see cref="ILanguageManager"/> that should be used when creating <see cref="LibrarySource"/>s
    /// that should not be doing any language-related calls. All calls to this manager will cause a
    /// test failure.
    /// </summary>
    public sealed class IgnoredLanguageManager : ILanguageManager
    {
        public IReadOnlyList<Language> AllLanguages
        {
            get
            {
                Assert.Fail( $"{nameof( ILanguageManager )}.{nameof( AllLanguages )} was called." );
                return null;
            }
        }

        public Language GetLanguage( string input )
        {
            Assert.Fail( $"{nameof( ILanguageManager )}.{nameof( GetLanguage )} was called." );
            return null;
        }
    }
}
