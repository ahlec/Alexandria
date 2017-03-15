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
			Success = 0,

			FileNotFound = 1,
			UnknownWebError = 99,

			UnknownDeserializationError = 199
		}

		protected LibrarySource()
		{
			IsCachingWebResults = true;
			CacheDirectory = Path.Combine( "cache", GetType().Name );
			CacheLifetime = TimeSpan.FromDays( 1.0D );
		}

		public abstract IAuthor GetAuthor( String name );

		public abstract IFanfic GetFanfic( String handle );

		protected WebPageParseResult GetWebPage( String cacheHandle, String endpoint, Boolean ignoreCache, out Uri responseUrl, out HtmlDocument document )
		{
			if ( IsCachingWebResults && !ignoreCache )
			{
				String cacheFilename = Path.Combine( CacheDirectory, cacheHandle + ".htm" );
				if ( File.Exists( cacheFilename ) )
				{
					document = new HtmlDocument();
					document.Load( cacheFilename );
					responseUrl = null;
					return WebPageParseResult.Success;
				}
			}

			HttpWebRequest request = (HttpWebRequest) WebRequest.Create( endpoint );
			request.Method = "GET";
			HttpWebResponse response = (HttpWebResponse) request.GetResponse();
			responseUrl = response.ResponseUri;
			
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
						return WebPageParseResult.UnknownWebError;
					}
			}

			String text;
			using ( Stream responseStream = response.GetResponseStream() )
			using ( StreamReader reader = new StreamReader( responseStream ) )
			{
				text = reader.ReadToEnd();
			}

			// There's some super-bizzaro thing with HtmlAgilityPack where it doesn't recognise </option>. I know that sounds
			// like I'm making bullshit up to cover my arse because "I'm not using it right" but here
			// http://stackoverflow.com/questions/293342/htmlagilitypack-drops-option-end-tags
			// Just replacing the tag name altogether to something else, it doesn't matter, we're not going to pay attention
			// to it right now.
			text = text.Replace( "<option ", "<my_option " ).Replace( "option>", "my_option>" );

			document = new HtmlDocument();
			document.LoadHtml( text );
			if ( document.ParseErrors.Any() )
			{
				return WebPageParseResult.UnknownDeserializationError;
			}

			if ( IsCachingWebResults )
			{
				WriteToCache( cacheHandle, text );
			}

			return WebPageParseResult.Success;
		}

		#region Caching

		public Boolean IsCachingWebResults { get; private set; }

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
