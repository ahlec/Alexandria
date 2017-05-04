using System;
using Alexandria.Searching;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class LibrarySearchViewModel : BaseViewModel
	{
		public LibrarySearchViewModel( ProjectViewModel projectViewModel, LibrarySearch search )
		{
			_projectViewModel = projectViewModel;
			_search = search;
		}

		public String Title
		{
			get => _search.Title;
			set
			{
				String oldTitle = Title;
				if ( String.Equals( oldTitle, value, StringComparison.CurrentCultureIgnoreCase ) )
				{
					return;
				}

				_search.Title = value;
				_projectViewModel.UndoStack.Push( new SetTitleUndoAction( this, oldTitle, value ) );
				InvokeTitleChanged();
			}
		}

		void InvokeTitleChanged()
		{
			OnPropertyChanged( nameof( Title ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		readonly ProjectViewModel _projectViewModel;
		readonly LibrarySearch _search;
	}
}
