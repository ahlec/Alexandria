using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Alexandria;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Bibliothecary.Core;

namespace Bibliothecary
{
	internal static class Program
	{
		static void Main()
		{
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
				new Bibliothecary()
			};
			ServiceBase.Run( ServicesToRun );
		}
	}
}
