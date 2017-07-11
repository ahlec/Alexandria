using System;
using System.IO;
using System.Text;
using Alexandria.Model;
using HtmlAgilityPack;
using Alexandria.RequestHandles;
using Alexandria.Searching;
using Alexandria.Utils;

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

		public abstract String SourceHandle { get; }

		public abstract T MakeRequest<T>( IRequestHandle<T> request ) where T : IRequestable;

		public abstract IQueryResultsPage<IFanfic, IFanficRequestHandle> Search( LibrarySearch searchCriteria );

		protected HtmlDocument GetWebPage( CacheableObjects objectType, String cacheHandle, String endpoint, Boolean ignoreCache, out Uri responseUrl )
		{
			HtmlDocument document;

			if ( !CacheableObjectsUtils.IsHtmlObject( objectType ) )
			{
				throw new ArgumentException( "Only HTML objects can be requested by this function", nameof( objectType ) );
			}

			if ( IsCachingObjectType( objectType ) && !ignoreCache )
			{
				String cacheFilename = GetCacheFileName( objectType, cacheHandle );
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

		#region Caching

		public Boolean IsCachingObjectType( CacheableObjects objectType )
		{
			if ( objectType.HasMultipleFlagsSet() )
			{
				throw new ArgumentException( $"The provided {nameof( CacheableObjects )} must only have a single flag set!", nameof( objectType ) );
			}

			return _config.CachedObjects.HasFlag( objectType );
		}

		String GetCacheFileName( CacheableObjects objectType, String cacheHandle )
		{
			return Path.Combine( _personalCacheDirectory, objectType.ToString(), cacheHandle + ".htm" );
		}

		void WriteToCache( CacheableObjects objectType, String cacheHandle, HtmlDocument document )
		{
			String cacheFilename = GetCacheFileName( objectType, cacheHandle );
			String directoryPath = Path.GetDirectoryName( cacheFilename );
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

		#endregion Caching

		readonly LibrarySourceConfig _config;
		readonly String _personalCacheDirectory;
	}
}
