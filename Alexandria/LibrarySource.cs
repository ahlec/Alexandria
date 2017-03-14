using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Alexandria.Model;

namespace Alexandria
{
	public abstract class LibrarySource
	{
		protected enum WebPageParseResult
		{
			UnknownError = -1,

			Success = 0,

			FileNotFound = 1,
		}

		protected LibrarySource()
		{
			IsCaching = true;
			CacheDirectory = Path.Combine( "cache", GetType().Name );
			CacheLifetime = TimeSpan.FromDays( 1.0D );
		}

		public abstract IAuthor GetAuthor( String name );

		public abstract IFanfic GetFanfic( String handle );

		protected WebPageParseResult GetWebPage( String cacheHandle, String endpoint, out HtmlDocument document )
		{
			if ( IsCaching )
			{
				String cacheFilename = Path.Combine( CacheDirectory, cacheHandle + ".htm" );
				if ( File.Exists( cacheFilename ) )
				{
					document = new HtmlDocument();
					document.Load( cacheFilename );
					return WebPageParseResult.Success;
				}
			}

			HttpWebRequest request = (HttpWebRequest) WebRequest.Create( endpoint );
			request.Method = "GET";
			HttpWebResponse response = (HttpWebResponse) request.GetResponse();
			
			switch ( response.StatusCode )
			{
				case HttpStatusCode.NotFound:
					{
						document = null;
						return WebPageParseResult.FileNotFound;
					}
				case HttpStatusCode.OK:
					{
						break;
					}
				default:
					{
						document = null;
						return WebPageParseResult.UnknownError;
					}
			}

			String text;
			using ( Stream responseStream = response.GetResponseStream() )
			using ( StreamReader reader = new StreamReader( responseStream ) )
			{
				text = reader.ReadToEnd();
			}

			document = new HtmlDocument();
			document.LoadHtml( text );
			if ( document.ParseErrors.Any() )
			{
				return WebPageParseResult.UnknownError;
			}

			if ( IsCaching )
			{
				WriteToCache( cacheHandle, text );
			}

			return WebPageParseResult.Success;
		}

		#region Caching

		public Boolean IsCaching { get; private set; }

		public String CacheDirectory { get; private set; }

		public TimeSpan CacheLifetime { get; private set; }

		void WriteToCache( String cacheHandle, String text )
		{
			Directory.CreateDirectory( CacheDirectory );
			String cacheFilename = Path.Combine( CacheDirectory, cacheHandle + ".htm" );
			File.WriteAllText( cacheFilename, text );
		}

		#endregion Caching
	}
}
