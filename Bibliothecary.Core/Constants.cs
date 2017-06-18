using System;

namespace Bibliothecary.Core
{
	public static class Constants
	{
		public const String BibliothecaryName = "Bibliothecary: An Alexandria Suite Tool";
		public const String DatabaseFilename = "bibliothecary.sqlite";
		public static readonly Uri HttpServiceAddress = new Uri( "http://localhost:1700" );
		public const String ServiceEndpoint = "Bibliothecary";
		public const String LogFilename = "bibliothecary.log";
	}
}