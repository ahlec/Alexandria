using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using HeadLibrarian.WPF;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class StringListViewModel : BaseViewModel, INotifyCollectionChanged, IEnumerable<String>
	{
		public StringListViewModel( ProjectViewModel projectViewModel, List<String> list )
		{
			_projectViewModel = projectViewModel;
			_list = list;
		}

		public String CurrentInputText
		{
			get => _currentInputText;
			set => SetProperty( ref _currentInputText, value );
		}

		public IEnumerator<String> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		void InvokeItemAdded( String item, Int32 index )
		{
			CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		void InvokeItemRemoved( String item, Int32 index )
		{
			CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item, index ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		void CommandAdd( Object o )
		{
			if ( String.IsNullOrWhiteSpace( CurrentInputText ) )
			{
				return;
			}

			if ( _list.Contains( CurrentInputText, StringComparer.InvariantCultureIgnoreCase ) )
			{
				return;
			}

			_list.Add( CurrentInputText );
			_projectViewModel.UndoStack.Push( new AddStringUndoAction( this, _list, CurrentInputText ) );
			InvokeItemAdded( CurrentInputText, _list.Count - 1 );
			CurrentInputText = null;
		}

		void CommandRemove( Object o )
		{
			if ( !( o is String ) )
			{
				throw new ArgumentException( "Argument must be a string!", nameof( o ) );
			}
			String item = (String) o;

			Int32 index = _list.IndexOf( item );
			if ( !_list.Remove( item ) )
			{
				throw new ApplicationException( $"Could not remove '{item}' from the list!" );
			}
			_projectViewModel.UndoStack.Push( new RemoveStringUndoAction( this, _list, item ) );
			InvokeItemRemoved( item, index );
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public ICommand AddCommand => ( _addCommand ?? ( _addCommand = new Command( null, CommandAdd ) ) );

		public ICommand RemoveCommand => ( _removeCommand ?? ( _removeCommand = new Command( null, CommandRemove ) ) );

		readonly ProjectViewModel _projectViewModel;
		readonly List<String> _list;
		ICommand _addCommand;
		ICommand _removeCommand;
		String _currentInputText;
	}
}
