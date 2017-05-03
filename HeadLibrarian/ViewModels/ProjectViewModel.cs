using System;
using System.Windows.Input;
using Bibliothecary.Data;
using HeadLibrarian.WPF;

namespace HeadLibrarian.ViewModels
{
	public sealed class ProjectViewModel : BaseViewModel
	{
		public ProjectViewModel( Database database, Project project )
		{
			UndoStack = new UndoRedoStack();
			UndoStack.UndoPerformed += OnUndoRedo;
			UndoStack.RedoPerformed += OnUndoRedo;
			UndoCommand = new Command( null, CommandUndo );
			RedoCommand = new Command( null, CommandRedo );

			_database = database;
			_project = project;
			SearchQuery = new LibrarySearchViewModel( project.SearchQuery );

			SaveCommand = new Command( null, CommandSave );
		}

		#region UndoStack

		public UndoRedoStack UndoStack { get; }

		public Boolean CanUndo
		{
			get => _canUndo;
			private set => SetProperty( ref _canUndo, value );
		}

		public Boolean CanRedo
		{
			get => _canRedo;
			private set => SetProperty( ref _canRedo, value );
		}

		void OnUndoRedo( Int32 undoStackSize, Int32 redoStackSize )
		{
			CanUndo = ( undoStackSize > 0 );
			CanRedo = ( redoStackSize > 0 );
		}

		public ICommand UndoCommand { get; }

		void CommandUndo( Object o )
		{
			UndoStack.Undo();
		}

		public ICommand RedoCommand { get; }

		void CommandRedo( Object o )
		{
			UndoStack.Redo();
		}

		#endregion

		public Int32 ProjectId => _project.ProjectId;

		public Boolean HasUnsavedChanges => _project.HasUnsavedChanges;

		public String Name
		{
			get => _project.Name;
			set
			{
				String oldName = Name;
				if ( _project.SetName( value ) )
				{
					UndoStack.Push( new SetNameUndoAction( this, oldName, Name ) );
					InvokeNameChanged();
				}
			}
		}

		void InvokeNameChanged()
		{
			OnPropertyChanged( nameof( Name ) );
			OnPropertyChanged( nameof( HasUnsavedChanges ) );
		}

		public TimeSpan UpdateFrequency
		{
			get => _project.UpdateFrequency;
			set
			{
				if ( _project.SetUpdateFrequency( (Int32) value.TotalMinutes ) )
				{
					OnPropertyChanged( nameof( UpdateFrequency ) );
					OnPropertyChanged( nameof( HasUnsavedChanges ) );
				}
			}
		}

		public LibrarySearchViewModel SearchQuery { get; }

		public ICommand SaveCommand { get; }

		void CommandSave( Object o )
		{
			_project.Save();
			OnPropertyChanged( nameof( HasUnsavedChanges ) );
		}

		public Boolean Delete()
		{
			return _project.Delete();
		}

		readonly Database _database;
		readonly Project _project;
		Boolean _canUndo;
		Boolean _canRedo;

		#region Undo Commands

		class SetNameUndoAction : IUndoRedoAction
		{
			public SetNameUndoAction( ProjectViewModel viewModel, String oldName, String newName )
			{
				_viewModel = viewModel;
				_oldName = oldName;
				_newName = newName;
			}

			public void Undo()
			{
				_viewModel._project.SetName( _oldName );
				_viewModel.InvokeNameChanged();
			}

			public void Redo()
			{
				_viewModel._project.SetName( _newName );
				_viewModel.InvokeNameChanged();
			}

			readonly ProjectViewModel _viewModel;
			readonly String _oldName;
			readonly String _newName;
		}

		#endregion
	}
}
