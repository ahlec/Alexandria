using System;
using Alexandria.Searching;

namespace HeadLibrarian.ViewModels
{
	public abstract partial class NumberSearchCriteriaViewModel : BaseViewModel
	{
		protected NumberSearchCriteriaViewModel( ProjectViewModel projectViewModel, LibrarySearch search )
		{
			_projectViewModel = projectViewModel;
			Search = search;
		}

		public NumberSearchCriteriaType Type
		{
			get => ActualObject?.Type ?? NumberSearchCriteriaType.ExactMatch;
			set
			{
				NumberSearchCriteriaType oldType = Type;
				if ( ActualObject != null && oldType == value )
				{
					return;
				}

				if ( ActualObject == null )
				{
					ActualObject = new NumberSearchCriteria
					{
						Type = value
					};
					_projectViewModel.UndoStack.Push( new SetActualObjectUndoAction( this, null, ActualObject ) );
					InvokeAllPropertiesChanged();
					return;
				}

				// Otherwise
				NumberSearchCriteriaType oldActualType = ActualObject.Type;
				ActualObject.Type = value;
				_projectViewModel.UndoStack.Push( new SetTypeUndoAction( this, oldActualType, value ) );
				InvokeTypeChanged();
			}
		}

		internal void InvokeAllPropertiesChanged()
		{
			OnPropertyChanged( nameof( Type ) );
			OnPropertyChanged( nameof( Number1 ) );
			OnPropertyChanged( nameof( Number2 ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		void InvokeTypeChanged()
		{
			OnPropertyChanged( nameof( Type ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Int32 Number1
		{
			get => ActualObject?.Number1 ?? 0;
			set
			{
				if ( ActualObject == null )
				{
					throw new InvalidOperationException();
				}

				Int32 oldNumber = ActualObject.Number1;
				if ( oldNumber == value )
				{
					return;
				}

				ActualObject.Number1 = value;
				_projectViewModel.UndoStack.Push( new SetNumber1UndoAction( this, oldNumber, value ) );
				InvokeNumber1Changed();
			}
		}

		void InvokeNumber1Changed()
		{
			OnPropertyChanged( nameof( Number1 ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Int32 Number2
		{
			get => ActualObject?.Number2 ?? 0;
			set
			{
				if ( ActualObject == null )
				{
					throw new InvalidOperationException();
				}

				Int32 oldNumber = ActualObject.Number2;
				if ( oldNumber == value )
				{
					return;
				}

				ActualObject.Number2 = value;
				_projectViewModel.UndoStack.Push( new SetNumber2UndoAction( this, oldNumber, value ) );
				InvokeNumber2Changed();
			}
		}

		void InvokeNumber2Changed()
		{
			OnPropertyChanged( nameof( Number2 ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		protected abstract NumberSearchCriteria ActualObject { get; set; }

		public NumberSearchCriteriaType[] AllValidNumberTypes { get; } =
		{
			NumberSearchCriteriaType.ExactMatch,
			NumberSearchCriteriaType.LessThan,
			NumberSearchCriteriaType.GreaterThan,
			NumberSearchCriteriaType.Range
		};

		protected readonly LibrarySearch Search;
		readonly ProjectViewModel _projectViewModel;
	}
}
