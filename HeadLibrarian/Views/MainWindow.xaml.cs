using System;
using System.ComponentModel;
using HeadLibrarian.ViewModels;

namespace HeadLibrarian.Views
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		void OnClosing( Object sender, CancelEventArgs e )
		{
			e.Cancel = ( (MainWindowViewModel) DataContext ).ShouldPreventWindowClosing( this );
		}
	}
}
