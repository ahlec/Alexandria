using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using HeadLibrarian.WPF;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class StringListViewModel : BaseViewModel, INotifyCollectionChanged
	{
		public StringListViewModel( ProjectViewModel projectViewModel, List<String> list )
		{
			_projectViewModel = projectViewModel;
			_list = list;
		}

		public IEnumerable<String> Items => _list;

		void InvokeItemAdded( String item, Int32 index )
		{
			OnPropertyChanged( nameof( Items ) );
			CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		void InvokeItemRemoved( String item, Int32 index )
		{
			OnPropertyChanged( nameof( Items ) );
			CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item, index ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		void CommandAdd( Object o )
		{
			if ( !( o is String ) )
			{
				throw new ArgumentException( "Argument must be a string!", nameof( o ) );
			}
			String item = (String) o;
			if ( String.IsNullOrWhiteSpace( item ) )
			{
				return;
			}

			if ( _list.Contains( item, StringComparer.InvariantCultureIgnoreCase ) )
			{
				return;
			}

			_list.Add( item );
			_projectViewModel.UndoStack.Push( new AddStringUndoAction( this, _list, item ) );
			InvokeItemAdded( item, _list.Count - 1 );
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
	}
}
