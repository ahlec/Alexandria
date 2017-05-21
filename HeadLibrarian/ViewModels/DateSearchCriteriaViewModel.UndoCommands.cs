using System;
using Alexandria.Searching;

namespace HeadLibrarian.ViewModels
{
	public partial class DateSearchCriteriaViewModel
	{
		class SetActualObjectUndoAction : IUndoRedoAction
		{
			public SetActualObjectUndoAction( DateSearchCriteriaViewModel viewModel, DateSearchCriteria oldObject, DateSearchCriteria newObject )
			{
				_viewModel = viewModel;
				_oldObject = oldObject;
				_newObject = newObject;
			}

			public void Undo()
			{
				_viewModel.Search.Date = _oldObject;
				_viewModel.InvokeAllPropertiesChanged();
			}

			public void Redo()
			{
				_viewModel.Search.Date = _newObject;
				_viewModel.InvokeAllPropertiesChanged();
			}

			readonly DateSearchCriteriaViewModel _viewModel;
			readonly DateSearchCriteria _oldObject;
			readonly DateSearchCriteria _newObject;
		}

		class SetTypeUndoAction : IUndoRedoAction
		{
			public SetTypeUndoAction( DateSearchCriteriaViewModel viewModel, DateSearchCriteriaType oldType, DateSearchCriteriaType newType )
			{
				_viewModel = viewModel;
				_oldType = oldType;
				_newType = newType;
			}

			public void Undo()
			{
				_viewModel.Search.Date.Type = _oldType;
				_viewModel.InvokeTypeChanged();
			}

			public void Redo()
			{
				_viewModel.Search.Date.Type = _newType;
				_viewModel.InvokeTypeChanged();
			}

			readonly DateSearchCriteriaViewModel _viewModel;
			readonly DateSearchCriteriaType _oldType;
			readonly DateSearchCriteriaType _newType;
		}

		class SetDateUnitUndoAction : IUndoRedoAction
		{
			public SetDateUnitUndoAction( DateSearchCriteriaViewModel viewModel, DateField oldUnit, DateField newUnit )
			{
				_viewModel = viewModel;
				_oldUnit = oldUnit;
				_newUnit = newUnit;
			}

			public void Undo()
			{
				_viewModel.Search.Date.DateUnit = _oldUnit;
				_viewModel.InvokeDateUnitChanged();
			}

			public void Redo()
			{
				_viewModel.Search.Date.DateUnit = _newUnit;
				_viewModel.InvokeDateUnitChanged();
			}

			readonly DateSearchCriteriaViewModel _viewModel;
			readonly DateField _oldUnit;
			readonly DateField _newUnit;
		}

		class SetNumber1UndoAction : IUndoRedoAction
		{
			public SetNumber1UndoAction( DateSearchCriteriaViewModel viewModel, Int32 oldNumber1, Int32 newNumber1 )
			{
				_viewModel = viewModel;
				_oldNumber1 = oldNumber1;
				_newNumber1 = newNumber1;
			}

			public void Undo()
			{
				_viewModel.Search.Date.Number1 = _oldNumber1;
				_viewModel.InvokeNumber1Changed();
			}

			public void Redo()
			{
				_viewModel.Search.Date.Number1 = _newNumber1;
				_viewModel.InvokeNumber1Changed();
			}

			readonly DateSearchCriteriaViewModel _viewModel;
			readonly Int32 _oldNumber1;
			readonly Int32 _newNumber1;
		}

		class SetNumber2UndoAction : IUndoRedoAction
		{
			public SetNumber2UndoAction( DateSearchCriteriaViewModel viewModel, Int32 oldNumber2, Int32 newNumber2 )
			{
				_viewModel = viewModel;
				_oldNumber2 = oldNumber2;
				_newNumber2 = newNumber2;
			}

			public void Undo()
			{
				_viewModel.Search.Date.Number2 = _oldNumber2;
				_viewModel.InvokeNumber2Changed();
			}

			public void Redo()
			{
				_viewModel.Search.Date.Number2 = _newNumber2;
				_viewModel.InvokeNumber2Changed();
			}

			readonly DateSearchCriteriaViewModel _viewModel;
			readonly Int32 _oldNumber2;
			readonly Int32 _newNumber2;
		}
	}
}
