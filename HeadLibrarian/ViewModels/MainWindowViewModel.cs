using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			set => SetProperty( ref _selectedProject, value );
		}

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
			ProjectViewModel project = ( o as ProjectViewModel );
			if ( project == null )
			{
				throw new ArgumentException( nameof( o ) );
			}

			ConfirmProjectDeletionDialog confirmDialog = new ConfirmProjectDeletionDialog( project );
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

		readonly Object _projectsCollectionLock = new Object();
		Boolean _isConnectedToDatabase;
		Boolean _isAttemptingConnectionToDatabase;
		Database _database;
		ProjectViewModel _selectedProject;
	}
}
