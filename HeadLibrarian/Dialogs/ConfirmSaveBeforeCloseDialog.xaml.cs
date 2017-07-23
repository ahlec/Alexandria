using System;
using System.Windows.Input;
using HeadLibrarian.WPF;

namespace HeadLibrarian.Dialogs
{
	public enum ConfirmSaveBeforeCloseResults
	{
		Cancel,
		SaveThenClose,
		IgnoreChangesAndClose
	}

	public partial class ConfirmSaveBeforeCloseDialog
	{
		public ConfirmSaveBeforeCloseDialog()
		{
			YesCommand = new Command( null, CommandYes );
			NoCommand = new Command( null, CommandNo );
			CancelCommand = new Command( null, CommandCancel );

			InitializeComponent();
		}

		public ConfirmSaveBeforeCloseResults Result { get; private set; } = ConfirmSaveBeforeCloseResults.Cancel;

		public ICommand YesCommand { get; }

		public ICommand NoCommand { get; }

		public ICommand CancelCommand { get; }

		void CommandYes( Object o )
		{
			Result = ConfirmSaveBeforeCloseResults.SaveThenClose;
			DialogResult = true;
			IsEnabled = false;
			Close();
		}

		void CommandNo( Object o )
		{
			Result = ConfirmSaveBeforeCloseResults.IgnoreChangesAndClose;
			DialogResult = true;
			IsEnabled = false;
			Close();
		}

		void CommandCancel( Object o )
		{
			Result = ConfirmSaveBeforeCloseResults.Cancel;
			DialogResult = true;
			IsEnabled = false;
			Close();
		}
	}
}
