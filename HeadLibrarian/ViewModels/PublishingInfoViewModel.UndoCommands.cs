using System;

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

		class SetSenderUsernameUndoAction : BaseUndoAction
		{
			public SetSenderUsernameUndoAction( PublishingInfoViewModel viewModel, String oldUsername, String newUsername )
			{
				_viewModel = viewModel;
				_oldUsername = oldUsername;
				_newUsername = newUsername;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetSenderUsername( _oldUsername ) );
				_viewModel.InvokeSenderUsernameChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetSenderUsername( _newUsername ) );
				_viewModel.InvokeSenderUsernameChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly String _oldUsername;
			readonly String _newUsername;
		}

		class SetSenderPasswordUndoAction : BaseUndoAction
		{
			public SetSenderPasswordUndoAction( PublishingInfoViewModel viewModel, String oldPassword, String newPassword )
			{
				_viewModel = viewModel;
				_oldPassword = oldPassword;
				_newPassword = newPassword;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _viewModel._info.SetSenderPassword( _oldPassword ) );
				_viewModel.InvokeSenderPasswordChanged();
			}

			public override void Redo()
			{
				AssertModelSetFunction( _viewModel._info.SetSenderPassword( _newPassword ) );
				_viewModel.InvokeSenderPasswordChanged();
			}

			readonly PublishingInfoViewModel _viewModel;
			readonly String _oldPassword;
			readonly String _newPassword;
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
	}
}
