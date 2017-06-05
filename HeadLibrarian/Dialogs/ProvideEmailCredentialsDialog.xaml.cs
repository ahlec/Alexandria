using System;
using System.Security;
using System.Windows.Input;
using HeadLibrarian.WPF;

namespace HeadLibrarian.Dialogs
{
	public partial class ProvideEmailCredentialsDialog
	{
		public ProvideEmailCredentialsDialog( String originalUsername )
		{
			InitializeComponent();
			Username = originalUsername;
		}

		public String Username
		{
			get => UsernameTextBox.Text;
			private set => UsernameTextBox.Text = value;
		}

		public SecureString Password => PasswordBox.SecurePassword;

		public ICommand OkCommand => ( _okCommand ?? ( _okCommand = new Command( null, CommandOk ) ) );

		public ICommand CancelCommand => ( _cancelCommand ?? ( _cancelCommand = new Command( null, CommandCancel ) ) );

		void CommandOk( Object o )
		{
			IsEnabled = false;
			DialogResult = true;
		}

		void CommandCancel( Object o )
		{
			IsEnabled = false;
			DialogResult = false;
		}

		ICommand _okCommand;
		ICommand _cancelCommand;
	}
}
