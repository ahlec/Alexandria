// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;

namespace Alexandria.Caching
{
    /// <summary>
    /// A document that has has been stored in a <seealso cref="Cache"/>. This is meant for
    /// use only within the Caching namespace, and should be transfered to <seealso cref="Document"/>
    /// before it is consumed by another non-Caching class.
    /// </summary>
    internal sealed class CachedDocument : IDisposable
    {
        bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedDocument"/> class.
        /// </summary>
        /// <param name="url">The url that the document was retrieved from.</param>
        /// <param name="stream">A stream with the contents of the document.</param>
        public CachedDocument( Uri url, Stream stream )
        {
            Url = url ?? throw new ArgumentNullException( nameof( url ) );
            Stream = stream ?? throw new ArgumentNullException( nameof( stream ) );
        }

        /// <summary>
        /// Gets the url that the document was retrieved from originally.
        /// </summary>
        public Uri Url { get; }

        /// <summary>
        /// Gets a stream with the contents of the document.
        /// </summary>
        public Stream Stream { get; }

        /// <summary>
        /// Disposes of the document cleanly.
        /// </summary>
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
