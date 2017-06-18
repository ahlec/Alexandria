using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using Bibliothecary.Core;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Bibliothecary
{
	internal static class Program
	{
		static void Main()
		{
			LoggingConfiguration config = new LoggingConfiguration();
			FileTarget fileTarget = new FileTarget
			{
				FileName = Constants.LogFilename,
				Layout = "[${date:format=yyyy-MM-dd HH\\:mm\\:ss}][${logger:shortName=true}] ${trim-whitespace:${message}}"
			};
			config.AddTarget( "file", fileTarget );
			LoggingRule rule = new LoggingRule( "*", LogLevel.Info, fileTarget );
			config.LoggingRules.Add( rule );
			LogManager.Configuration = config;

			_log.Info( "---------------------------------------------------------" );
			_log.Info( "Bibliothecary.exe has been started" );

			String alexandriaDirectory = Path.GetDirectoryName( Assembly.GetEntryAssembly().Location );
			if ( String.IsNullOrEmpty( alexandriaDirectory ) )
			{
				_log.Fatal( $"Somehow, {nameof( alexandriaDirectory )} was null?" );
				_log.Fatal( $"Entry Assembly Location: {Assembly.GetEntryAssembly().Location}" );
				return;
			}
			Directory.SetCurrentDirectory( alexandriaDirectory );
			_log.Info( $"Setting cwd to {alexandriaDirectory}" );

			ServiceBase.Run( new ServiceBase[] { new WindowsNativeService() } );
		}

		static readonly Logger _log = LogManager.GetCurrentClassLogger();
	}
}
