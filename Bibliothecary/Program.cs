using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Bibliothecary.Database;

namespace Alexandria.Bibliothecary
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			Database database = Database.Open( "bibliothecary.sqlite" );
			IQueryResultsPage<IFanfic, IFanficRequestHandle> results = Config.Sources[0].Search( Config.SearchQuery );

			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
				new Bibliothecary()
			};
			ServiceBase.Run( ServicesToRun );
		}
	}
}
