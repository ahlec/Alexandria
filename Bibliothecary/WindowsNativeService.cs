using System;
using System.ComponentModel;
using System.ServiceModel;
using System.ServiceProcess;
using System.Timers;
using Bibliothecary.Core;
using NLog;

namespace Bibliothecary
{
	[DesignerCategory( "" )]
	public class WindowsNativeService : ServiceBase
	{
		public WindowsNativeService()
		{
			_timer = new Timer( PollingTimeMilliseconds );
			_timer.Elapsed += OnPollingTimerElapsed;

			_bibliothecary = new BibliothecaryService();
		}

		protected override void OnStart( String[] args )
		{
			try
			{
				_serviceHost = new ServiceHost( _bibliothecary, Constants.HttpServiceAddress );
				_serviceHost.AddServiceEndpoint( typeof( IBibliothecaryService ), new BasicHttpBinding(), Constants.ServiceEndpoint );
				_serviceHost.Open();
				_timer.Start();
				_log.Info( $"{nameof( WindowsNativeService )} has started successfully." );
				_log.Info( $"Bibliothecary polling is occurring on a {_pollingTimeTimeSpan} interval" );
			}
			catch ( Exception ex )
			{
				_log.Error( ex );
			}
		}

		protected override void OnStop()
		{
			try
			{
				_timer.Stop();
				_serviceHost.Close();
				_log.Info( $"{nameof( WindowsNativeService )} has stopped successfully." );
			}
			catch ( Exception ex )
			{
				_log.Error( ex );
			}
		}

		void OnPollingTimerElapsed( Object sender, ElapsedEventArgs e )
		{
			try
			{
				_bibliothecary.MarkTimeElapsed( _pollingTimeTimeSpan );
			}
			catch ( Exception ex )
			{
				_log.Error( ex );
			}
		}

		const Double PollingTimeMilliseconds = 2.5 * 60 * 1000; // 10 minutes
		static readonly TimeSpan _pollingTimeTimeSpan = TimeSpan.FromMilliseconds( PollingTimeMilliseconds );
		static readonly Logger _log = LogManager.GetCurrentClassLogger();
		readonly Timer _timer;
		readonly BibliothecaryService _bibliothecary;
		ServiceHost _serviceHost;
	}
}
