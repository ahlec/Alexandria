using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.OAuth;
using HeadLibrarian.WPF;

namespace HeadLibrarian.Dialogs
{
	public partial class AuthenticateTumblrDialog
	{
		public AuthenticateTumblrDialog( String consumerKey, String consumerSecret )
		{
			if ( String.IsNullOrEmpty( consumerKey ) )
			{
				throw new ArgumentNullException( nameof( consumerKey ) );
			}

			if ( String.IsNullOrEmpty( consumerSecret ) )
			{
				throw new ArgumentNullException( nameof( consumerSecret ) );
			}

			_consumerKey = consumerKey;
			_consumerSecret = consumerSecret;

			AuthenticateCommand = new Command( null, CommandAuthenticate );
			CancelCommand = new Command( null, CommandCancel );

			InitializeComponent();
		}

		public Int32 CallbackPort
		{
			get => (Int32) GetValue( CallbackPortProperty );
			set => SetValue( CallbackPortProperty, value );
		}

		public ICommand AuthenticateCommand { get; }

		public ICommand CancelCommand { get; }

		void CommandAuthenticate( Object o )
		{
			Int32 callbackPort = CallbackPort;
			if ( callbackPort < MinPortNumber || callbackPort > MaxPortNumber )
			{
				return;
			}

			lock ( _authenticationThreadLock )
			{
				if ( _authenticationThread != null )
				{
					return;
				}

				_mainDispatcher = Dispatcher.CurrentDispatcher;
				_authenticationThread = new Thread( PerformAuthentication );
				_authenticationThread.Start( callbackPort );
			}
		}

		void CommandCancel( Object o )
		{
			IsEnabled = false;
			DialogResult = false;
		}

		async void PerformAuthentication( Object callbackPort )
		{
			try
			{
				using ( HttpListener listener = new HttpListener() )
				{
					String callbackUrl = String.Concat( CallbackPrefix, callbackPort, "/" );
					listener.Prefixes.Add( callbackUrl );

					OAuthClient oauthClient = new OAuthClient( new HmacSha1HashProvider(), _consumerKey, _consumerSecret );
					Token requestToken = await oauthClient.GetRequestTokenAsync( callbackUrl );

					listener.Start();

					String authenticateUrl = String.Concat( "https://www.tumblr.com/oauth/authorize?oauth_token=", requestToken.Key );
					Process.Start( authenticateUrl );

					HttpListenerContext context = listener.GetContext();

					Token accessToken = await oauthClient.GetAccessTokenAsync( requestToken, context.Request.RawUrl );
					OauthToken = accessToken.Key;
					OauthSecret = accessToken.Secret;

					lock ( _authenticationThreadLock )
					{
						_mainDispatcher.Invoke( () =>
						{
							IsEnabled = false;
							DialogResult = true;
						} );
					}
				}
			}
			finally
			{
				lock ( _authenticationThreadLock )
				{
					_authenticationThread = null;
				}
			}
		}

		public String OauthToken { get; private set; }

		public String OauthSecret { get; private set; }

		public static readonly DependencyProperty CallbackPortProperty = DependencyProperty.Register( "CallbackPort", typeof( Int32 ),
			typeof( AuthenticateTumblrDialog ), new PropertyMetadata( DefaultPortNumber ) );

		public const String CallbackPrefix = "http://localhost:";
		public const Int32 MinPortNumber = 1000;
		public const Int32 MaxPortNumber = UInt16.MaxValue;
		public const Int32 DefaultPortNumber = 1717;

		readonly String _consumerKey;
		readonly String _consumerSecret;
		readonly Object _authenticationThreadLock = new Object();
		Thread _authenticationThread;
		Dispatcher _mainDispatcher;
	}
}
