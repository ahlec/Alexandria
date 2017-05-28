using System;
using Bibliothecary.Data;

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
			OnPropertyChanged( nameof( SenderUsername ) );
			OnPropertyChanged( nameof( SenderPassword ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public String SenderUsername
		{
			get => _info.SenderUsername;
			set
			{
				String oldUsername = SenderUsername;
				if ( _info.SetSenderUsername( value ) )
				{
					_projectViewModel.UndoStack.Push( new SetSenderUsernameUndoAction( this, oldUsername, value ) );
					InvokeSenderUsernameChanged();
				}
			}
		}

		void InvokeSenderUsernameChanged()
		{
			OnPropertyChanged( nameof( SenderUsername ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public String SenderPassword
		{
			get => _info.SenderPassword;
			set
			{
				String oldPassword = SenderPassword;
				if ( _info.SetSenderPassword( value ) )
				{
					_projectViewModel.UndoStack.Push( new SetSenderPasswordUndoAction( this, oldPassword, value ) );
					InvokeSenderPasswordChanged();
				}
			}
		}

		void InvokeSenderPasswordChanged()
		{
			OnPropertyChanged( nameof( SenderPassword ) );
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

		readonly ProjectViewModel _projectViewModel;
		readonly PublishingInfo _info;
	}
}
