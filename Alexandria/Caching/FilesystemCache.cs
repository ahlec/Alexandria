// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Alexandria.Documents;

namespace Alexandria.Caching
{
    public sealed class FilesystemCache : Cache
    {
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

        /// <inheritdoc />
        internal override bool Contains<TDocument>( string handle )
        {
            string filename = ConstructFileName<TDocument>( handle );
            return _filesystem.File.Exists( filename );
        }

        /// <inheritdoc />
        internal override void WriteToCache<TDocument>( TDocument document )
        {
            string filename = ConstructFileName<TDocument>( document.Handle );
            using ( Stream stream = _filesystem.File.Create( filename ) )
            {
                document.Write( stream );
            }
        }

        /// <inheritdoc />
        internal override Stream GetCachedDocumentStream<TDocument>( string handle )
        {
            string filename = ConstructFileName<TDocument>( handle );
            return _filesystem.File.OpenRead( filename );
        }

        string ConstructFileName<TDocument>( string handle )
            where TDocument : CacheableDocument
        {
            if ( !_documentTypeExtensions.TryGetValue( typeof( TDocument ), out string extension ) )
            {
                throw new NotImplementedException( $"Unsupported {nameof( CacheableDocument )}: {typeof( TDocument )}" );
            }

            string filename = handle + "." + extension;
            return Path.Combine( _cacheDirectory, filename );
        }

        static readonly IReadOnlyDictionary<Type, string> _documentTypeExtensions = new Dictionary<Type, string>
        {
            { typeof( HtmlCacheableDocument ), "htm" }
        };

        readonly IFileSystem _filesystem;
        readonly string _cacheDirectory;
    }
}
