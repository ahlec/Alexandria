using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Bibliothecary.Core;
using HeadLibrarian.Dialogs;
using HeadLibrarian.WPF;

namespace HeadLibrarian.ViewModels
{
	internal sealed class MainWindowViewModel : BaseViewModel
	{
		public MainWindowViewModel()
		{
			ConnectToDatabaseCommand = new Command( null, CommandConnectToDatabase );
			CreateProjectCommand = new Command( null, CommandCreateProject );
			DeleteProjectCommand = new Command( null, CommandDeleteProject );
			OpenLogsCommand = new Command( null, CommandOpenLogs );
			GoToAlecCommand = new Command( null, CommandGoToAlec );
			GoToVisualStudioImageLibraryCommand = new Command( null, CommandGoToVisualStudioImageLibrary );
			GoToOmmoZoubayrCommand = new Command( null, CommandGoToOmmoZoubayr );
			GoToRaindropmemoryCommand = new Command( null, CommandGoToRaindropmemory );
			ViewSourceCodeCommand = new Command( null, CommandViewSourceCode );

			BindingOperations.EnableCollectionSynchronization( Projects, _projectsCollectionLock );
		}

		#region Database

		public Boolean IsConnectedToDatabase
		{
			get => _isConnectedToDatabase;
			private set => SetProperty( ref _isConnectedToDatabase, value );
		}

		public Boolean IsAttemptingConnectionToDatabase
		{
			get => _isAttemptingConnectionToDatabase;
			private set => SetProperty( ref _isAttemptingConnectionToDatabase, value );
		}

		public ICommand ConnectToDatabaseCommand { get; }

		void CommandConnectToDatabase( Object o )
		{
			if ( IsAttemptingConnectionToDatabase || IsConnectedToDatabase )
			{
				return;
			}
			IsAttemptingConnectionToDatabase = true;

			new Thread( PerformDatabaseConnection ).Start();
		}

		void PerformDatabaseConnection()
		{
			_database = Database.Open( Constants.DatabaseFilename );
			if ( _database != null )
			{
				IEnumerable<Project> projects = _database.GetAllProjectIds().Select( projectId => Project.Read( _database, projectId ) );
				Application.Current.Dispatcher.Invoke( () => FinishDatabaseConnection( true, projects ) );
			}
			else
			{
				Application.Current.Dispatcher.Invoke( () => FinishDatabaseConnection( false, null ) );
			}
		}

		void FinishDatabaseConnection( Boolean isConnected, IEnumerable<Project> projects )
		{
			IsConnectedToDatabase = isConnected;
			lock ( _projectsCollectionLock )
			{
				Projects.Clear();
				if ( projects != null )
				{
					foreach ( Project project in projects )
					{
						Projects.Add( new ProjectViewModel( _database, project ) );
					}
				}
			}
			IsAttemptingConnectionToDatabase = false;
			SelectedProject = Projects.FirstOrDefault();
		}

		#endregion

		public ObservableCollection<ProjectViewModel> Projects { get; } = new ObservableCollection<ProjectViewModel>();

		public ProjectViewModel SelectedProject
		{
			get => _selectedProject;
			set
			{
				if ( SetProperty( ref _selectedProject, value ) )
				{
					OnPropertyChanged( nameof( IsProjectSelected ) );
				}
			}
		}

		public Boolean IsProjectSelected => ( SelectedProject != null );

		public ICommand CreateProjectCommand { get; }

		void CommandCreateProject( Object o )
		{
			Project newProject = Project.Create( _database );
			ProjectViewModel viewModel = new ProjectViewModel( _database, newProject );
			lock ( _projectsCollectionLock )
			{
				Projects.Add( viewModel );
			}
			SelectedProject = viewModel;
		}

		public ICommand DeleteProjectCommand { get; }

		void CommandDeleteProject( Object o )
		{
			Window mainWindow = ( o as Window );
			if ( mainWindow == null )
			{
				throw new ArgumentNullException( nameof( mainWindow ) );
			}

			ProjectViewModel project = SelectedProject;
			if ( project == null )
			{
				return;
			}

			ConfirmProjectDeletionDialog confirmDialog = new ConfirmProjectDeletionDialog( project )
			{
				Owner = mainWindow
			};
			confirmDialog.ShowDialog();
			if ( confirmDialog.DialogResult != true )
			{
				return;
			}

			if ( !IsConnectedToDatabase )
			{
				throw new InvalidOperationException();
			}

			Boolean deleteSuccess = project.Delete();
			if ( !deleteSuccess )
			{
				return;
			}

			lock ( _projectsCollectionLock )
			{
				Projects.Remove( project );
			}

			ProjectDelete?.Invoke( this, project );
		}

		public event EventHandler<ProjectViewModel> ProjectDelete;

		public ICommand OpenLogsCommand { get; }

		static void CommandOpenLogs( Object o )
		{
			Process.Start( Constants.LogFilename );
		}

		public ICommand GoToAlecCommand { get; }

		static void CommandGoToAlec( Object o )
		{
			Process.Start( @"http://alec.deitloff.com/" );
		}

		public ICommand GoToVisualStudioImageLibraryCommand { get; }

		static void CommandGoToVisualStudioImageLibrary( Object o )
		{
			Process.Start( @"https://www.microsoft.com/en-us/download/details.aspx?id=35825" );
		}

		public ICommand GoToOmmoZoubayrCommand { get; }

		static void CommandGoToOmmoZoubayr( Object o )
		{
			Process.Start( @"https://www.iconfinder.com/iconsets/free-basic" );
		}

		public ICommand GoToRaindropmemoryCommand { get; }

		static void CommandGoToRaindropmemory( Object o )
		{
			Process.Start( @"https://www.iconfinder.com/icons/88895/library_icon#size=128" );
		}

		public ICommand ViewSourceCodeCommand { get; }

		static void CommandViewSourceCode( Object o )
		{
			Process.Start( @"https://bitbucket.org/ahlec/alexandria/src" );
		}

		readonly Object _projectsCollectionLock = new Object();
		Boolean _isConnectedToDatabase;
		Boolean _isAttemptingConnectionToDatabase;
		Database _database;
		ProjectViewModel _selectedProject;
	}
}
