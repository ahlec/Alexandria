using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.Utils;

namespace Alexandria
{
	public abstract class LibrarySource
	{
		protected LibrarySource()
		{
			_objectsBeingCached = CacheableObjects.All;
			CacheDirectory = Path.Combine( "cache", GetType().Name );
			CacheLifetime = TimeSpan.FromDays( 1.0D );
		}

		public abstract T MakeRequest<T>( IRequestHandle<T> request ) where T : IRequestable;

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
					responseUrl = null;
					return document;
				}
			}

			HttpWebRequest request = (HttpWebRequest) WebRequest.Create( endpoint );
			request.Method = "GET";
			HttpWebResponse response = (HttpWebResponse) request.GetResponse();
			responseUrl = response.ResponseUri;
			
			if ( response.StatusCode != HttpStatusCode.OK )
			{
				throw new ApplicationException( $"The page {endpoint} resulted in a {response.StatusCode} error code." );
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
			const String OptionsOpenTagReplacement = "<" + OptionsHtmlTag + " ";
			const String OptionsCloseTagReplacement = OptionsHtmlTag + ">";
			text = text.Replace( "<option ", OptionsOpenTagReplacement ).Replace( "option>", OptionsCloseTagReplacement );

			document = new HtmlDocument();
			Byte[] bytes = Encoding.UTF8.GetBytes( text );
			using ( Stream textStream = new MemoryStream( bytes ) )
			{
				document.Load( textStream, Encoding.UTF8 );
			}
			if ( document.ParseErrors.Any() )
			{
				StringBuilder exceptionMessage = new StringBuilder( $"The page {endpoint} resulted in {document.ParseErrors.Count()} error(s)" );
				exceptionMessage.AppendLine();
				foreach ( HtmlParseError error in document.ParseErrors )
				{
					exceptionMessage.AppendLine( $"- Line {error.Line} Col {error.LinePosition}: {error.Reason} [{error.Code}]" );
				}
				throw new ApplicationException( exceptionMessage.ToString() );
			}

			if ( IsCachingObjectType( objectType ) )
			{
				WriteToCache( objectType, cacheHandle, text );
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

			return _objectsBeingCached.HasFlag( objectType );
		}

		public String CacheDirectory { get; private set; }

		public TimeSpan CacheLifetime { get; private set; }

		String GetCacheFileName( CacheableObjects objectType, String cacheHandle )
		{
			return Path.Combine( CacheDirectory, objectType.ToString(), cacheHandle + ".htm" );
		}

		void WriteToCache( CacheableObjects objectType, String cacheHandle, String text )
		{
			String cacheFilename = GetCacheFileName( objectType, cacheHandle );
			String directoryPath = Path.GetDirectoryName( cacheFilename );
			Directory.CreateDirectory( directoryPath );
			File.WriteAllText( cacheFilename, text );
		}

		#endregion Caching

		public const String OptionsHtmlTag = "my_option";

		CacheableObjects _objectsBeingCached = CacheableObjects.None;
	}
}
