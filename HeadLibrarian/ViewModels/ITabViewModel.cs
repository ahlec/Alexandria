using System;
using System.Windows.Input;

namespace HeadLibrarian.ViewModels
{
	public interface ITabViewModel
	{
		Boolean IsProject { get; }

		Boolean HasUnsavedChanges { get; }

		ICommand UndoCommand { get; }

		ICommand RedoCommand { get; }
	}
}
