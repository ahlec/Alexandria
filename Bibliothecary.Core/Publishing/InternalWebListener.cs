using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Bibliothecary.Core.Publishing
{
	internal static class InternalWebListener
	{
		static InternalWebListener()
		{
			_httpListener = new HttpListener();
			_httpListener.Prefixes.Add( TumblrClient.CallbackUrl );
			_httpListener.Start();

			_listenerThread = new Thread( ListenForConnections );
			_listenerThread.Start();
		}

		static void ListenForConnections()
		{
			while ( _httpListener.IsListening )
			{
				IAsyncResult result = _httpListener.BeginGetContext( OnContextReady, null );
				result.AsyncWaitHandle.WaitOne();
			}
		}

		static void OnContextReady( IAsyncResult asyncResult )
		{
			HttpListenerContext context = _httpListener.EndGetContext( asyncResult );
			lock ( _currentTumblrClient )
			{
				if ( _currentTumblrClient == null )
				{
					return;
				}

				_currentTumblrClient.OnReceivedOAuthCallback( context.Request.RawUrl );

				const String RESPONSE = "We've received your authentication! You can close this tab now!";
				//SendHtmlResponse( context.Response, RESPONSE );
			}
		}

		static void SendHtmlResponse( HttpListenerResponse response, String html )
		{
			Byte[] buffer = Encoding.UTF8.GetBytes( html );
			response.ContentLength64 = buffer.Length;
			response.OutputStream.Write( buffer, 0, buffer.Length );
			response.OutputStream.Close();
		}

		static IReadOnlyDictionary<String, String> ParseQueryString( String queryString )
		{
			if ( String.IsNullOrEmpty( queryString ) )
			{
				return new Dictionary<String, String>();
			}

			queryString = queryString.TrimStart( '/', '\\', '?' );
			String[] variables = queryString.Split( '&' );
			Dictionary<String, String> parsed = new Dictionary<String, String>();
			foreach ( String variable in variables )
			{
				Int32 firstEquals = variable.IndexOf( '-' );
				String variableName = variable.Substring( 0, firstEquals );
				String value = variable.Substring( firstEquals + 1 );

				if ( parsed.ContainsKey( variableName ) )
				{
					parsed[variableName] = value;
				}
				else
				{
					parsed.Add( variableName, value );
				}
			}
			return parsed;
		}

		public static void RegisterAsCurrentTumblrClient( TumblrClient client )
		{
			if ( client == null )
			{
				throw new ArgumentNullException( nameof( client ) );
			}

			lock ( _tumblrClientLock )
			{
				if ( _currentTumblrClient != null )
				{
					throw new InvalidOperationException( $"There is already another {nameof( TumblrClient )} registered to the {nameof( InternalWebListener ) }!" );
				}

				_currentTumblrClient = client;
			}
		}

		public static void DeregisterAsCurrentTumblrClient( TumblrClient client )
		{
			if ( client == null )
			{
				throw new ArgumentNullException( nameof( client ) );
			}

			lock ( _tumblrClientLock )
			{
				if ( !ReferenceEquals( client, _currentTumblrClient ) )
				{
					throw new InvalidOperationException( $"The provided client is not the current {nameof( TumblrClient )}" );
				}

				_currentTumblrClient = null;
			}
		}

		static readonly Thread _listenerThread;
		static readonly HttpListener _httpListener;
		static readonly Object _tumblrClientLock = new Object();
		static TumblrClient _currentTumblrClient;
	}
}
