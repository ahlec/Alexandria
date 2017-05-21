using System;
using System.Collections.Generic;
using Alexandria.Model;
using Alexandria.Searching;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class LibrarySearchViewModel
	{
		class SetTitleUndoAction : IUndoRedoAction
		{
			public SetTitleUndoAction( LibrarySearchViewModel viewModel, Boolean oldEnabled, Boolean newEnabled, String oldName, String newName )
			{
				_viewModel = viewModel;
				_oldEnabled = oldEnabled;
				_newEnabled = newEnabled;
				_oldName = oldName;
				_newName = newName;
			}

			public void Undo()
			{
				_viewModel._isTitleEnabled = _oldEnabled;
				_viewModel._search.Title = _oldName;
				_viewModel.InvokeTitleChanged();
			}

			public void Redo()
			{
				_viewModel._isTitleEnabled = _newEnabled;
				_viewModel._search.Title = _newName;
				_viewModel.InvokeTitleChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldEnabled;
			readonly Boolean _newEnabled;
			readonly String _oldName;
			readonly String _newName;
		}

		class SetAuthorUndoAction : IUndoRedoAction
		{
			public SetAuthorUndoAction( LibrarySearchViewModel viewModel, Boolean oldEnabled, Boolean newEnabled, String oldAuthor, String newAuthor )
			{
				_viewModel = viewModel;
				_oldEnabled = oldEnabled;
				_newEnabled = newEnabled;
				_oldAuthor = oldAuthor;
				_newAuthor = newAuthor;
			}

			public void Undo()
			{
				_viewModel._isAuthorEnabled = _oldEnabled;
				_viewModel._search.Author = _oldAuthor;
				_viewModel.InvokeAuthorChanged();
			}

			public void Redo()
			{
				_viewModel._isAuthorEnabled = _newEnabled;
				_viewModel._search.Author = _newAuthor;
				_viewModel.InvokeAuthorChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldEnabled;
			readonly Boolean _newEnabled;
			readonly String _oldAuthor;
			readonly String _newAuthor;
		}

		class SetIsDateEnabledUndoAction : IUndoRedoAction
		{
			public SetIsDateEnabledUndoAction( LibrarySearchViewModel viewModel, Boolean oldEnabled, Boolean newEnabled, DateSearchCriteria oldDate, DateSearchCriteria newDate )
			{
				_viewModel = viewModel;
				_oldEnabled = oldEnabled;
				_newEnabled = newEnabled;
				_oldDate = oldDate;
				_newDate = newDate;
			}

			public void Undo()
			{
				_viewModel._isDateEnabled = _oldEnabled;
				_viewModel._search.Date = _oldDate;
				_viewModel.InvokeIsDateEnabledChanged();
			}

			public void Redo()
			{
				_viewModel._isDateEnabled = _newEnabled;
				_viewModel._search.Date = _newDate;
				_viewModel.InvokeIsDateEnabledChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldEnabled;
			readonly Boolean _newEnabled;
			readonly DateSearchCriteria _oldDate;
			readonly DateSearchCriteria _newDate;
		}

		class SetOnlyIncludeCompleteUndoAction : IUndoRedoAction
		{
			public SetOnlyIncludeCompleteUndoAction( LibrarySearchViewModel viewModel, Boolean oldValue, Boolean newValue )
			{
				_viewModel = viewModel;
				_oldValue = oldValue;
				_newValue = newValue;
			}

			public void Undo()
			{
				_viewModel._search.OnlyIncludeCompleteFanfics = _oldValue;
				_viewModel.InvokeOnlyIncludeCompleteChanged();
			}

			public void Redo()
			{
				_viewModel._search.OnlyIncludeCompleteFanfics = _newValue;
				_viewModel.InvokeOnlyIncludeCompleteChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldValue;
			readonly Boolean _newValue;
		}

		class SetOnlyIncludeSingleChapterUndoAction : IUndoRedoAction
		{
			public SetOnlyIncludeSingleChapterUndoAction( LibrarySearchViewModel viewModel, Boolean oldValue, Boolean newValue )
			{
				_viewModel = viewModel;
				_oldValue = oldValue;
				_newValue = newValue;
			}

			public void Undo()
			{
				_viewModel._search.OnlyIncludeSingleChapterFanfics = _oldValue;
				_viewModel.InvokeOnlyIncludeSingleChapterChanged();
			}

			public void Redo()
			{
				_viewModel._search.OnlyIncludeSingleChapterFanfics = _newValue;
				_viewModel.InvokeOnlyIncludeSingleChapterChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldValue;
			readonly Boolean _newValue;
		}

		class SetIsWordCountEnabledUndoAction : IUndoRedoAction
		{
			public SetIsWordCountEnabledUndoAction( LibrarySearchViewModel viewModel, Boolean oldEnabled, Boolean newEnabled, NumberSearchCriteria oldCount, NumberSearchCriteria newCount )
			{
				_viewModel = viewModel;
				_oldEnabled = oldEnabled;
				_newEnabled = newEnabled;
				_oldCount = oldCount;
				_newCount = newCount;
			}

			public void Undo()
			{
				_viewModel._isWordCountEnabled = _oldEnabled;
				_viewModel._search.WordCount = _oldCount;
				_viewModel.InvokeIsWordCountEnabledChanged();
			}

			public void Redo()
			{
				_viewModel._isWordCountEnabled = _newEnabled;
				_viewModel._search.WordCount = _newCount;
				_viewModel.InvokeIsWordCountEnabledChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldEnabled;
			readonly Boolean _newEnabled;
			readonly NumberSearchCriteria _oldCount;
			readonly NumberSearchCriteria _newCount;
		}

		class SetLanguageUndoAction : IUndoRedoAction
		{
			public SetLanguageUndoAction( LibrarySearchViewModel viewModel, Boolean oldEnabled, Boolean newEnabled, Language? oldLanguage, Language? newLanguage )
			{
				_viewModel = viewModel;
				_oldEnabled = oldEnabled;
				_newEnabled = newEnabled;
				_oldLanguage = oldLanguage;
				_newLanguage = newLanguage;
			}

			public void Undo()
			{
				_viewModel._isLanguageEnabled = _oldEnabled;
				_viewModel._search.Language = _oldLanguage;
				_viewModel.InvokeLanguageChanged();
			}

			public void Redo()
			{
				_viewModel._isLanguageEnabled = _newEnabled;
				_viewModel._search.Language = _newLanguage;
				_viewModel.InvokeLanguageChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldEnabled;
			readonly Boolean _newEnabled;
			readonly Language? _oldLanguage;
			readonly Language? _newLanguage;
		}

		class SetAreFandomsEnabledUndoAction : IUndoRedoAction
		{
			public SetAreFandomsEnabledUndoAction( LibrarySearchViewModel viewModel, Boolean oldEnabled, Boolean newEnabled, IReadOnlyList<String> oldValues, IReadOnlyList<String> newValues )
			{
				_viewModel = viewModel;
				_oldEnabled = oldEnabled;
				_newEnabled = newEnabled;
				_oldValues = oldValues;
				_newValues = newValues;
			}

			public void Undo()
			{
				_viewModel._areFandomsEnabled = _oldEnabled;
				_viewModel._search.Fandoms.Clear();
				_viewModel._search.Fandoms.AddRange( _oldValues );
				_viewModel.InvokeAreFandomsEnabledChanged();
			}

			public void Redo()
			{
				_viewModel._areFandomsEnabled = _newEnabled;
				_viewModel._search.Fandoms.Clear();
				_viewModel._search.Fandoms.AddRange( _newValues );
				_viewModel.InvokeAreFandomsEnabledChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldEnabled;
			readonly Boolean _newEnabled;
			readonly IReadOnlyList<String> _oldValues;
			readonly IReadOnlyList<String> _newValues;
		}

		class SetMaturityRatingUndoAction : IUndoRedoAction
		{
			public SetMaturityRatingUndoAction( LibrarySearchViewModel viewModel, Boolean oldEnabled, Boolean newEnabled, MaturityRating? oldMaturityRating, MaturityRating? newMaturityRating )
			{
				_viewModel = viewModel;
				_oldEnabled = oldEnabled;
				_newEnabled = newEnabled;
				_oldMaturityRating = oldMaturityRating;
				_newMaturityRating = newMaturityRating;
			}

			public void Undo()
			{
				_viewModel._isRatingEnabled = _oldEnabled;
				_viewModel._search.Rating = _oldMaturityRating;
				_viewModel.InvokeMaturityRatingChanged();
			}

			public void Redo()
			{
				_viewModel._isRatingEnabled = _newEnabled;
				_viewModel._search.Rating = _newMaturityRating;
				_viewModel.InvokeMaturityRatingChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldEnabled;
			readonly Boolean _newEnabled;
			readonly MaturityRating? _oldMaturityRating;
			readonly MaturityRating? _newMaturityRating;
		}

		class SetAreCharacterNamesEnabledUndoAction : IUndoRedoAction
		{
			public SetAreCharacterNamesEnabledUndoAction( LibrarySearchViewModel viewModel, Boolean oldEnabled, Boolean newEnabled, IReadOnlyList<String> oldValues, IReadOnlyList<String> newValues )
			{
				_viewModel = viewModel;
				_oldEnabled = oldEnabled;
				_newEnabled = newEnabled;
				_oldValues = oldValues;
				_newValues = newValues;
			}

			public void Undo()
			{
				_viewModel._areCharacterNamesEnabled = _oldEnabled;
				_viewModel._search.CharacterNames.Clear();
				_viewModel._search.CharacterNames.AddRange( _oldValues );
				_viewModel.InvokeAreCharacterNamesEnabledChanged();
			}

			public void Redo()
			{
				_viewModel._areCharacterNamesEnabled = _newEnabled;
				_viewModel._search.CharacterNames.Clear();
				_viewModel._search.CharacterNames.AddRange( _newValues );
				_viewModel.InvokeAreCharacterNamesEnabledChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldEnabled;
			readonly Boolean _newEnabled;
			readonly IReadOnlyList<String> _oldValues;
			readonly IReadOnlyList<String> _newValues;
		}

		class SetAreShipsEnabledUndoAction : IUndoRedoAction
		{
			public SetAreShipsEnabledUndoAction( LibrarySearchViewModel viewModel, Boolean oldEnabled, Boolean newEnabled, IReadOnlyList<String> oldValues, IReadOnlyList<String> newValues )
			{
				_viewModel = viewModel;
				_oldEnabled = oldEnabled;
				_newEnabled = newEnabled;
				_oldValues = oldValues;
				_newValues = newValues;
			}

			public void Undo()
			{
				_viewModel._areShipsEnabled = _oldEnabled;
				_viewModel._search.Ships.Clear();
				_viewModel._search.Ships.AddRange( _oldValues );
				_viewModel.InvokeAreShipsEnabledChanged();
			}

			public void Redo()
			{
				_viewModel._areShipsEnabled = _newEnabled;
				_viewModel._search.Ships.Clear();
				_viewModel._search.Ships.AddRange( _newValues );
				_viewModel.InvokeAreShipsEnabledChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldEnabled;
			readonly Boolean _newEnabled;
			readonly IReadOnlyList<String> _oldValues;
			readonly IReadOnlyList<String> _newValues;
		}

		class SetAreTagsEnabledUndoAction : IUndoRedoAction
		{
			public SetAreTagsEnabledUndoAction( LibrarySearchViewModel viewModel, Boolean oldEnabled, Boolean newEnabled, IReadOnlyList<String> oldValues, IReadOnlyList<String> newValues )
			{
				_viewModel = viewModel;
				_oldEnabled = oldEnabled;
				_newEnabled = newEnabled;
				_oldValues = oldValues;
				_newValues = newValues;
			}

			public void Undo()
			{
				_viewModel._areTagsEnabled = _oldEnabled;
				_viewModel._search.Tags.Clear();
				_viewModel._search.Tags.AddRange( _oldValues );
				_viewModel.InvokeAreTagsEnabledChanged();
			}

			public void Redo()
			{
				_viewModel._areTagsEnabled = _newEnabled;
				_viewModel._search.Tags.Clear();
				_viewModel._search.Tags.AddRange( _newValues );
				_viewModel.InvokeAreTagsEnabledChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldEnabled;
			readonly Boolean _newEnabled;
			readonly IReadOnlyList<String> _oldValues;
			readonly IReadOnlyList<String> _newValues;
		}

		class SetIsNumberLikesEnabledUndoAction : IUndoRedoAction
		{
			public SetIsNumberLikesEnabledUndoAction( LibrarySearchViewModel viewModel, Boolean oldEnabled, Boolean newEnabled, NumberSearchCriteria oldCount, NumberSearchCriteria newCount )
			{
				_viewModel = viewModel;
				_oldEnabled = oldEnabled;
				_newEnabled = newEnabled;
				_oldCount = oldCount;
				_newCount = newCount;
			}

			public void Undo()
			{
				_viewModel._isNumberLikesEnabled = _oldEnabled;
				_viewModel._search.NumberLikes = _oldCount;
				_viewModel.InvokeIsNumberLikesEnabledChanged();
			}

			public void Redo()
			{
				_viewModel._isNumberLikesEnabled = _newEnabled;
				_viewModel._search.NumberLikes = _newCount;
				_viewModel.InvokeIsNumberLikesEnabledChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldEnabled;
			readonly Boolean _newEnabled;
			readonly NumberSearchCriteria _oldCount;
			readonly NumberSearchCriteria _newCount;
		}

		class SetIsNumberCommentsEnabledUndoAction : IUndoRedoAction
		{
			public SetIsNumberCommentsEnabledUndoAction( LibrarySearchViewModel viewModel, Boolean oldEnabled, Boolean newEnabled, NumberSearchCriteria oldCount, NumberSearchCriteria newCount )
			{
				_viewModel = viewModel;
				_oldEnabled = oldEnabled;
				_newEnabled = newEnabled;
				_oldCount = oldCount;
				_newCount = newCount;
			}

			public void Undo()
			{
				_viewModel._isNumberCommentsEnabled = _oldEnabled;
				_viewModel._search.NumberComments = _oldCount;
				_viewModel.InvokeIsNumberCommentsEnabledChanged();
			}

			public void Redo()
			{
				_viewModel._isNumberCommentsEnabled = _newEnabled;
				_viewModel._search.NumberComments = _newCount;
				_viewModel.InvokeIsNumberCommentsEnabledChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Boolean _oldEnabled;
			readonly Boolean _newEnabled;
			readonly NumberSearchCriteria _oldCount;
			readonly NumberSearchCriteria _newCount;
		}
	}
}
