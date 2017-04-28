using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Alexandria;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Bibliothecary.Data;

namespace Bibliothecary
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main()
		{
			Database database = Database.Open( "bibliothecary.sqlite" );
			Project test = database.GetProject( 1 ); //database.CreateNewProject();
			test.SearchQuery.Ships.Add( "Hiccup Horrendous Haddock III/Jack Frost (Rise of the Guardians)" );
			test.SearchQuery.Rating = MaturityRating.Explicit;
			test.SearchQuery.Language = Language.English;
			database.SaveProject( test );

			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
				new Bibliothecary()
			};
			ServiceBase.Run( ServicesToRun );
		}
	}
}
