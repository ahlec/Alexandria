using System;
using System.ServiceModel;
using System.ServiceProcess;
using System.Threading;
using Bibliothecary.Core;
using PubSub;

namespace HeadLibrarian
{
	public static class BibliothecaryBridge
	{
		static BibliothecaryBridge()
		{
			_serviceController = new ServiceController( Constants.ServiceName );
			String endpoint = String.Concat( Constants.HttpServiceAddress, "/", Constants.ServiceEndpoint );
			_httpFactory = new ChannelFactory<IBibliothecaryService>( new BasicHttpBinding(), new EndpointAddress( endpoint ) );

			new Thread( ServicePollingThread ).Start();
		}

		public static Boolean IsServiceRunning
		{
			get
			{
				lock ( _isServiceRunningLock )
				{
					return _isServiceRunning;
				}
			}
		}

		public static IBibliothecaryService Service => _httpFactory.CreateChannel();

		// ReSharper disable once FunctionNeverReturns
		static void ServicePollingThread()
		{
			lock ( _isServiceRunningLock )
			{
				_isServiceRunning = ( _serviceController.Status == ServiceControllerStatus.Running );
			}
			_serviceController.Publish<BibliothecaryServiceStatusChanged>();

			while ( true )
			{
				_serviceController.Refresh();

				Boolean didStatusChange = false;
				lock( _isServiceRunningLock )
				{
					Boolean isServiceNowRunning = ( _serviceController.Status == ServiceControllerStatus.Running );
					if ( IsServiceRunning != isServiceNowRunning )
					{
						_isServiceRunning = isServiceNowRunning;
						didStatusChange = true;
					}
				}

				if ( didStatusChange )
				{
					_serviceController.Publish<BibliothecaryServiceStatusChanged>();
				}

				Thread.Sleep( 500 );
			}
		}

		static readonly ServiceController _serviceController;
		static readonly ChannelFactory<IBibliothecaryService> _httpFactory;
		static readonly Object _isServiceRunningLock = new Object();
		static Boolean _isServiceRunning;
	}
}
