// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Caching
{
    public abstract class Cache
    {
        internal abstract bool Contains( string handle );

        internal abstract void RemoveItem( string handle );

        internal bool TryReadFromCache( string handle, out Document document )
        {
            if ( !Contains( handle ) )
            {
                document = null;
                return false;
            }

            document = ReadFromCache( handle );
            return true;
        }

        internal abstract void WriteToCache( Document document );

        internal abstract CachedDocument GetCachedDocument( string handle );

        Document ReadFromCache( string handle )
        {
            using ( CachedDocument cachedDocument = GetCachedDocument( handle ) )
            {
                return Document.ReadFromStream( handle, cachedDocument.Url, cachedDocument.Stream );
            }
        }
    }
}
