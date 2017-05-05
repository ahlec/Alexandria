using System;
using System.Collections.Generic;
using Alexandria.Model;
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

		public String Author
		{
			get => _search.Author;
			set
			{
				String oldAuthor = Author;
				if ( String.Equals( oldAuthor, value, StringComparison.CurrentCultureIgnoreCase ) )
				{
					return;
				}

				_search.Author = value;
				_projectViewModel.UndoStack.Push( new SetAuthorUndoAction( this, oldAuthor, value ) );
				InvokeAuthorChanged();
			}
		}

		void InvokeAuthorChanged()
		{
			OnPropertyChanged( nameof( Author ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean OnlyIncludeComplete
		{
			get => _search.OnlyIncludeCompleteFanfics;
			set
			{
				Boolean oldValue = OnlyIncludeComplete;
				if ( value == oldValue )
				{
					return;
				}

				_search.OnlyIncludeCompleteFanfics = value;
				_projectViewModel.UndoStack.Push( new SetOnlyIncludeCompleteUndoAction( this, oldValue, value ) );
				InvokeOnlyIncludeCompleteChanged();
			}
		}

		void InvokeOnlyIncludeCompleteChanged()
		{
			OnPropertyChanged( nameof( OnlyIncludeComplete ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean OnlyIncludeSingleChapter
		{
			get => _search.OnlyIncludeSingleChapterFanfics;
			set
			{
				Boolean oldValue = OnlyIncludeSingleChapter;
				if ( value == oldValue )
				{
					return;
				}

				_search.OnlyIncludeSingleChapterFanfics = value;
				_projectViewModel.UndoStack.Push( new SetOnlyIncludeSingleChapterUndoAction( this, oldValue, value ) );
				InvokeOnlyIncludeSingleChapterChanged();
			}
		}

		void InvokeOnlyIncludeSingleChapterChanged()
		{
			OnPropertyChanged( nameof( OnlyIncludeSingleChapter ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public IEnumerable<Language?> AllLanguages { get; } = GetAllNullableEnumValues<Language>();

		public Language? Language
		{
			get => _search.Language;
			set
			{
				Language? oldLanguage = Language;
				if ( oldLanguage == value )
				{
					return;
				}

				_search.Language = value;
				_projectViewModel.UndoStack.Push( new SetLanguageUndoAction( this, oldLanguage, value ) );
				InvokeLanguageChanged();
			}
		}

		void InvokeLanguageChanged()
		{
			OnPropertyChanged( nameof( Language ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public IEnumerable<MaturityRating?> AllMaturityRatings { get; } = GetAllNullableEnumValues<MaturityRating>();

		public MaturityRating? MaturityRating
		{
			get => _search.Rating;
			set
			{
				MaturityRating? oldMaturityRating = MaturityRating;
				if ( oldMaturityRating == value )
				{
					return;
				}

				_search.Rating = value;
				_projectViewModel.UndoStack.Push( new SetMaturityRatingUndoAction( this, oldMaturityRating, value ) );
				InvokeMaturityRatingChanged();
			}
		}

		void InvokeMaturityRatingChanged()
		{
			OnPropertyChanged( nameof( MaturityRating ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		static IEnumerable<T?> GetAllNullableEnumValues<T>() where T : struct, IConvertible
		{
			yield return null;
			foreach ( T enumValue in Enum.GetValues( typeof( T ) ) )
			{
				yield return enumValue;
			}
		}

		readonly ProjectViewModel _projectViewModel;
		readonly LibrarySearch _search;
	}
}
