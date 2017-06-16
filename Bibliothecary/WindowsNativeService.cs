using System;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;
using System.Timers;
using Bibliothecary.Core;
using Bibliothecary.Extensions;

namespace Bibliothecary
{
	[DesignerCategory( "" )]
	public class WindowsNativeService : ServiceBase
	{
		public WindowsNativeService()
		{
			_log = new EventLog();
			if ( !EventLog.SourceExists( Constants.ServiceEventLogSource ) )
			{
				EventLog.CreateEventSource( Constants.ServiceEventLogSource, Constants.WindowsNativeServiceEventLogName );
			}
			_log.Source = Constants.ServiceEventLogSource;
			_log.Log = Constants.WindowsNativeServiceEventLogName;

			_timer = new Timer( PollingTimeMilliseconds );
			_timer.Elapsed += OnPollingTimerElapsed;

			_bibliothecary = new BibliothecaryService();
		}

		protected override void OnStart( String[] args )
		{
			try
			{
				_serviceHost = new ServiceHost( _bibliothecary, Constants.HttpServiceAddress );
				_serviceHost.Open();
				_timer.Start();
			}
			catch ( Exception ex )
			{
				_log.WriteException( ex );
			}
		}

		protected override void OnStop()
		{
			try
			{
				_timer.Stop();
				_serviceHost.Close();
			}
			catch ( Exception ex )
			{
				_log.WriteException( ex );
			}
		}

		void OnPollingTimerElapsed( Object sender, ElapsedEventArgs e )
		{
			try
			{
				_bibliothecary.MarkTimeElapsed( PollingTimeMilliseconds );
			}
			catch ( Exception ex )
			{
				_log.WriteException( ex );
			}
		}

		const Double PollingTimeMilliseconds = 10 * 60 * 1000; // 10 minutes
		readonly EventLog _log;
		readonly Timer _timer;
		readonly BibliothecaryService _bibliothecary;
		ServiceHost _serviceHost;
	}
}
