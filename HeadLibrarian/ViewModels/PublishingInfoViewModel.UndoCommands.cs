using System;
using System.Security;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class PublishingInfoViewModel
	{
		class SetUsesEmailUndoAction : BaseUndoAction
		{
			public SetUsesEmailUndoAction( PublishingInfoViewModel viewModel, Boolean oldValue, Boolean newValue )
			{
				_viewModel = viewModel;
				_oldValue = oldValue;
				_newValue = newValue;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetUsesEmail( _oldValue ) );
				_viewModel.InvokeUsesEmailChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetUsesEmail( _newValue ) );
				_viewModel.InvokeUsesEmailChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly Boolean _oldValue;
			readonly Boolean _newValue;
		}

		class SetSenderEmailUndoAction : BaseUndoAction
		{
			public SetSenderEmailUndoAction( PublishingInfoViewModel viewModel, String oldEmail, String newEmail )
			{
				_viewModel = viewModel;
				_oldEmail = oldEmail;
				_newEmail = newEmail;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetSenderEmail( _oldEmail ) );
				_viewModel.InvokeSenderEmailChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetSenderEmail( _newEmail ) );
				_viewModel.InvokeSenderEmailChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly String _oldEmail;
			readonly String _newEmail;
		}

		class SetSenderHostUndoAction : BaseUndoAction
		{
			public SetSenderHostUndoAction( PublishingInfoViewModel viewModel, String oldHost, String newHost )
			{
				_viewModel = viewModel;
				_oldHost = oldHost;
				_newHost = newHost;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetSenderHost( _oldHost ) );
				_viewModel.InvokeSenderHostChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetSenderHost( _newHost ) );
				_viewModel.InvokeSenderHostChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly String _oldHost;
			readonly String _newHost;
		}

		class SetSenderPortUndoAction : BaseUndoAction
		{
			public SetSenderPortUndoAction( PublishingInfoViewModel viewModel, Int32 oldPort, Int32 newPort )
			{
				_viewModel = viewModel;
				_oldPort = oldPort;
				_newPort = newPort;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetSenderPort( _oldPort ) );
				_viewModel.InvokeSenderPortChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetSenderPort( _newPort ) );
				_viewModel.InvokeSenderPortChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly Int32 _oldPort;
			readonly Int32 _newPort;
		}

		class SetDoesSenderUseSslUndoAction : BaseUndoAction
		{
			public SetDoesSenderUseSslUndoAction( PublishingInfoViewModel viewModel, Boolean oldValue, Boolean newValue )
			{
				_viewModel = viewModel;
				_oldValue = oldValue;
				_newValue = newValue;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetDoesSenderUseSsl( _oldValue ) );
				_viewModel.InvokeDoesSenderUseSslChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetDoesSenderUseSsl( _newValue ) );
				_viewModel.InvokeDoesSenderUseSslChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly Boolean _oldValue;
			readonly Boolean _newValue;
		}

		class SetDoesSenderRequireCredentialsUndoAction : BaseUndoAction
		{
			public SetDoesSenderRequireCredentialsUndoAction( PublishingInfoViewModel viewModel, Boolean oldValue, Boolean newValue )
			{
				_viewModel = viewModel;
				_oldValue = oldValue;
				_newValue = newValue;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetDoesSenderRequireCredentials( _oldValue ) );
				_viewModel.InvokeDoesSenderRequireCredentialsChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetDoesSenderRequireCredentials( _newValue ) );
				_viewModel.InvokeDoesSenderRequireCredentialsChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly Boolean _oldValue;
			readonly Boolean _newValue;
		}

		class SetSenderLoginCredentialsUndoAction : BaseUndoAction
		{
			public SetSenderLoginCredentialsUndoAction( PublishingInfoViewModel viewModel, String oldUsername, String newUsername,
				SecureString oldPassword, SecureString newPassword )
			{
				_viewModel = viewModel;
				_oldUsername = oldUsername;
				_newUsername = newUsername;
				_oldPassword = oldPassword;
				_newPassword = newPassword;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetSenderUsername( _oldUsername ) );
				AssertModelSetFunction( _viewModel._info.SetSenderPassword( _oldPassword ) );
				_viewModel._projectViewModel.HasChangedPublishingSenderEmailCredentials = false;
				_viewModel._projectViewModel.RefreshHasSavedChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetSenderUsername( _newUsername ) );
				AssertModelSetFunction( _viewModel._info.SetSenderPassword( _newPassword ) );
				_viewModel._projectViewModel.HasChangedPublishingSenderEmailCredentials = true;
				_viewModel._projectViewModel.RefreshHasSavedChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly String _oldUsername;
			readonly String _newUsername;
			readonly SecureString _oldPassword;
			readonly SecureString _newPassword;
		}

		class SetRecipientEmailUndoAction : BaseUndoAction
		{
			public SetRecipientEmailUndoAction( PublishingInfoViewModel viewModel, String oldEmail, String newEmail )
			{
				_viewModel = viewModel;
				_oldEmail = oldEmail;
				_newEmail = newEmail;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetRecipientEmail( _oldEmail ) );
				_viewModel.InvokeRecipientEmailChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetRecipientEmail( _newEmail ) );
				_viewModel.InvokeRecipientEmailChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly String _oldEmail;
			readonly String _newEmail;
		}

		class SetUsesTumblrUndoAction : BaseUndoAction
		{
			public SetUsesTumblrUndoAction( PublishingInfoViewModel viewModel, Boolean oldValue, Boolean newValue )
			{
				_viewModel = viewModel;
				_oldValue = oldValue;
				_newValue = newValue;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetUsesTumblr( _oldValue ) );
				_viewModel.InvokeUsesTumblrChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetUsesTumblr( _newValue ) );
				_viewModel.InvokeUsesTumblrChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly Boolean _oldValue;
			readonly Boolean _newValue;
		}

		class SetTumblrConsumerKeyUndoAction : BaseUndoAction
		{
			public SetTumblrConsumerKeyUndoAction( PublishingInfoViewModel viewModel, String oldValue, String newValue, String oldOauthToken, String oldOauthSecret )
			{
				_viewModel = viewModel;
				_oldValue = oldValue;
				_newValue = newValue;
				_oldOauthToken = oldOauthToken;
				_oldOauthSecret = oldOauthSecret;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetTumblrConsumerKey( _oldValue ) );
				_viewModel._info.SetTumblrOauthToken( _oldOauthToken );
				_viewModel._info.SetTumblrOauthSecret( _oldOauthSecret );
				_viewModel.InvokeTumblrConsumerKeyChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetTumblrConsumerKey( _newValue ) );
				_viewModel._info.SetTumblrOauthToken( null );
				_viewModel._info.SetTumblrOauthSecret( null );
				_viewModel.InvokeTumblrConsumerKeyChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly String _oldValue;
			readonly String _newValue;
			readonly String _oldOauthToken;
			readonly String _oldOauthSecret;
		}

		class SetTumblrConsumerSecretUndoAction : BaseUndoAction
		{
			public SetTumblrConsumerSecretUndoAction( PublishingInfoViewModel viewModel, String oldValue, String newValue, String oldOauthToken, String oldOauthSecret )
			{
				_viewModel = viewModel;
				_oldValue = oldValue;
				_newValue = newValue;
				_oldOauthToken = oldOauthToken;
				_oldOauthSecret = oldOauthSecret;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetTumblrConsumerSecret( _oldValue ) );
				_viewModel._info.SetTumblrOauthToken( _oldOauthToken );
				_viewModel._info.SetTumblrOauthSecret( _oldOauthSecret );
				_viewModel.InvokeTumblrConsumerSecretChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetTumblrConsumerSecret( _newValue ) );
				_viewModel._info.SetTumblrOauthToken( null );
				_viewModel._info.SetTumblrOauthSecret( null );
				_viewModel.InvokeTumblrConsumerSecretChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly String _oldValue;
			readonly String _newValue;
			readonly String _oldOauthToken;
			readonly String _oldOauthSecret;
		}

		class SetTumblrOauthUndoAction : BaseUndoAction
		{
			public SetTumblrOauthUndoAction( PublishingInfoViewModel viewModel, String oldOauthToken, String oldOauthSecret, String newOauthToken, String newOauthSecret )
			{
				_viewModel = viewModel;
				_oldOauthToken = oldOauthToken;
				_oldOauthSecret = oldOauthSecret;
				_newOauthToken = newOauthToken;
				_newOauthSecret = newOauthSecret;
			}

			public override void Undo()
			{
				_viewModel._info.SetTumblrOauthToken( _oldOauthToken );
				_viewModel._info.SetTumblrOauthSecret( _oldOauthSecret );
				_viewModel.InvokeTumblrOauthChanged();
			}

			public override void Redo()
			{
				_viewModel._info.SetTumblrOauthToken( _newOauthToken );
				_viewModel._info.SetTumblrOauthSecret( _newOauthSecret );
				_viewModel.InvokeTumblrOauthChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly String _oldOauthToken;
			readonly String _oldOauthSecret;
			readonly String _newOauthToken;
			readonly String _newOauthSecret;
		}

		class SetTumblrBlogNameUndoAction : BaseUndoAction
		{
			public SetTumblrBlogNameUndoAction( PublishingInfoViewModel viewModel, String oldValue, String newValue )
			{
				_viewModel = viewModel;
				_oldValue = oldValue;
				_newValue = newValue;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetTumblrBlogName( _oldValue ) );
				_viewModel.InvokeTumblrBlogNameChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetTumblrBlogName( _newValue ) );
				_viewModel.InvokeTumblrBlogNameChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly String _oldValue;
			readonly String _newValue;
		}
	}
}
