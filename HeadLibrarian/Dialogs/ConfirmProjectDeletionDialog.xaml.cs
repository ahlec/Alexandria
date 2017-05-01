using System;
using System.Windows.Input;
using HeadLibrarian.ViewModels;
using HeadLibrarian.WPF;

namespace HeadLibrarian.Dialogs
{
	public partial class ConfirmProjectDeletionDialog
	{
		public ConfirmProjectDeletionDialog( ProjectViewModel project )
		{
			if ( project == null )
			{
				throw new ArgumentNullException();
			}
			ProjectName = project.Name;

			YesCommand = new Command( null, CommandYes );
			NoCommand = new Command( null, CommandNo );

			InitializeComponent();
		}

		public String ProjectName { get; }

		public ICommand YesCommand { get; }

		public ICommand NoCommand { get; }

		void CommandYes( Object o )
		{
			if ( DialogResult != null )
			{
				return;
			}

			DialogResult = true;
			IsEnabled = false;
		}

		void CommandNo( Object o )
		{
			if ( DialogResult != null )
			{
				return;
			}

			DialogResult = false;
			IsEnabled = false;
		}
	}
}
