using System;
using System.Collections.Generic;
using System.Linq;
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

			_isTitleEnabled = !String.IsNullOrEmpty( search.Title );
			_isAuthorEnabled = !String.IsNullOrEmpty( search.Author );
			_isDateEnabled = ( search.Date != null );
			_isWordCountEnabled = ( search.WordCount != null );
			_isLanguageEnabled = ( search.Language != null );
			_areFandomsEnabled = ( search.Fandoms.Count > 0 );
			_isRatingEnabled = ( search.Rating != null );
			_areCharacterNamesEnabled = ( search.CharacterNames.Count > 0 );
			_areShipsEnabled = ( search.Ships.Count > 0 );
			_areTagsEnabled = ( search.Tags.Count > 0 );
			_isNumberLikesEnabled = ( search.NumberLikes != null );
			_isNumberCommentsEnabled = ( search.NumberComments != null );

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

		public Boolean IsTitleEnabled
		{
			get => _isTitleEnabled;
			set
			{
				Boolean oldEnabled = IsTitleEnabled;
				if ( SetProperty( ref _isTitleEnabled, value ) )
				{
					String oldTitle = Title;
					if ( !value )
					{
						_search.Title = null;
					}
					_projectViewModel.UndoStack.Push( new SetTitleUndoAction( this, oldEnabled, value, oldTitle, Title ) );
					InvokeTitleChanged();
				}
			}
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
				_projectViewModel.UndoStack.Push( new SetTitleUndoAction( this, IsTitleEnabled, IsTitleEnabled, oldTitle, value ) );
				InvokeTitleChanged();
			}
		}

		void InvokeTitleChanged()
		{
			OnPropertyChanged( nameof( IsTitleEnabled ) );
			OnPropertyChanged( nameof( Title ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean IsAuthorEnabled
		{
			get => _isAuthorEnabled;
			set
			{
				Boolean oldEnabled = IsAuthorEnabled;
				if ( SetProperty( ref _isAuthorEnabled, value ) )
				{
					String oldAuthor = Author;
					if ( !value )
					{
						_search.Author = null;
					}
					_projectViewModel.UndoStack.Push( new SetAuthorUndoAction( this, oldEnabled, value, oldAuthor, Author ) );
					InvokeAuthorChanged();
				}
			}
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
				_projectViewModel.UndoStack.Push( new SetAuthorUndoAction( this, IsAuthorEnabled, IsAuthorEnabled, oldAuthor, value ) );
				InvokeAuthorChanged();
			}
		}

		void InvokeAuthorChanged()
		{
			OnPropertyChanged( nameof( IsAuthorEnabled ) );
			OnPropertyChanged( nameof( Author ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean IsDateEnabled
		{
			get => _isDateEnabled;
			set
			{
				Boolean oldEnabled = IsDateEnabled;
				if ( SetProperty( ref _isDateEnabled, value ) )
				{
					DateSearchCriteria oldDate = _search.Date;
					if ( !value )
					{
						_search.Date = null;
					}
					else if ( _search.Date == null )
					{
						_search.Date = new DateSearchCriteria
						{
							Type = default( DateSearchCriteriaType )
						};
					}
					_projectViewModel.UndoStack.Push( new SetIsDateEnabledUndoAction( this, oldEnabled, value, oldDate, _search.Date ) );
					InvokeIsDateEnabledChanged();
				}
			}
		}

		public DateSearchCriteriaViewModel Date { get; }

		void InvokeIsDateEnabledChanged()
		{
			OnPropertyChanged( nameof( IsDateEnabled ) );
			Date.InvokeAllPropertiesChanged();
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

		public Boolean IsWordCountEnabled
		{
			get => _isWordCountEnabled;
			set
			{
				Boolean oldEnabled = IsWordCountEnabled;
				if ( SetProperty( ref _isWordCountEnabled, value ) )
				{
					NumberSearchCriteria oldCount = _search.WordCount;
					if ( !value )
					{
						_search.WordCount = null;
					}
					else if ( _search.WordCount == null )
					{
						_search.WordCount = new NumberSearchCriteria
						{
							Type = default( NumberSearchCriteriaType )
						};
					}
					_projectViewModel.UndoStack.Push( new SetIsWordCountEnabledUndoAction( this, oldEnabled, value, oldCount, _search.WordCount ) );
					InvokeIsWordCountEnabledChanged();
				}
			}
		}

		public NumberSearchCriteriaViewModel WordCount { get; }

		void InvokeIsWordCountEnabledChanged()
		{
			OnPropertyChanged( nameof( IsWordCountEnabled ) );
			WordCount.InvokeAllPropertiesChanged();
		}

		public static IEnumerable<ILanguageInfo> AllLanguages { get; } = GetAllLanguageOptions();

		public Boolean IsLanguageEnabled
		{
			get => _isLanguageEnabled;
			set
			{
				Boolean oldEnabled = IsLanguageEnabled;
				if ( SetProperty( ref _isLanguageEnabled, value ) )
				{
					Language? oldLanguage = _search.Language;
					if ( !value )
					{
						_search.Language = null;
					}
					_projectViewModel.UndoStack.Push( new SetLanguageUndoAction( this, oldEnabled, value, oldLanguage, _search.Language ) );
					InvokeLanguageChanged();
				}
			}
		}

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
				_projectViewModel.UndoStack.Push( new SetLanguageUndoAction( this, IsTitleEnabled, IsLanguageEnabled, oldLanguage?.Language, value?.Language ) );
				InvokeLanguageChanged();
			}
		}

		void InvokeLanguageChanged()
		{
			OnPropertyChanged( nameof( IsLanguageEnabled ) );
			OnPropertyChanged( nameof( Language ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean AreFandomsEnabled
		{
			get => _areFandomsEnabled;
			set
			{
				Boolean oldEnabled = AreFandomsEnabled;
				if ( SetProperty( ref _areFandomsEnabled, value ) )
				{
					IReadOnlyList<String> oldValues = _search.Fandoms.ToList(); // copy
					if ( !value )
					{
						_search.Fandoms.Clear();
					}
					_projectViewModel.UndoStack.Push( new SetAreFandomsEnabledUndoAction( this, oldEnabled, value, oldValues, _search.Fandoms.ToList() ) );
					InvokeAreFandomsEnabledChanged();
				}
			}
		}

		public StringListViewModel Fandoms { get; }

		void InvokeAreFandomsEnabledChanged()
		{
			OnPropertyChanged( nameof( AreFandomsEnabled ) );
			Fandoms.InvokeItemsReplaced();
			_projectViewModel.RefreshHasSavedChanged();
		}

		public static IEnumerable<MaturityRating?> AllMaturityRatings { get; } = GetAllMaturityRatings();

		public Boolean IsMaturityRatingEnabled
		{
			get => _isRatingEnabled;
			set
			{
				Boolean oldEnabled = IsMaturityRatingEnabled;
				if ( SetProperty( ref _isRatingEnabled, value ) )
				{
					MaturityRating? oldRating = _search.Rating;
					if ( !value )
					{
						_search.Rating = null;
					}
					_projectViewModel.UndoStack.Push( new SetMaturityRatingUndoAction( this, oldEnabled, value, oldRating, _search.Rating ) );
					InvokeMaturityRatingChanged();
				}
			}
		}

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
				_projectViewModel.UndoStack.Push( new SetMaturityRatingUndoAction( this, IsMaturityRatingEnabled, IsMaturityRatingEnabled, oldMaturityRating, value ) );
				InvokeMaturityRatingChanged();
			}
		}

		void InvokeMaturityRatingChanged()
		{
			OnPropertyChanged( nameof( IsMaturityRatingEnabled ) );
			OnPropertyChanged( nameof( MaturityRating ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public ContentWarningViewModel ContentWarnings { get; }

		public Boolean AreCharacterNamesEnabled
		{
			get => _areCharacterNamesEnabled;
			set
			{
				Boolean oldEnabled = AreCharacterNamesEnabled;
				if ( SetProperty( ref _areCharacterNamesEnabled, value ) )
				{
					IReadOnlyList<String> oldValues = _search.CharacterNames.ToList(); // copy
					if ( !value )
					{
						_search.CharacterNames.Clear();
					}
					_projectViewModel.UndoStack.Push( new SetAreCharacterNamesEnabledUndoAction( this, oldEnabled, value, oldValues, _search.CharacterNames.ToList() ) );
					InvokeAreCharacterNamesEnabledChanged();
				}
			}
		}

		public StringListViewModel CharacterNames { get; }

		void InvokeAreCharacterNamesEnabledChanged()
		{
			OnPropertyChanged( nameof( AreCharacterNamesEnabled ) );
			CharacterNames.InvokeItemsReplaced();
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean AreShipsEnabled
		{
			get => _areShipsEnabled;
			set
			{
				Boolean oldEnabled = AreShipsEnabled;
				if ( SetProperty( ref _areShipsEnabled, value ) )
				{
					IReadOnlyList<String> oldValues = _search.Ships.ToList(); // copy
					if ( !value )
					{
						_search.Ships.Clear();
					}
					_projectViewModel.UndoStack.Push( new SetAreShipsEnabledUndoAction( this, oldEnabled, value, oldValues, _search.Ships.ToList() ) );
					InvokeAreShipsEnabledChanged();
				}
			}
		}

		public StringListViewModel Ships { get; }

		void InvokeAreShipsEnabledChanged()
		{
			OnPropertyChanged( nameof( AreShipsEnabled ) );
			Ships.InvokeItemsReplaced();
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean AreTagsEnabled
		{
			get => _areTagsEnabled;
			set
			{
				Boolean oldEnabled = AreTagsEnabled;
				if ( SetProperty( ref _areTagsEnabled, value ) )
				{
					IReadOnlyList<String> oldValues = _search.Tags.ToList(); // copy
					if ( !value )
					{
						_search.Tags.Clear();
					}
					_projectViewModel.UndoStack.Push( new SetAreTagsEnabledUndoAction( this, oldEnabled, value, oldValues, _search.Tags.ToList() ) );
					InvokeAreTagsEnabledChanged();
				}
			}
		}

		public StringListViewModel Tags { get; }

		void InvokeAreTagsEnabledChanged()
		{
			OnPropertyChanged( nameof( AreTagsEnabled ) );
			Tags.InvokeItemsReplaced();
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean IsNumberLikesEnabled
		{
			get => _isNumberLikesEnabled;
			set
			{
				Boolean oldEnabled = IsNumberLikesEnabled;
				if ( SetProperty( ref _isNumberLikesEnabled, value ) )
				{
					NumberSearchCriteria oldCount = _search.NumberLikes;
					if ( !value )
					{
						_search.NumberLikes = null;
					}
					else if ( _search.NumberLikes == null )
					{
						_search.NumberLikes = new NumberSearchCriteria
						{
							Type = default( NumberSearchCriteriaType )
						};
					}
					_projectViewModel.UndoStack.Push( new SetIsNumberLikesEnabledUndoAction( this, oldEnabled, value, oldCount, _search.NumberLikes ) );
					InvokeIsNumberLikesEnabledChanged();
				}
			}
		}

		public NumberSearchCriteriaViewModel NumberLikes { get; }

		void InvokeIsNumberLikesEnabledChanged()
		{
			OnPropertyChanged( nameof( IsNumberLikesEnabled ) );
			NumberLikes.InvokeAllPropertiesChanged();
		}

		public Boolean IsNumberCommentsEnabled
		{
			get => _isNumberCommentsEnabled;
			set
			{
				Boolean oldEnabled = IsNumberCommentsEnabled;
				if ( SetProperty( ref _isNumberCommentsEnabled, value ) )
				{
					NumberSearchCriteria oldCount = _search.NumberComments;
					if ( !value )
					{
						_search.NumberComments = null;
					}
					else if ( _search.NumberComments == null )
					{
						_search.NumberComments = new NumberSearchCriteria
						{
							Type = default( NumberSearchCriteriaType )
						};
					}
					_projectViewModel.UndoStack.Push( new SetIsNumberCommentsEnabledUndoAction( this, oldEnabled, value, oldCount, _search.NumberComments ) );
					InvokeIsNumberCommentsEnabledChanged();
				}
			}
		}

		public NumberSearchCriteriaViewModel NumberComments { get; }

		void InvokeIsNumberCommentsEnabledChanged()
		{
			OnPropertyChanged( nameof( IsNumberCommentsEnabled ) );
			NumberComments.InvokeAllPropertiesChanged();
		}

		public static IEnumerable<SearchField> AllSortFields { get; } = GetAllSearchFields();

		public SearchField SortField
		{
			get => _search.SortField;
			set
			{
				SearchField oldSortField = SortField;
				if ( oldSortField == value )
				{
					return;
				}

				_search.SortField = value;
				_projectViewModel.UndoStack.Push( new SetSortFieldUndoAction( this, oldSortField, value ) );
				InvokeSortFieldChanged();
			}
		}

		void InvokeSortFieldChanged()
		{
			OnPropertyChanged( nameof( SortField ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public static IEnumerable<SortDirection> AllSortDirections { get; } = GetAllSortDirections();

		public SortDirection SortDirection
		{
			get => _search.SortDirection;
			set
			{
				SortDirection oldSortDirection = SortDirection;
				if ( oldSortDirection == value )
				{
					return;
				}

				_search.SortDirection = value;
				_projectViewModel.UndoStack.Push( new SetSortDirectionUndoAction( this, oldSortDirection, value ) );
				InvokeSortDirectionChanged();
			}
		}

		void InvokeSortDirectionChanged()
		{
			OnPropertyChanged( nameof( SortDirection ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		static IEnumerable<MaturityRating?> GetAllMaturityRatings()
		{
			foreach ( MaturityRating rating in Enum.GetValues( typeof( MaturityRating ) ) )
			{
				yield return rating;
			}
		}

		static IEnumerable<ILanguageInfo> GetAllLanguageOptions()
		{
			foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
			{
				yield return LanguageUtils.GetInfo( language );
			}
		}

		static IEnumerable<SearchField> GetAllSearchFields()
		{
			foreach ( SearchField field in Enum.GetValues( typeof( SearchField ) ) )
			{
				yield return field;
			}
		}

		static IEnumerable<SortDirection> GetAllSortDirections()
		{
			foreach ( SortDirection direction in Enum.GetValues( typeof( SortDirection ) ) )
			{
				yield return direction;
			}
		}

		readonly ProjectViewModel _projectViewModel;
		readonly LibrarySearch _search;

		Boolean _isTitleEnabled;
		Boolean _isAuthorEnabled;
		Boolean _isDateEnabled;
		Boolean _isWordCountEnabled;
		Boolean _isLanguageEnabled;
		Boolean _areFandomsEnabled;
		Boolean _isRatingEnabled;
		Boolean _areCharacterNamesEnabled;
		Boolean _areShipsEnabled;
		Boolean _areTagsEnabled;
		Boolean _isNumberLikesEnabled;
		Boolean _isNumberCommentsEnabled;
	}
}
