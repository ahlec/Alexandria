// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using Alexandria.RequestHandles;

namespace Alexandria.Caching
{
    /// <summary>
    /// An implementation of <see cref="Cache"/> that stores each item in the cache
    /// as a separate file in a specific directory on the filesystem.
    /// </summary>
    public sealed class FilesystemCache : Cache
    {
        readonly IFileSystem _filesystem;
        readonly string _cacheDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemCache"/> class.
        /// </summary>
        /// <param name="filesystem">The filesystem abstraction that can be used to perform the
        /// filesystem IO operations.</param>
        /// <param name="cacheDirectory">The specific directory that is being used for storing
        /// the items of the cache. This directory should exist and, ideally, be used ONLY for
        /// storing items in the cache.</param>
        public FilesystemCache( IFileSystem filesystem, string cacheDirectory )
        {
            _filesystem = filesystem ?? throw new ArgumentNullException( nameof( filesystem ) );

            if ( string.IsNullOrWhiteSpace( cacheDirectory ) )
            {
                throw new ArgumentNullException( nameof( cacheDirectory ) );
            }

            if ( !_filesystem.Directory.Exists( cacheDirectory ) )
            {
                throw new DirectoryNotFoundException( "A valid, existing directory must be provided as the directory to cache files to." );
            }

            _cacheDirectory = cacheDirectory;
        }

        /// <summary>
        /// Writes a header for the cached document, storing metadata for serialization.
        /// <para />
        /// See also: <seealso cref="ReadCacheFilePrefix"/>
        /// </summary>
        /// <param name="stream">The writable output stream that should have the metadata encoded
        /// into it.</param>
        /// <param name="url">The url that the document was originally retrieved from.</param>
        internal static void WriteCacheFilePrefix( Stream stream, Uri url )
        {
            byte[] urlBytes = Encoding.UTF8.GetBytes( url.AbsoluteUri );

            byte[] urlLengthBytes = BitConverter.GetBytes( urlBytes.Length );
            stream.Write( urlLengthBytes, 0, 4 ); // The length of `urlLengthBytes` is guaranteed to be 4

            stream.Write( urlBytes, 0, urlBytes.Length );
        }

        /// <summary>
        /// Reads the header that was encoded with <seealso cref="WriteCacheFilePrefix"/> to retrieve
        /// the metadata from an item that was serialized in the cache.
        /// </summary>
        /// <param name="stream">The readable input stream for an item being deserialized from the cache
        /// that should have its metadata read and the stream advanced accordingly.</param>
        /// <returns>Returns the url that the document was originally retrieved from.</returns>
        internal static Uri ReadCacheFilePrefix( Stream stream )
        {
            byte[] urlLengthBytes = new byte[4];
            int numBytesRead = stream.Read( urlLengthBytes, 0, 4 );
            if ( numBytesRead != 4 )
            {
                throw new InvalidDataException( $"Was only able to read {numBytesRead} byte(s) from the prefix header when reading the url length." );
            }

            int urlLength = BitConverter.ToInt32( urlLengthBytes, 0 );
            byte[] urlBytes = new byte[urlLength];
            numBytesRead = stream.Read( urlBytes, 0, urlLength );
            if ( numBytesRead != urlLength )
            {
                throw new InvalidDataException( $"Was only able to read {numBytesRead} byte(s) when expecting to read {urlLength} byte(s) of url from prefix header." );
            }

            string url = Encoding.UTF8.GetString( urlBytes );
            return new Uri( url );
        }

        /// <inheritdoc />
        internal override bool Contains( string handle )
        {
            string filename = ConstructFileName( handle );
            return _filesystem.File.Exists( filename );
        }

        /// <inheritdoc />
        internal override void RemoveItem( string handle )
        {
            string filename = ConstructFileName( handle );
            _filesystem.File.Delete( filename );
        }

        /// <inheritdoc />
        internal override void WriteToCache( Document document )
        {
            string filename = ConstructFileName( document.Handle );
            using ( Stream stream = _filesystem.File.Create( filename ) )
            {
                WriteCacheFilePrefix( stream, document.Url );
                document.WriteHtmlToStream( stream );
            }
        }

        /// <inheritdoc />
        internal override CachedDocument GetCachedDocument( string handle )
        {
            string filename = ConstructFileName( handle );
            Stream documentStream = _filesystem.File.OpenRead( filename );
            Uri url = ReadCacheFilePrefix( documentStream );
            return new CachedDocument( url, documentStream );
        }

        /// <summary>
        /// Constructs the appropriate filename for the cached document from the handle
        /// of the document. This is deterministic and the same input will always produce
        /// the same output.
        /// </summary>
        /// <param name="handle">The handle for the specific <seealso cref="IRequestHandle{TModel}"/> class.
        /// This will uniquely identify not only the type of the data being requested, but will also
        /// indicate the unique ID of the data being retrieved. Handles are formated with sufficient complexity
        /// that collisions should never happen. This parameter is never null.</param>
        /// <returns>Returns the non-null filename for the file that the cached document should be
        /// stored in for the particular handle.</returns>
        internal string ConstructFileName( string handle )
        {
            string filename = handle + ".dat";
            return Path.Combine( _cacheDirectory, filename );
        }
    }
}
