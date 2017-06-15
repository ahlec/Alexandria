using System;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;
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
		}

		protected override void OnStart( String[] args )
		{
			try
			{
				_serviceHost = new ServiceHost( typeof( BibliothecaryService ), Constants.HttpServiceAddress );
				_serviceHost.Open();
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
				_serviceHost.Close();
			}
			catch ( Exception ex )
			{
				_log.WriteException( ex );
			}
		}

		readonly EventLog _log;
		ServiceHost _serviceHost;
	}
}
