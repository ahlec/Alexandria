using System.ServiceProcess;

namespace Bibliothecary
{
	internal static class Program
	{
		static void Main()
		{
			ServiceBase.Run( new ServiceBase[] { new WindowsNativeService() } );
		}
	}
}
