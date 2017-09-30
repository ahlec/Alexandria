// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;

namespace Alexandria.Documents
{
    internal abstract class CacheableDocument
    {
        protected CacheableDocument( string handle, Uri url )
        {
            if ( string.IsNullOrWhiteSpace( handle ) )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            Handle = handle;
            Url = url ?? throw new ArgumentNullException( nameof( url ) );
        }

        public string Handle { get; }

        public Uri Url { get; }

        public abstract void Write( Stream stream );
    }
}
