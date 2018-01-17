// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.RequestHandles;

namespace Alexandria.Caching
{
    /// <summary>
    /// A base class for a cache that Alexandria and <seealso cref="LibrarySource"/> is capable of using.
    /// This provides an API and common implementation of functions that allow for storing retrieve web
    /// responses in various ways (the backing and implementation of it being left up to the user), with various
    /// methods of retention (also left up to the user).
    /// </summary>
    public abstract class Cache
    {
        /// <summary>
        /// Determines if the provided handle has any retrievable data stored in the cache.
        /// </summary>
        /// <param name="handle">The handle for the specific <seealso cref="IRequestHandle{TModel}"/> class.
        /// This will uniquely identify not only the type of the data being requested, but will also
        /// indicate the unique ID of the data being retrieved. Handles are formated with sufficient complexity
        /// that collisions should never happen. This parameter is never null.</param>
        /// <returns>Returns true if the provided handle has data stored in the cache already, false otherwise.</returns>
        internal abstract bool Contains( string handle );

        /// <summary>
        /// Removes data from the cache for the provided handle.
        /// <para />
        /// NOTE: There is no guarantee that data will already exist for that handle, and it is up to the implementation
        /// of this function to gracefully handle the case where this function is called for a handle that does not have
        /// data.
        /// </summary>
        /// <param name="handle">The handle for the specific <seealso cref="IRequestHandle{TModel}"/> class.
        /// This will uniquely identify not only the type of the data being requested, but will also
        /// indicate the unique ID of the data being retrieved. Handles are formated with sufficient complexity
        /// that collisions should never happen. This parameter is never null.</param>
        internal abstract void RemoveItem( string handle );

        /// <summary>
        /// Attempts to safely read the document from the cache if data for it exists.
        /// </summary>
        /// <param name="handle">The handle for the specific <seealso cref="IRequestHandle{TModel}"/> class.
        /// This will uniquely identify not only the type of the data being requested, but will also
        /// indicate the unique ID of the data being retrieved. Handles are formated with sufficient complexity
        /// that collisions should never happen. This parameter is never null.</param>
        /// <param name="document">The out-parameter variable that should be read into if the handle has data
        /// stored for it in the cache. This will be a valid document if this function returns true, or null if
        /// the function returns false.</param>
        /// <returns>Returns true if a document was successfully read from the cache, or false if the handle
        /// did not have data stored for it.</returns>
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

        /// <summary>
        /// Saves the provided document to the cache.
        /// </summary>
        /// <param name="document">The non-null document that should be stored in the cache.</param>
        internal abstract void WriteToCache( Document document );

        /// <summary>
        /// Retrieves the cached document for a particular handle.
        /// <para />
        /// This function should only be called when the handle has data stored in the cache.
        /// </summary>
        /// <param name="handle">The handle for the specific <seealso cref="IRequestHandle{TModel}"/> class.
        /// This will uniquely identify not only the type of the data being requested, but will also
        /// indicate the unique ID of the data being retrieved. Handles are formated with sufficient complexity
        /// that collisions should never happen. This parameter is never null.</param>
        /// <returns>The cached document that was previously stored in the cache.</returns>
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
