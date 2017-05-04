using System;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class LibrarySearchViewModel
	{
		class SetTitleUndoAction : IUndoRedoAction
		{
			public SetTitleUndoAction( LibrarySearchViewModel viewModel, String oldName, String newName )
			{
				_viewModel = viewModel;
				_oldName = oldName;
				_newName = newName;
			}

			public void Undo()
			{
				_viewModel._search.Title = _oldName;
				_viewModel.InvokeTitleChanged();
			}

			public void Redo()
			{
				_viewModel._search.Title = _newName;
				_viewModel.InvokeTitleChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly String _oldName;
			readonly String _newName;
		}
	}
}
