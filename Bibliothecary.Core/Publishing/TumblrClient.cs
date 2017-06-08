using System;
using System.Diagnostics;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.OAuth;

namespace Bibliothecary.Core.Publishing
{
	public sealed class TumblrClient : IDisposable
	{
		~TumblrClient()
		{
			Dispose();
		}

		public void Dispose()
		{
			if ( !_isDisposed )
			{
				if ( _isListeningToWebServer )
				{
					//InternalWebListener.DeregisterAsCurrentTumblrClient( this );
				}

				_isDisposed = true;
			}
		}

		public const String CallbackUrl = "http://localhost:1717/";

		public String ConsumerKey { get; set; }

		public String ConsumerSecret { get; set; }

		public String OauthToken { get; set; }

		public String OauthTokenSecret { get; set; }

		public String BlogName { get; set; }

		public async void Authenticate()
		{
			if ( String.IsNullOrEmpty( ConsumerKey ) )
			{
				throw new InvalidOperationException( $"{nameof( ConsumerKey )} must not be empty." );
			}
			if ( String.IsNullOrEmpty( ConsumerSecret ) )
			{
				throw new InvalidOperationException( $"{nameof( ConsumerSecret )} must not be empty." );
			}

			_oauthClient = new OAuthClient( new HmacSha1HashProvider(), ConsumerKey, ConsumerSecret );
			_requestToken = await _oauthClient.GetRequestTokenAsync( CallbackUrl );

			//InternalWebListener.RegisterAsCurrentTumblrClient( this );
			_isListeningToWebServer = true;

			String authenticateUrl = String.Concat( "https://www.tumblr.com/oauth/authorize?oauth_token=", _requestToken.Key );
			Process.Start( authenticateUrl );
		}

		internal async void OnReceivedOAuthCallback( String verifierUrl )
		{
			Token accessToken = await _oauthClient.GetAccessTokenAsync( _requestToken, verifierUrl );
			accessToken.ToString();
		}

		Boolean _isDisposed;
		Boolean _isListeningToWebServer;
		OAuthClient _oauthClient;
		Token _requestToken;
	}
}
