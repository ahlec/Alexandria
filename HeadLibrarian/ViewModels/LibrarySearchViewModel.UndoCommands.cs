using System;
using Alexandria.Model;

namespace HeadLibrarian.ViewModels
{
	public sealed partial class LibrarySearchViewModel
	{
		class SetTitleUndoAction : IUndoRedoAction
		{
			public SetTitleUndoAction( LibrarySearchViewModel viewModel, String oldName, String newName )
			{
				_viewModel = viewModel;
				_oldName = oldName;
				_newName = newName;
			}

			public void Undo()
			{
				_viewModel._search.Title = _oldName;
				_viewModel.InvokeTitleChanged();
			}

			public void Redo()
			{
				_viewModel._search.Title = _newName;
				_viewModel.InvokeTitleChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly String _oldName;
			readonly String _newName;
		}

		class SetAuthorUndoAction : IUndoRedoAction
		{
			public SetAuthorUndoAction( LibrarySearchViewModel viewModel, String oldAuthor, String newAuthor )
			{
				_viewModel = viewModel;
				_oldAuthor = oldAuthor;
				_newAuthor = newAuthor;
			}

			public void Undo()
			{
				_viewModel._search.Author = _oldAuthor;
				_viewModel.InvokeAuthorChanged();
			}

			public void Redo()
			{
				_viewModel._search.Author = _newAuthor;
				_viewModel.InvokeAuthorChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly String _oldAuthor;
			readonly String _newAuthor;
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

		class SetLanguageUndoAction : IUndoRedoAction
		{
			public SetLanguageUndoAction( LibrarySearchViewModel viewModel, Language? oldLanguage, Language? newLanguage )
			{
				_viewModel = viewModel;
				_oldLanguage = oldLanguage;
				_newLanguage = newLanguage;
			}

			public void Undo()
			{
				_viewModel._search.Language = _oldLanguage;
				_viewModel.InvokeLanguageChanged();
			}

			public void Redo()
			{
				_viewModel._search.Language = _newLanguage;
				_viewModel.InvokeLanguageChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly Language? _oldLanguage;
			readonly Language? _newLanguage;
		}

		class SetMaturityRatingUndoAction : IUndoRedoAction
		{
			public SetMaturityRatingUndoAction( LibrarySearchViewModel viewModel, MaturityRating? oldMaturityRating, MaturityRating? newMaturityRating )
			{
				_viewModel = viewModel;
				_oldMaturityRating = oldMaturityRating;
				_newMaturityRating = newMaturityRating;
			}

			public void Undo()
			{
				_viewModel._search.Rating = _oldMaturityRating;
				_viewModel.InvokeMaturityRatingChanged();
			}

			public void Redo()
			{
				_viewModel._search.Rating = _newMaturityRating;
				_viewModel.InvokeMaturityRatingChanged();
			}

			readonly LibrarySearchViewModel _viewModel;
			readonly MaturityRating? _oldMaturityRating;
			readonly MaturityRating? _newMaturityRating;
		}
	}
}
