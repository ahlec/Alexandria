using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using Bibliothecary.Core.Publishing;
using HeadLibrarian.WPF;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class TumblrTagRuleListViewModel : BaseViewModel, INotifyCollectionChanged, IEnumerable<TumblrTagRule>
	{
		public TumblrTagRuleListViewModel( ProjectViewModel projectViewModel )
		{
			_projectViewModel = projectViewModel;
		}

		public String CurrentInputText
		{
			get => _currentInputText;
			set => SetProperty( ref _currentInputText, value );
		}

		public IEnumerator<TumblrTagRule> GetEnumerator()
		{
			return ReadOnlyList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		internal void InvokeItemsReplaced()
		{
			CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
			if ( ReadOnlyList.Count > 0 )
			{
				CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, ReadOnlyList, 0 ) );
			}
		}

		void InvokeItemAdded( TumblrTagRule tag, Int32 index )
		{
			CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, tag, index ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		void InvokeItemRemoved( TumblrTagRule tag, Int32 index )
		{
			CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, tag, index ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		void CommandAdd( Object o )
		{
			if ( String.IsNullOrWhiteSpace( CurrentInputText ) )
			{
				return;
			}

			TumblrTagRule tag = new TumblrTagRule( CurrentInputText );
			if ( !_projectViewModel.Project.PublishingInfo.AddTumblrTag( tag ) )
			{
				return;
			}
			_projectViewModel.UndoStack.Push( new AddTagUndoAction( this, _projectViewModel.Project.PublishingInfo, tag ) );
			InvokeItemAdded( tag, ReadOnlyList.Count - 1 );
			CurrentInputText = null;
		}

		void CommandRemove( Object o )
		{
			if ( !( o is TumblrTagRule ) )
			{
				throw new ArgumentException( $"Argument must be a {nameof( TumblrTagRule )}!", nameof( o ) );
			}
			TumblrTagRule tag = (TumblrTagRule) o;

			if ( !_projectViewModel.Project.PublishingInfo.RemoveTumblrTag( tag, out Int32 oldIndex ) )
			{
				return;
			}

			_projectViewModel.UndoStack.Push( new RemoveTagUndoAction( this, _projectViewModel.Project.PublishingInfo, tag, oldIndex ) );
			InvokeItemRemoved( tag, oldIndex );
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public ICommand AddCommand => ( _addCommand ?? ( _addCommand = new Command( null, CommandAdd ) ) );

		public ICommand RemoveCommand => ( _removeCommand ?? ( _removeCommand = new Command( null, CommandRemove ) ) );

		IReadOnlyList<TumblrTagRule> ReadOnlyList => _projectViewModel.Project.PublishingInfo.TumblrTags;

		readonly ProjectViewModel _projectViewModel;
		ICommand _addCommand;
		ICommand _removeCommand;
		String _currentInputText;
	}
}
