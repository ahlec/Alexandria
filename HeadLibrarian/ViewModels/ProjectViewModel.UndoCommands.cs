using System;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class ProjectViewModel
	{
		class SetNameUndoAction : IUndoRedoAction
		{
			public SetNameUndoAction( ProjectViewModel viewModel, String oldName, String newName )
			{
				_viewModel = viewModel;
				_oldName = oldName;
				_newName = newName;
			}

			public void Undo()
			{
				_viewModel.Project.SetName( _oldName );
				_viewModel.InvokeNameChanged();
			}

			public void Redo()
			{
				_viewModel.Project.SetName( _newName );
				_viewModel.InvokeNameChanged();
			}

			readonly ProjectViewModel _viewModel;
			readonly String _oldName;
			readonly String _newName;
		}

		class SetMaxResultsPerSearchUndoAction : IUndoRedoAction
		{
			public SetMaxResultsPerSearchUndoAction( ProjectViewModel viewModel, Int32 oldValue, Int32 newValue )
			{
				_viewModel = viewModel;
				_oldValue = oldValue;
				_newValue = newValue;
			}

			public void Undo()
			{
				_viewModel.Project.SetMaxResultsPerSearch( _oldValue );
				_viewModel.InvokeMaxResultsPerSearchChanged();
			}

			public void Redo()
			{
				_viewModel.Project.SetMaxResultsPerSearch( _newValue );
				_viewModel.InvokeMaxResultsPerSearchChanged();
			}

			readonly ProjectViewModel _viewModel;
			readonly Int32 _oldValue;
			readonly Int32 _newValue;
		}

		class SetSearchAO3UndoAction : IUndoRedoAction
		{
			public SetSearchAO3UndoAction( ProjectViewModel viewModel, Boolean oldValue, Boolean newValue )
			{
				_viewModel = viewModel;
				_oldValue = oldValue;
				_newValue = newValue;
			}

			public void Undo()
			{
				_viewModel.Project.SetSearchAO3( _oldValue );
				_viewModel.InvokeSearchAO3Changed();
			}

			public void Redo()
			{
				_viewModel.Project.SetSearchAO3( _newValue );
				_viewModel.InvokeSearchAO3Changed();
			}

			readonly ProjectViewModel _viewModel;
			readonly Boolean _oldValue;
			readonly Boolean _newValue;
		}
	}
}
