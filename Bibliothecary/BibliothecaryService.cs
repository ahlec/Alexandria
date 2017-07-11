using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Alexandria;
using Alexandria.AO3;
using Bibliothecary.Core;
using NLog;

namespace Bibliothecary
{
	[ServiceBehavior( InstanceContextMode = InstanceContextMode.Single )]
	public sealed class BibliothecaryService : IBibliothecaryService
	{
		public BibliothecaryService()
		{
			_database = Database.Open( Constants.DatabaseFilename );
			RefreshProjects();
		}

		public void RestartProjectTimer()
		{
			lock ( _projectsLock )
			{
				_log.Info( $"Resetting Project Timers ({nameof( RestartProjectTimer )})" );
				try
				{
					foreach ( LiveProject project in _projects )
					{
						project.ResetCountdown();
						_log.Info( $"- {project.Project.Name} ( Project #{project.Project.ProjectId})" );
					}
				}
				catch ( Exception ex )
				{
					_log.Error( ex );
				}
			}
		}

		public void RefreshProjects()
		{
			lock ( _projectsLock )
			{
				_log.Info( $"Refreshing Project Knowledge ({nameof( RefreshProjects )})" );
				try
				{
					Dictionary<Int32, LiveProject> snapshot = _projects.ToDictionary( project => project.Project.ProjectId );
					_projects.Clear();
					foreach ( Int32 projectId in _database.GetAllProjectIds() )
					{
						LiveProject project;
						if ( !snapshot.TryGetValue( projectId, out project ) )
						{
							project = new LiveProject( Project.Read( _database, projectId ) );
							_log.Info( $"- Added {project.Project.Name} ( Project #{project.Project.ProjectId} )" );
						}
						_projects.Add( project );
					}
					IEnumerable<Int32> removedProjectIds = snapshot.Keys.Except( _projects.Select( project => project.Project.ProjectId ) );
					foreach ( Int32 projectId in removedProjectIds )
					{
						LiveProject project = snapshot[projectId];
						_log.Info( $"- Removed {project.Project.Name} ( Project #{project.Project.ProjectId} )" );
					}
					if ( _projects.Count > 0 )
					{
						_log.Info( $"There are currently {_projects.Count} project(s) currently active." );
					}
					else
					{
						_log.Warn( "There are no active projects that Bibliothecary is tracking." );
					}
				}
				catch ( Exception ex )
				{
					_log.Error( ex );
				}
			}
		}

		public void AggregateProjectsNow()
		{
			lock ( _projectsLock )
			{
				_log.Info( $"Processing projects on demand ({nameof( AggregateProjectsNow )})" );
				try
				{
					foreach ( LiveProject project in _projects )
					{
						ProcessProject( project.Project, 1 );
						project.ResetCountdown();
					}
				}
				catch ( Exception ex )
				{
					_log.Error( ex );
				}
			}
		}

		public void MarkTimeElapsed( TimeSpan time )
		{
			lock ( _projectsLock )
			{
				_log.Info( $"Marking {time} has elapsed ({nameof( MarkTimeElapsed )})" );
				try
				{
					foreach ( LiveProject project in _projects )
					{
						String projectPrefix = $"- {project.Project.Name} ( Project #{project.Project.ProjectId} )";
						project.Update( time );
						if ( project.IsTimeToSearch )
						{
							String projectSuffix = null;
							if ( project.NumberOfTimesToSearch > 1 )
							{
								projectSuffix = $" (x{project.NumberOfTimesToSearch} the normal number of max results to make up for missed collections)";
							}
							_log.Info( $"{projectPrefix}: searching now{projectSuffix}" );
							ProcessProject( project.Project, project.NumberOfTimesToSearch );
						}
						else
						{
							_log.Info( $"{projectPrefix}: ready to search at {project.NextSearchTime}" );
						}
					}
				}
				catch ( Exception ex )
				{
					_log.Error( ex );
				}
			}
		}

		void ProcessProject( Project project, Int32 maxResultsMultipler )
		{
			if ( project.SearchAO3 )
			{
				_ao3.ProcessProject( project, _database, _log, maxResultsMultipler );
			}
		}

		static readonly Logger _log = LogManager.GetCurrentClassLogger();
		readonly Database _database;
		readonly Object _projectsLock = new Object();
		readonly List<LiveProject> _projects = new List<LiveProject>();
		readonly BibliothecarySource _ao3 = new BibliothecarySource( new AO3Source( LibrarySourceConfig.Default) );
	}
}
