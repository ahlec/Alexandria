using System;
using System.Collections.Generic;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class StringListViewModel
	{
		class AddStringUndoAction : IUndoRedoAction
		{
			public AddStringUndoAction( StringListViewModel viewModel, List<String> list, String itemAdded )
			{
				_viewModel = viewModel;
				_list = list;
				_itemAdded = itemAdded;
			}

			public void Undo()
			{
				Int32 index = _list.IndexOf( _itemAdded );
				if ( !_list.Remove( _itemAdded ) )
				{
					throw new ApplicationException( $"Could not redo the {nameof( AddStringUndoAction )}!" );
				}
				_viewModel.InvokeItemRemoved( _itemAdded, index );
			}

			public void Redo()
			{
				_list.Add( _itemAdded );
				_viewModel.InvokeItemAdded( _itemAdded, _list.Count - 1 );
			}

			readonly StringListViewModel _viewModel;
			readonly List<String> _list;
			readonly String _itemAdded;
		}

		class RemoveStringUndoAction : IUndoRedoAction
		{
			public RemoveStringUndoAction( StringListViewModel viewModel, List<String> list, String itemRemoved )
			{
				_viewModel = viewModel;
				_list = list;
				_itemRemoved = itemRemoved;
			}

			public void Undo()
			{
				_list.Add( _itemRemoved );
				_viewModel.InvokeItemAdded( _itemRemoved, _list.Count - 1 );
			}

			public void Redo()
			{
				Int32 index = _list.IndexOf( _itemRemoved );
				if ( !_list.Remove( _itemRemoved ) )
				{
					throw new ApplicationException( $"Could not redo the {nameof( RemoveStringUndoAction )}!" );
				}
				_viewModel.InvokeItemRemoved( _itemRemoved, index );
			}

			readonly StringListViewModel _viewModel;
			readonly List<String> _list;
			readonly String _itemRemoved;
		}
	}
}
