using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Bibliothecary.Data;
using HeadLibrarian.WPF;

namespace HeadLibrarian.ViewModels
{
	internal sealed class MainWindowViewModel : BaseViewModel
	{
		public MainWindowViewModel()
		{
			ConnectToDatabaseCommand = new Command( CanExecuteConnectToDatabase, CommandConnectToDatabase );
			CreateProjectCommand = new Command( null, CommandCreateProject );
		}

		#region Database

		public Boolean IsConnectedToDatabase
		{
			get => _isConnectedToDatabase;
			private set => SetProperty( ref _isConnectedToDatabase, value );
		}

		public ICommand ConnectToDatabaseCommand { get; }

		Boolean CanExecuteConnectToDatabase( Object o )
		{
			if ( IsConnectedToDatabase )
			{
				return false;
			}

			return true;
		}

		void CommandConnectToDatabase( Object o )
		{
			_database = Database.Open( Constants.DatabaseFilename );
			if ( _database != null )
			{
				IsConnectedToDatabase = true;
				foreach ( Project project in _database.GetAllProjectIds().Select( _database.GetProject ) )
				{
					Projects.Add( new ProjectViewModel( _database, project ) );
				}
			}
			else
			{
				IsConnectedToDatabase = false;
				Projects.Clear();
			}
		}

		#endregion

		public ObservableCollection<ProjectViewModel> Projects { get; } = new ObservableCollection<ProjectViewModel>();

		public ICommand CreateProjectCommand { get; }

		void CommandCreateProject( Object o )
		{
			Project newProject = _database.CreateNewProject();
			Projects.Add( new ProjectViewModel( _database, newProject ) );
		}

		Boolean _isConnectedToDatabase;
		Database _database;
	}
}
