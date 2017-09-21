// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.Searching;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria
{
    public abstract class LibrarySource
    {
        protected LibrarySource( LibrarySourceConfig config )
        {
            if ( config == null )
            {
                throw new ArgumentNullException( nameof( config ) );
            }

            if ( !config.IsSealed )
            {
                throw new ArgumentException( $"You can only construct new {nameof( LibrarySource )} instances with a sealed {nameof( LibrarySourceConfig )}" );
            }

            _config = config;
            _personalCacheDirectory = Path.Combine( config.CacheBaseDirectory, GetType().Name );
        }

        public abstract string SourceHandle { get; }

        public abstract T MakeRequest<T>( IRequestHandle<T> request )
            where T : IRequestable;

        public abstract IQueryResultsPage<IFanfic, IFanficRequestHandle> Search( LibrarySearch searchCriteria );

        public bool IsCachingObjectType( CacheableObjects objectType )
        {
            if ( objectType.HasMultipleFlagsSet() )
            {
                throw new ArgumentException( $"The provided {nameof( CacheableObjects )} must only have a single flag set!", nameof( objectType ) );
            }

            return _config.CachedObjects.HasFlag( objectType );
        }

        protected HtmlDocument GetWebPage( CacheableObjects objectType, string cacheHandle, string endpoint, bool ignoreCache, out Uri responseUrl )
        {
            HtmlDocument document;

            if ( !CacheableObjectsUtils.IsHtmlObject( objectType ) )
            {
                throw new ArgumentException( "Only HTML objects can be requested by this function", nameof( objectType ) );
            }

            if ( IsCachingObjectType( objectType ) && !ignoreCache )
            {
                string cacheFilename = GetCacheFileName( objectType, cacheHandle );
                if ( File.Exists( cacheFilename ) )
                {
                    document = new HtmlDocument();
                    document.Load( cacheFilename, Encoding.UTF8 );
                    responseUrl = new Uri( endpoint );
                    return document;
                }
            }

            document = HtmlUtils.GetWebPage( endpoint, out responseUrl );

            if ( IsCachingObjectType( objectType ) )
            {
                WriteToCache( objectType, cacheHandle, document );
            }

            return document;
        }

        string GetCacheFileName( CacheableObjects objectType, string cacheHandle )
        {
            return Path.Combine( _personalCacheDirectory, objectType.ToString(), cacheHandle + ".htm" );
        }

        void WriteToCache( CacheableObjects objectType, string cacheHandle, HtmlDocument document )
        {
            string cacheFilename = GetCacheFileName( objectType, cacheHandle );
            string directoryPath = Path.GetDirectoryName( cacheFilename );
            if ( directoryPath == null )
            {
                throw new ApplicationException();
            }

            Directory.CreateDirectory( directoryPath );

            using ( Stream cacheFileStream = File.OpenWrite( cacheFilename ) )
            {
                document.Save( cacheFileStream, Encoding.UTF8 );
            }
        }

        readonly LibrarySourceConfig _config;
        readonly string _personalCacheDirectory;
    }
}
