// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;
using Alexandria.Documents;

namespace Alexandria.Caching
{
    public abstract class Cache
    {
        internal abstract bool Contains<TDocument>( string handle )
            where TDocument : CacheableDocument;

        internal abstract void RemoveItem<TDocument>( string handle )
            where TDocument : CacheableDocument;

        internal bool TryReadFromCache<TDocument>( string handle, out TDocument document )
            where TDocument : CacheableDocument
        {
            if ( !Contains<TDocument>( handle ) )
            {
                document = default( TDocument );
                return false;
            }

            document = ReadFromCache<TDocument>( handle );
            return true;
        }

        internal abstract void WriteToCache<TDocument>( TDocument document )
            where TDocument : CacheableDocument;

        internal abstract CachedDocument GetCachedDocument<TDocument>( string handle )
            where TDocument : CacheableDocument;

        // TODO: Come back here and figure out a way to handle this function better without so much casting
        TDocument ReadFromCache<TDocument>( string handle )
            where TDocument : CacheableDocument
        {
            using ( CachedDocument cachedDocument = GetCachedDocument<TDocument>( handle ) )
            {
                if ( typeof( TDocument ) == typeof( HtmlCacheableDocument ) )
                {
                    return (TDocument) (CacheableDocument) HtmlCacheableDocument.ReadFromStream( handle, cachedDocument.Url, cachedDocument.Stream );
                }

                throw new NotSupportedException( $"Unrecognized {nameof( CacheableDocument )}: {typeof( TDocument )}" );
            }
        }
    }
}
