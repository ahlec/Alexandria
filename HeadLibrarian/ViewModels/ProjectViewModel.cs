using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bibliothecary.Data;

namespace HeadLibrarian.ViewModels
{
	internal sealed class ProjectViewModel : BaseViewModel
	{
		public ProjectViewModel( Database database, Project project )
		{
			_database = database;
			_project = project;
			SearchQuery = new LibrarySearchViewModel( project.SearchQuery );
		}

		public String Name
		{
			get => _project.Name;
			set
			{
				if ( _project.SetName( value ) )
				{
					OnPropertyChanged( nameof( Name ) );
				}
			}
		}

		public TimeSpan UpdateFrequency
		{
			get => _project.UpdateFrequency;
			set
			{
				if ( _project.SetUpdateFrequency( (Int32) value.TotalMinutes ) )
				{
					OnPropertyChanged( nameof( UpdateFrequency ) );
				}
			}
		}

		public LibrarySearchViewModel SearchQuery { get; }

		readonly Database _database;
		readonly Project _project;
	}
}
