using System;
using System.Security;
using System.Windows;
using System.Windows.Input;
using Bibliothecary.Core;
using Bibliothecary.Core.Publishing;
using HeadLibrarian.Dialogs;
using HeadLibrarian.WPF;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class PublishingInfoViewModel : BaseViewModel
	{
		public PublishingInfoViewModel( ProjectViewModel viewModel, PublishingInfo info )
		{
			_projectViewModel = viewModel;
			_info = info;
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
			TumblrClient tumblrClient = new TumblrClient
			{
				ConsumerKey = "",
				ConsumerSecret = ""
			};

			tumblrClient.Authenticate();
		}

		ICommand _provideEmailCredentialsCommand;
		ICommand _sendTestEmailCommand;
		ICommand _authenticateTumblrCommand;

		readonly ProjectViewModel _projectViewModel;
		readonly PublishingInfo _info;
	}
}
