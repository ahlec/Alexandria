using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using Alexandria;
using Alexandria.AO3;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Bibliothecary.Data;
using Bibliothecary.Extensions;

namespace Bibliothecary
{
	[DesignerCategory( "" )]
	public class Bibliothecary : ServiceBase
	{
		const String EventLogSource = "Bibliothecary";
		const String EventLogName = "Bibliothecary Log";
		const Double PollingTimeMilliseconds = 10 * 60 * 1000; // 10 minutes

		public Bibliothecary()
		{
			_log = new EventLog();
			if ( !EventLog.SourceExists( EventLogSource ) )
			{
				EventLog.CreateEventSource( EventLogSource, EventLogName );
			}
			_log.Source = EventLogSource;
			_log.Log = EventLogName;

			_timer = new Timer( PollingTimeMilliseconds );
			_timer.Elapsed += OnPollingTimerElapsed;
		}

		protected override void OnStart( String[] args )
		{
			_log.WriteEntry( "Starting Bibliothecary..." );

			try
			{
				_database = Database.Open( Constants.DatabaseFilename );
			}
			catch ( Exception ex )
			{
				_log.WriteException( ex );
				_database = null;
				_log.WriteEntry( "Shutting down due to exception while opening database." );
				return;
			}

			RepopulateProjects();
			lock ( _projects )
			{
				StringBuilder projectsMessage = new StringBuilder( "Loaded " );
				projectsMessage.Append( _projects.Count );
				projectsMessage.Append( " project" );
				if ( _projects.Count != 1 )
				{
					projectsMessage.Append( "s" );
				}
				if ( _projects.Count > 0 )
				{
					projectsMessage.Append( ": " );
					for ( Int32 index = 0; index < _projects.Count; ++index )
					{
						if ( index > 0 )
						{
							projectsMessage.Append( ", " );
						}
						projectsMessage.Append( "'" );
						projectsMessage.Append( _projects[index].Project.Name );
						projectsMessage.Append( "' (" );
						projectsMessage.Append( _projects[index].Project.ProjectId );
						projectsMessage.Append( ")" );
					}
				}
				_log.WriteEntry( projectsMessage.ToString() );
			}

			_timer.Start();
			_log.WriteEntry( $"Started! Polling every {_pollingTime}" );
		}

		protected override void OnStop()
		{
			_log.WriteEntry( "Stopping Bibliothecary" );
			_timer.Stop();
		}

		void OnPollingTimerElapsed( Object sender, ElapsedEventArgs e )
		{
			_log.WriteEntry( "Polling" );
			if ( _database == null )
			{
				_log.WriteEntry( $"Error! {nameof( _database )} is null!" );
				return;
			}

			foreach ( LiveProject project in _projects )
			{
				project.Update( _pollingTime );
				if ( project.IsTimeToSearch )
				{
					ProcessProject( project.Project );
					project.ResetCountdown();
				}
				else
				{
					_log.WriteEntry( $"Time left until project '{project.Project.Name}' is ready for updating: {project.TimeRemaining}" );
				}
			}
		}

		void RepopulateProjects()
		{
			lock ( _projectsLock )
			{
				_projects.Clear();
				foreach ( Int32 projectId in _database.GetAllProjectIds() )
				{
					_projects.Add( new LiveProject( Project.Read( _database, projectId ) ) );
				}
			}
		}

		void ProcessProject( Project project )
		{
			foreach ( BibliothecarySource source in GetLibrarySources( project ) )
			{
				source.ProcessProject( project, _database );
			}
		}

		IEnumerable<BibliothecarySource> GetLibrarySources( Project project )
		{
			if ( project.SearchAO3 )
			{
				yield return _ao3;
			}
		}

		static readonly TimeSpan _pollingTime = TimeSpan.FromMilliseconds( PollingTimeMilliseconds );
		readonly EventLog _log;
		readonly Timer _timer;
		readonly Object _projectsLock = new Object();
		readonly List<LiveProject> _projects = new List<LiveProject>();
		readonly BibliothecarySource _ao3 = new BibliothecarySource( new AO3Source( LibrarySourceConfig.Default ), "ao3" );
		Database _database;
	}
}
