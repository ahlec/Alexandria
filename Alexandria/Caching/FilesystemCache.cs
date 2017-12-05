// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;

namespace Alexandria.Caching
{
    public sealed class FilesystemCache : Cache
    {
        readonly IFileSystem _filesystem;
        readonly string _cacheDirectory;

        public FilesystemCache( IFileSystem filesystem, string cacheDirectory )
        {
            _filesystem = filesystem ?? throw new ArgumentNullException( nameof( filesystem ) );

            if ( string.IsNullOrWhiteSpace( cacheDirectory ) )
            {
                throw new ArgumentNullException( nameof( cacheDirectory ) );
            }

            if ( !_filesystem.Directory.Exists( _cacheDirectory ) )
            {
                throw new DirectoryNotFoundException( "A valid, existing directory must be provided as the directory to cache files to." );
            }

            _cacheDirectory = cacheDirectory;
        }

        internal static void WriteCacheFilePrefix( Stream stream, Uri url )
        {
            byte[] urlBytes = Encoding.UTF8.GetBytes( url.AbsoluteUri );

            byte[] urlLengthBytes = BitConverter.GetBytes( urlBytes.Length );
            stream.Write( urlLengthBytes, 0, 4 ); // The length of `urlLengthBytes` is guaranteed to be 4

            stream.Write( urlBytes, 0, urlBytes.Length );
        }

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

        string ConstructFileName( string handle )
        {
            string filename = handle + ".htm";
            return Path.Combine( _cacheDirectory, filename );
        }
    }
}
