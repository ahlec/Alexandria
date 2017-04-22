using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Bibliothecary
{
	[DesignerCategory( "" )]
	public class Bibliothecary : ServiceBase
	{
		public Bibliothecary()
		{
		}

		protected override void OnStart( String[] args )
		{
		}

		protected override void OnStop()
		{
		}
	}
}
