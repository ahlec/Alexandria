using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Bibliothecary.Core;
using Bibliothecary.Core.Publishing;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using HeadLibrarian.Dialogs;
using HeadLibrarian.WPF;
using DontPanicTumblrClient = DontPanic.TumblrSharp.Client.TumblrClient;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class PublishingInfoViewModel : BaseViewModel
	{
		public PublishingInfoViewModel( ProjectViewModel viewModel, PublishingInfo info )
		{
			_projectViewModel = viewModel;
			_info = info;
			AvailableTumblrBlogNames = GetTumblrBlogNames();
		}

		public Boolean UsesEmail
		{
			get => _info.UsesEmail;
			set
			{
				Boolean oldValue = UsesEmail;
				if ( _info.SetUsesEmail( value ) )
				{
					_projectViewModel.UndoStack.Push( new SetUsesEmailUndoAction( this, oldValue, value ) );
					InvokeUsesEmailChanged();
				}
			}
		}

		void InvokeUsesEmailChanged()
		{
			OnPropertyChanged( nameof( UsesEmail ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public String SenderEmail
		{
			get => _info.SenderEmail;
			set
			{
				String oldEmail = SenderEmail;
				if ( _info.SetSenderEmail( value ) )
				{
					_projectViewModel.UndoStack.Push( new SetSenderEmailUndoAction( this, oldEmail, value ) );
					InvokeSenderEmailChanged();
				}
			}
		}

		void InvokeSenderEmailChanged()
		{
			OnPropertyChanged( nameof( SenderEmail ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public String SenderHost
		{
			get => _info.SenderHost;
			set
			{
				String oldHost = SenderHost;
				if ( _info.SetSenderHost( value ) )
				{
					_projectViewModel.UndoStack.Push( new SetSenderHostUndoAction( this, oldHost, value ) );
					InvokeSenderHostChanged();
				}
			}
		}

		void InvokeSenderHostChanged()
		{
			OnPropertyChanged( nameof( SenderHost ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Int32 SenderPort
		{
			get => _info.SenderPort;
			set
			{
				Int32 oldPort = SenderPort;
				if ( _info.SetSenderPort( value ) )
				{
					_projectViewModel.UndoStack.Push( new SetSenderPortUndoAction( this, oldPort, value ) );
					InvokeSenderPortChanged();
				}
			}
		}

		void InvokeSenderPortChanged()
		{
			OnPropertyChanged( nameof( SenderPort ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean DoesSenderUseSsl
		{
			get => _info.DoesSenderUseSsl;
			set
			{
				Boolean oldValue = DoesSenderUseSsl;
				if ( _info.SetDoesSenderUseSsl( value ) )
				{
					_projectViewModel.UndoStack.Push( new SetDoesSenderUseSslUndoAction( this, oldValue, value ) );
					InvokeDoesSenderUseSslChanged();
				}
			}
		}

		void InvokeDoesSenderUseSslChanged()
		{
			OnPropertyChanged( nameof( DoesSenderUseSsl ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean DoesSenderRequireCredentials
		{
			get => _info.DoesSenderRequireCredentials;
			set
			{
				Boolean oldValue = DoesSenderRequireCredentials;
				if ( _info.SetDoesSenderRequireCredentials( value ) )
				{
					_projectViewModel.UndoStack.Push( new SetDoesSenderRequireCredentialsUndoAction( this, oldValue, value ) );
					InvokeDoesSenderRequireCredentialsChanged();
				}
			}
		}

		void InvokeDoesSenderRequireCredentialsChanged()
		{
			OnPropertyChanged( nameof( DoesSenderRequireCredentials ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public String RecipientEmail
		{
			get => _info.RecipientEmail;
			set
			{
				String oldEmail = RecipientEmail;
				if ( _info.SetRecipientEmail( value ) )
				{
					_projectViewModel.UndoStack.Push( new SetRecipientEmailUndoAction( this, oldEmail, value ) );
					InvokeRecipientEmailChanged();
				}
			}
		}

		void InvokeRecipientEmailChanged()
		{
			OnPropertyChanged( nameof( RecipientEmail ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean UsesTumblr
		{
			get => _info.UsesTumblr;
			set
			{
				Boolean oldValue = UsesTumblr;
				if ( _info.SetUsesTumblr( value ) )
				{
					_projectViewModel.UndoStack.Push( new SetUsesTumblrUndoAction( this, oldValue, value ) );
					InvokeUsesTumblrChanged();
				}
			}
		}

		void InvokeUsesTumblrChanged()
		{
			OnPropertyChanged( nameof( UsesTumblr ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public String TumblrConsumerKey
		{
			get => _info.TumblrConsumerKey;
			set
			{
				String oldValue = TumblrConsumerKey;
				if ( _info.SetTumblrConsumerKey( value ) )
				{
					String oldOauthToken = _info.TumblrOauthToken;
					String oldOauthSecret = _info.TumblrOauthSecret;
					_info.SetTumblrOauthToken( null );
					_info.SetTumblrOauthSecret( null );
					_projectViewModel.UndoStack.Push( new SetTumblrConsumerKeyUndoAction( this, oldValue, value, oldOauthToken, oldOauthSecret ) );
					InvokeTumblrConsumerKeyChanged();
				}
			}
		}

		void InvokeTumblrConsumerKeyChanged()
		{
			OnPropertyChanged( nameof( TumblrConsumerKey ) );
			OnPropertyChanged( nameof( CanAttemptTumblrAuthentication ) );
			OnPropertyChanged( nameof( IsAuthenticatedToTumblr ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public String TumblrConsumerSecret
		{
			get => _info.TumblrConsumerSecret;
			set
			{
				String oldValue = TumblrConsumerSecret;
				if ( _info.SetTumblrConsumerSecret( value ) )
				{
					String oldOauthToken = _info.TumblrOauthToken;
					String oldOauthSecret = _info.TumblrOauthSecret;
					_info.SetTumblrOauthToken( null );
					_info.SetTumblrOauthSecret( null );
					_projectViewModel.UndoStack.Push( new SetTumblrConsumerSecretUndoAction( this, oldValue, value, oldOauthToken, oldOauthSecret ) );
					InvokeTumblrConsumerSecretChanged();
				}
			}
		}

		void InvokeTumblrConsumerSecretChanged()
		{
			OnPropertyChanged( nameof( TumblrConsumerSecret ) );
			OnPropertyChanged( nameof( CanAttemptTumblrAuthentication ) );
			OnPropertyChanged( nameof( IsAuthenticatedToTumblr ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean CanAttemptTumblrAuthentication => !String.IsNullOrWhiteSpace( TumblrConsumerKey ) && !String.IsNullOrWhiteSpace( TumblrConsumerSecret );

		public Boolean IsAuthenticatedToTumblr => !String.IsNullOrEmpty( _info.TumblrOauthToken ) && !String.IsNullOrEmpty( _info.TumblrOauthSecret );

		public IEnumerable<String> AvailableTumblrBlogNames { get; private set; }

		void InvokeTumblrOauthChanged()
		{
			OnPropertyChanged( nameof( IsAuthenticatedToTumblr ) );
			OnPropertyChanged( nameof( AvailableTumblrBlogNames ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public String TumblrBlogName
		{
			get => _info.TumblrBlogName;
			set
			{
				String oldValue = TumblrBlogName;
				if ( _info.SetTumblrBlogName( value ) )
				{
					_projectViewModel.UndoStack.Push( new SetTumblrBlogNameUndoAction( this, oldValue, value ) );
					InvokeTumblrBlogNameChanged();
				}
			}
		}

		void InvokeTumblrBlogNameChanged()
		{
			OnPropertyChanged( nameof( TumblrBlogName ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public ICommand ProvideEmailCredentialsCommand => ( _provideEmailCredentialsCommand ?? ( _provideEmailCredentialsCommand = new Command( null, CommandProvideEmailCredentials ) ) );

		public ICommand SendTestEmailCommand => ( _sendTestEmailCommand ?? ( _sendTestEmailCommand = new Command( null, CommandSendTestEmail ) ) );

		public ICommand AuthenticateTumblrCommand => ( _authenticateTumblrCommand ?? ( _authenticateTumblrCommand = new Command( null, CommandAuthenticateTumblr ) ) );

		void CommandProvideEmailCredentials( Object o )
		{
			ProvideEmailCredentialsDialog dialog = new ProvideEmailCredentialsDialog( _info.SenderUsername );
			dialog.ShowDialog();

			if ( dialog.DialogResult != true )
			{
				return;
			}

			String oldUsername = _info.SenderUsername;
			SecureString oldPassword = _info.SenderPassword;
			_info.SetSenderUsername( dialog.Username );
			_info.SetSenderPassword( dialog.Password );
			_projectViewModel.UndoStack.Push( new SetSenderLoginCredentialsUndoAction( this, oldUsername, _info.SenderUsername,
				oldPassword, _info.SenderPassword ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		void CommandSendTestEmail( Object o )
		{
			EmailClient emailClient = new EmailClient
			{
				Host = SenderHost,
				Port = SenderPort,
				EnableSsl = DoesSenderUseSsl,
				FromEmail = SenderEmail,
				ToEmail = RecipientEmail
			};
			if ( DoesSenderRequireCredentials )
			{
				emailClient.SetCredentials( _info.SenderUsername, _info.SenderPassword );
			}

			try
			{
				emailClient.SendTestEmail( _projectViewModel.Project );
			}
			catch ( Exception ex )
			{
				MessageBox.Show( ex.Message, Constants.BibliothecaryName, MessageBoxButton.OK, MessageBoxImage.Error );
			}
		}

		void CommandAuthenticateTumblr( Object o )
		{
			if ( !UsesTumblr || !CanAttemptTumblrAuthentication )
			{
				return;
			}

			AuthenticateTumblrDialog dialog = new AuthenticateTumblrDialog( TumblrConsumerKey, TumblrConsumerSecret );
			dialog.ShowDialog();

			if ( dialog.DialogResult != true )
			{
				return;
			}

			String oldOauthToken = _info.TumblrOauthToken;
			String oldOauthSecret = _info.TumblrOauthSecret;
			Boolean didChangeToken = _info.SetTumblrOauthToken( dialog.OauthToken );
			Boolean didChangeSecret = _info.SetTumblrOauthSecret( dialog.OauthSecret );
			if ( !didChangeToken && !didChangeSecret )
			{
				return;
			}

			IEnumerable<String> oldBlogNames = AvailableTumblrBlogNames;
			AvailableTumblrBlogNames = GetTumblrBlogNames();

			_projectViewModel.UndoStack.Push( new SetTumblrOauthUndoAction( this, oldOauthToken, oldOauthSecret,
				_info.TumblrOauthToken, _info.TumblrOauthSecret, oldBlogNames, AvailableTumblrBlogNames ) );
			InvokeTumblrOauthChanged();
		}

		IEnumerable<String> GetTumblrBlogNames()
		{
			if ( !UsesTumblr || !IsAuthenticatedToTumblr )
			{
				return new List<String>();
			}

			DontPanicTumblrClient client = new DontPanicTumblrClient( new HmacSha1HashProvider(), TumblrConsumerKey, TumblrConsumerSecret,
				new Token( _info.TumblrOauthToken, _info.TumblrOauthSecret ) );
			Task<UserInfo> getUserInfoTask = client.GetUserInfoAsync();
			getUserInfoTask.Wait();
			List<String> blogNames = getUserInfoTask.Result.Blogs.Select( blog => blog.Name ).ToList();
			return blogNames;
		}

		ICommand _provideEmailCredentialsCommand;
		ICommand _sendTestEmailCommand;
		ICommand _authenticateTumblrCommand;

		readonly ProjectViewModel _projectViewModel;
		readonly PublishingInfo _info;
	}
}
