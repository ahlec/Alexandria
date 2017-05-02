using System;
using Bibliothecary.Data;

namespace HeadLibrarian.ViewModels
{
	public sealed class ProjectViewModel : BaseViewModel
	{
		public ProjectViewModel( Database database, Project project )
		{
			_database = database;
			Project = project;
			SearchQuery = new LibrarySearchViewModel( project.SearchQuery );
		}

		public Project Project { get; }

		public Int32 ProjectId => Project.ProjectId;

		public Boolean HasUnsavedChanges => Project.HasUnsavedChanges;

		public String Name
		{
			get => Project.Name;
			set
			{
				String oldName = Name;
				if ( Project.SetName( value ) )
				{
					UndoRedoStack.Push( new SetNameUndoAction( this, oldName, Name ) );
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
			get => Project.UpdateFrequency;
			set
			{
				if ( Project.SetUpdateFrequency( (Int32) value.TotalMinutes ) )
				{
					OnPropertyChanged( nameof( UpdateFrequency ) );
					OnPropertyChanged( nameof( HasUnsavedChanges ) );
				}
			}
		}

		public LibrarySearchViewModel SearchQuery { get; }

		readonly Database _database;

		class SetNameUndoAction : IUndoRedoAction
		{
			public SetNameUndoAction( ProjectViewModel project, String oldName, String newName )
			{
				_project = project;
				_oldName = oldName;
				_newName = newName;
			}

			public void Undo()
			{
				_project.Project.SetName( _oldName );
				_project.InvokeNameChanged();
			}

			public void Redo()
			{
				_project.Project.SetName( _newName );
				_project.InvokeNameChanged();
			}

			readonly ProjectViewModel _project;
			readonly String _oldName;
			readonly String _newName;
		}
	}
}
