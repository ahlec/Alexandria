// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;

namespace Alexandria.Caching
{
    internal sealed class CachedDocument : IDisposable
    {
        bool _isDisposed;

        public CachedDocument( Uri url, Stream stream )
        {
            Url = url ?? throw new ArgumentNullException( nameof( url ) );
            Stream = stream ?? throw new ArgumentNullException( nameof( stream ) );
        }

        public Uri Url { get; }

        public Stream Stream { get; }

        public void Dispose()
        {
            if ( _isDisposed )
            {
                return;
            }

            _isDisposed = true;
            Stream.Dispose();
        }
    }
}
