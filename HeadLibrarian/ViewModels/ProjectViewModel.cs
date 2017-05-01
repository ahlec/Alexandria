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
				if ( Project.SetName( value ) )
				{
					OnPropertyChanged( nameof( Name ) );
					OnPropertyChanged( nameof( HasUnsavedChanges ) );
				}
			}
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
	}
}
