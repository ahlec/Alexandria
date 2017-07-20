using System;
using System.Windows.Input;

namespace HeadLibrarian.ViewModels
{
	public sealed class IntroductionTabViewModel : BaseViewModel, ITabViewModel
	{
		public Boolean IsProject => false;

		public Boolean HasUnsavedChanges => false;

		public ICommand UndoCommand => null;

		public ICommand RedoCommand => null;
	}
}
