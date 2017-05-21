using System;
using Alexandria.Searching;

namespace HeadLibrarian.ViewModels
{
	public abstract partial class NumberSearchCriteriaViewModel
	{
		class SetActualObjectUndoAction : IUndoRedoAction
		{
			public SetActualObjectUndoAction( NumberSearchCriteriaViewModel viewModel, NumberSearchCriteria oldObject, NumberSearchCriteria newObject )
			{
				_viewModel = viewModel;
				_oldObject = oldObject;
				_newObject = newObject;
			}

			public void Undo()
			{
				_viewModel.ActualObject = _oldObject;
				_viewModel.InvokeAllPropertiesChanged();
			}

			public void Redo()
			{
				_viewModel.ActualObject = _newObject;
				_viewModel.InvokeAllPropertiesChanged();
			}

			readonly NumberSearchCriteriaViewModel _viewModel;
			readonly NumberSearchCriteria _oldObject;
			readonly NumberSearchCriteria _newObject;
		}

		class SetTypeUndoAction : IUndoRedoAction
		{
			public SetTypeUndoAction( NumberSearchCriteriaViewModel viewModel, NumberSearchCriteriaType oldType, NumberSearchCriteriaType newType )
			{
				_viewModel = viewModel;
				_oldType = oldType;
				_newType = newType;
			}

			public void Undo()
			{
				_viewModel.ActualObject.Type = _oldType;
				_viewModel.InvokeTypeChanged();
			}

			public void Redo()
			{
				_viewModel.ActualObject.Type = _newType;
				_viewModel.InvokeTypeChanged();
			}

			readonly NumberSearchCriteriaViewModel _viewModel;
			readonly NumberSearchCriteriaType _oldType;
			readonly NumberSearchCriteriaType _newType;
		}

		class SetNumber1UndoAction : IUndoRedoAction
		{
			public SetNumber1UndoAction( NumberSearchCriteriaViewModel viewModel, Int32 oldNumber1, Int32 newNumber1 )
			{
				_viewModel = viewModel;
				_oldNumber1 = oldNumber1;
				_newNumber1 = newNumber1;
			}

			public void Undo()
			{
				_viewModel.ActualObject.Number1 = _oldNumber1;
				_viewModel.InvokeNumber1Changed();
			}

			public void Redo()
			{
				_viewModel.ActualObject.Number1 = _newNumber1;
				_viewModel.InvokeNumber1Changed();
			}

			readonly NumberSearchCriteriaViewModel _viewModel;
			readonly Int32 _oldNumber1;
			readonly Int32 _newNumber1;
		}

		class SetNumber2UndoAction : IUndoRedoAction
		{
			public SetNumber2UndoAction( NumberSearchCriteriaViewModel viewModel, Int32 oldNumber2, Int32 newNumber2 )
			{
				_viewModel = viewModel;
				_oldNumber2 = oldNumber2;
				_newNumber2 = newNumber2;
			}

			public void Undo()
			{
				_viewModel.ActualObject.Number2 = _oldNumber2;
				_viewModel.InvokeNumber2Changed();
			}

			public void Redo()
			{
				_viewModel.ActualObject.Number2 = _newNumber2;
				_viewModel.InvokeNumber2Changed();
			}

			readonly NumberSearchCriteriaViewModel _viewModel;
			readonly Int32 _oldNumber2;
			readonly Int32 _newNumber2;
		}
	}
}
