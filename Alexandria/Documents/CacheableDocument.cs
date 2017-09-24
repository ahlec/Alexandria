// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;

namespace Alexandria.Documents
{
    internal abstract class CacheableDocument
    {
        protected CacheableDocument( string handle )
        {
            if ( string.IsNullOrWhiteSpace( handle ) )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            Handle = handle;
        }

        public string Handle { get; }

        public abstract void Write( Stream stream );
    }
}
