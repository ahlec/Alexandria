using System;
using Alexandria.Searching;

namespace HeadLibrarian.ViewModels
{
	public partial class DateSearchCriteriaViewModel : BaseViewModel
	{
		public DateSearchCriteriaViewModel( ProjectViewModel projectViewModel, LibrarySearch search )
		{
			_projectViewModel = projectViewModel;
			Search = search;
		}

		public DateSearchCriteriaType Type
		{
			get => Search.Date?.Type ?? DateSearchCriteriaType.Exactly;
			set
			{
				DateSearchCriteriaType oldType = Type;
				if ( Search.Date != null && oldType == value )
				{
					return;
				}

				if ( Search.Date == null )
				{
					Search.Date = new DateSearchCriteria
					{
						Type = value
					};
					_projectViewModel.UndoStack.Push( new SetActualObjectUndoAction( this, null, Search.Date ) );
					InvokeAllPropertiesChanged();
					return;
				}

				// Otherwise
				Search.Date.Type = value;
				_projectViewModel.UndoStack.Push( new SetTypeUndoAction( this, oldType, value ) );
				InvokeTypeChanged();
			}
		}

		internal void InvokeAllPropertiesChanged()
		{
			OnPropertyChanged( nameof( Type ) );
			OnPropertyChanged( nameof( DateUnit ) );
			OnPropertyChanged( nameof( Number1 ) );
			OnPropertyChanged( nameof( Number2 ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		void InvokeTypeChanged()
		{
			OnPropertyChanged( nameof( Type ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public DateField DateUnit
		{
			get => Search.Date?.DateUnit ?? default( DateField );
			set
			{
				if ( Search.Date == null )
				{
					throw new InvalidOperationException();
				}

				DateField oldDateUnit = Search.Date.DateUnit;
				if ( oldDateUnit == value )
				{
					return;
				}

				Search.Date.DateUnit = value;
				_projectViewModel.UndoStack.Push( new SetDateUnitUndoAction( this, oldDateUnit, value ) );
				InvokeDateUnitChanged();
			}
		}

		void InvokeDateUnitChanged()
		{
			OnPropertyChanged( nameof( DateUnit ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public Int32 Number1
		{
			get => Search.Date?.Number1 ?? 0;
			set
			{
				if ( Search.Date == null )
				{
					throw new InvalidOperationException();
				}

				Int32 oldNumber = Search.Date.Number1;
				if ( oldNumber == value )
				{
					return;
				}

				Search.Date.Number1 = value;
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
			get => Search.Date?.Number2 ?? 0;
			set
			{
				if ( Search.Date == null )
				{
					throw new InvalidOperationException();
				}

				Int32 oldNumber = Search.Date.Number2;
				if ( oldNumber == value )
				{
					return;
				}

				Search.Date.Number2 = value;
				_projectViewModel.UndoStack.Push( new SetNumber2UndoAction( this, oldNumber, value ) );
				InvokeNumber2Changed();
			}
		}

		void InvokeNumber2Changed()
		{
			OnPropertyChanged( nameof( Number2 ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		public DateSearchCriteriaType[] AllValidDateTypes { get; } =
		{
			DateSearchCriteriaType.Exactly,
			DateSearchCriteriaType.Before,
			DateSearchCriteriaType.After,
			DateSearchCriteriaType.Between
		};

		protected readonly LibrarySearch Search;
		readonly ProjectViewModel _projectViewModel;
	}
}
