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

		public SearchCriteriaViewModelType Type
		{
			get
			{
				if ( Search.Date == null )
				{
					return SearchCriteriaViewModelType.None;
				}

				return SearchCriteriaViewModelTypeUtils.FromAlexandriaDateType( Search.Date.Type );
			}
			set
			{
				SearchCriteriaViewModelType oldType = Type;
				if ( oldType == value )
				{
					return;
				}

				// Handle nulls
				if ( value == SearchCriteriaViewModelType.None )
				{
					DateSearchCriteria oldActualObject = Search.Date;
					Search.Date = null;
					_projectViewModel.UndoStack.Push( new SetActualObjectUndoAction( this, oldActualObject, null ) );
					InvokeActualObjectChanged();
					return;
				}

				DateSearchCriteriaType newType = SearchCriteriaViewModelTypeUtils.ToAlexandriaDateType( value );

				if ( oldType == SearchCriteriaViewModelType.None )
				{
					Search.Date = new DateSearchCriteria
					{
						Type = newType
					};
					_projectViewModel.UndoStack.Push( new SetActualObjectUndoAction( this, null, Search.Date ) );
					InvokeActualObjectChanged();
					return;
				}

				// Otherwise
				DateSearchCriteriaType oldActualType = Search.Date.Type;
				Search.Date.Type = newType;
				_projectViewModel.UndoStack.Push( new SetTypeUndoAction( this, oldActualType, newType ) );
				InvokeTypeChanged();
			}
		}

		public Boolean HasType => ( Type != SearchCriteriaViewModelType.None );

		void InvokeActualObjectChanged()
		{
			OnPropertyChanged( nameof( Type ) );
			OnPropertyChanged( nameof( HasType ) );
			OnPropertyChanged( nameof( DateUnit ) );
			OnPropertyChanged( nameof( Number1 ) );
			OnPropertyChanged( nameof( Number2 ) );
			_projectViewModel.RefreshHasSavedChanged();
		}

		void InvokeTypeChanged()
		{
			OnPropertyChanged( nameof( Type ) );
			OnPropertyChanged( nameof( HasType ) );
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

		protected readonly LibrarySearch Search;
		readonly ProjectViewModel _projectViewModel;
	}
}
