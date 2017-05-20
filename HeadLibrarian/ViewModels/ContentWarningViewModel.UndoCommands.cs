using System;
using Alexandria.Model;
using Alexandria.Searching;

namespace HeadLibrarian.ViewModels
{
	public partial class ContentWarningViewModel
	{
		class SetContentWarningUndoAction : IUndoRedoAction
		{
			public SetContentWarningUndoAction( LibrarySearch search, ContentWarnings oldFlags, ContentWarnings newFlags,
				Action invokeChangedAction )
			{
				_search = search;
				_oldFlags = oldFlags;
				_newFlags = newFlags;
				_invokeChangedAction = invokeChangedAction;
			}

			public void Undo()
			{
				_search.ContentWarnings = _oldFlags;
				_invokeChangedAction();
			}

			public void Redo()
			{
				_search.ContentWarnings = _newFlags;
				_invokeChangedAction();
			}

			readonly LibrarySearch _search;
			readonly ContentWarnings _oldFlags;
			readonly ContentWarnings _newFlags;
			readonly Action _invokeChangedAction;
		}
	}
}
