using System;
using Alexandria.Model;
using Alexandria.Searching;

namespace HeadLibrarian.ViewModels
{
	public partial class ContentWarningViewModel : BaseViewModel
	{
		public ContentWarningViewModel( ProjectViewModel projectViewModel, LibrarySearch search )
		{
			_projectViewModel = projectViewModel;
			Search = search;
		}

		public Boolean HasUndetermined
		{
			get => Search.ContentWarnings.HasFlag( ContentWarnings.Undetermined );
			set
			{
				ContentWarnings oldFlags = Search.ContentWarnings;
				ContentWarnings newFlags = ApplyBoolean( ContentWarnings.Undetermined, value );
				if ( oldFlags == newFlags )
				{
					return;
				}

				Search.ContentWarnings = newFlags;
				_projectViewModel.UndoStack.Push( new SetContentWarningUndoAction( Search, oldFlags, newFlags, InvokeHasUndeterminedChanged ) );
				InvokeHasUndeterminedChanged();
			}
		}

		void InvokeHasUndeterminedChanged()
		{
			OnPropertyChanged( nameof( HasUndetermined ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean HasViolence
		{
			get => Search.ContentWarnings.HasFlag( ContentWarnings.Violence );
			set
			{
				ContentWarnings oldFlags = Search.ContentWarnings;
				ContentWarnings newFlags = ApplyBoolean( ContentWarnings.Violence, value );
				if ( oldFlags == newFlags )
				{
					return;
				}

				Search.ContentWarnings = newFlags;
				_projectViewModel.UndoStack.Push( new SetContentWarningUndoAction( Search, oldFlags, newFlags, InvokeHasViolenceChanged ) );
				InvokeHasViolenceChanged();
			}
		}

		void InvokeHasViolenceChanged()
		{
			OnPropertyChanged( nameof( HasViolence ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean HasMajorCharacterDeath
		{
			get => Search.ContentWarnings.HasFlag( ContentWarnings.MajorCharacterDeath );
			set
			{
				ContentWarnings oldFlags = Search.ContentWarnings;
				ContentWarnings newFlags = ApplyBoolean( ContentWarnings.MajorCharacterDeath, value );
				if ( oldFlags == newFlags )
				{
					return;
				}

				Search.ContentWarnings = newFlags;
				_projectViewModel.UndoStack.Push( new SetContentWarningUndoAction( Search, oldFlags, newFlags, InvokeHasMajorCharacterDeathChanged ) );
				InvokeHasMajorCharacterDeathChanged();
			}
		}

		void InvokeHasMajorCharacterDeathChanged()
		{
			OnPropertyChanged( nameof( HasMajorCharacterDeath ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean HasRape
		{
			get => Search.ContentWarnings.HasFlag( ContentWarnings.Rape );
			set
			{
				ContentWarnings oldFlags = Search.ContentWarnings;
				ContentWarnings newFlags = ApplyBoolean( ContentWarnings.Rape, value );
				if ( oldFlags == newFlags )
				{
					return;
				}

				Search.ContentWarnings = newFlags;
				_projectViewModel.UndoStack.Push( new SetContentWarningUndoAction( Search, oldFlags, newFlags, InvokeHasRapeChanged ) );
				InvokeHasRapeChanged();
			}
		}

		void InvokeHasRapeChanged()
		{
			OnPropertyChanged( nameof( HasRape ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Boolean HasUnderage
		{
			get => Search.ContentWarnings.HasFlag( ContentWarnings.Underage );
			set
			{
				ContentWarnings oldFlags = Search.ContentWarnings;
				ContentWarnings newFlags = ApplyBoolean( ContentWarnings.Underage, value );
				if ( oldFlags == newFlags )
				{
					return;
				}

				Search.ContentWarnings = newFlags;
				_projectViewModel.UndoStack.Push( new SetContentWarningUndoAction( Search, oldFlags, newFlags, InvokeHasUnderageChanged ) );
				InvokeHasUnderageChanged();
			}
		}

		void InvokeHasUnderageChanged()
		{
			OnPropertyChanged( nameof( HasUnderage ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		ContentWarnings ApplyBoolean( ContentWarnings flag, Boolean value )
		{
			ContentWarnings current = Search.ContentWarnings;

			if ( value )
			{
				return ( current | flag );
			}

			return ( current & ~flag );
		}

		protected readonly LibrarySearch Search;
		readonly ProjectViewModel _projectViewModel;
	}
}
