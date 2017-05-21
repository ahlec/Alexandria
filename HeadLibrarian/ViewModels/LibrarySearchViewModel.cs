using System;
using System.Collections.Generic;
using Alexandria.Model;
using Alexandria.Searching;
using Alexandria.Utils;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class LibrarySearchViewModel : BaseViewModel
	{
		public LibrarySearchViewModel( ProjectViewModel projectViewModel, LibrarySearch search )
		{
			_projectViewModel = projectViewModel;
			_search = search;

			Date = new DateSearchCriteriaViewModel( projectViewModel, search );
			WordCount = new NumberSearchCriteriaViewModel.WordCount( projectViewModel, search );
			Fandoms = new StringListViewModel( projectViewModel, search.Fandoms );
			ContentWarnings = new ContentWarningViewModel( projectViewModel, search );
			CharacterNames = new StringListViewModel( projectViewModel, search.CharacterNames );
			Ships = new StringListViewModel( projectViewModel, search.Ships );
			Tags = new StringListViewModel( projectViewModel, search.Tags );
			NumberLikes = new NumberSearchCriteriaViewModel.NumberLikes( projectViewModel, search );
			NumberComments = new NumberSearchCriteriaViewModel.NumberComments( projectViewModel, search );
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

		public DateSearchCriteriaViewModel Date { get; }

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

		public NumberSearchCriteriaViewModel WordCount { get; }

		public static IEnumerable<ILanguageInfo> AllLanguages { get; } = GetAllLanguageOptions();

		public ILanguageInfo Language
		{
			get => ( _search.Language.HasValue ? LanguageUtils.GetInfo( _search.Language.Value ) : null );
			set
			{
				ILanguageInfo oldLanguage = Language;
				if ( oldLanguage?.Language == value?.Language )
				{
					return;
				}

				_search.Language = value?.Language;
				_projectViewModel.UndoStack.Push( new SetLanguageUndoAction( this, oldLanguage?.Language, value?.Language ) );
				InvokeLanguageChanged();
			}
		}

		void InvokeLanguageChanged()
		{
			OnPropertyChanged( nameof( Language ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public static IEnumerable<MaturityRating?> AllMaturityRatings { get; } = GetAllNullableEnumValues<MaturityRating>();

		public StringListViewModel Fandoms { get; }

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

		public ContentWarningViewModel ContentWarnings { get; }

		public StringListViewModel CharacterNames { get; }

		public StringListViewModel Ships { get; }

		public StringListViewModel Tags { get; }

		public NumberSearchCriteriaViewModel NumberLikes { get; }

		public NumberSearchCriteriaViewModel NumberComments { get; }

		static IEnumerable<T?> GetAllNullableEnumValues<T>() where T : struct, IConvertible
		{
			yield return null;
			foreach ( T enumValue in Enum.GetValues( typeof( T ) ) )
			{
				yield return enumValue;
			}
		}

		static IEnumerable<ILanguageInfo> GetAllLanguageOptions()
		{
			yield return null;
			foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
			{
				yield return LanguageUtils.GetInfo( language );
			}
		}

		readonly ProjectViewModel _projectViewModel;
		readonly LibrarySearch _search;
	}
}
