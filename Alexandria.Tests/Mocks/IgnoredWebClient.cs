// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Net;
using NUnit.Framework;

namespace Alexandria.Tests.Mocks
{
    /// <summary>
    /// A <see cref="IWebClient"/> that should be used when creating <see cref="LibrarySource"/>s
    /// that should not be doing any networking calls. All calls to this client will cause a test
    /// failure.
    /// </summary>
    public sealed class IgnoredWebClient : IWebClient
    {
        public WebResult Get( string url )
        {
            Assert.Fail( $"{nameof( IWebClient )}.{nameof( Get )} was called." );
            return null;
        }
    }
}
