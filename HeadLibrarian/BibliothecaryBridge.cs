using System;
using System.ServiceModel;
using Bibliothecary.Core;

namespace HeadLibrarian
{
	public static class BibliothecaryBridge
	{
		static BibliothecaryBridge()
		{
			String endpoint = String.Concat( Constants.HttpServiceAddress, "/", Constants.ServiceEndpoint );
			_httpFactory = new ChannelFactory<IBibliothecaryService>( new BasicHttpBinding(), new EndpointAddress( endpoint ) );
		}

		public static Boolean IsServiceRunning => throw new NotImplementedException();

		public static IBibliothecaryService Service => _httpFactory.CreateChannel();

		static readonly ChannelFactory<IBibliothecaryService> _httpFactory;
	}
}
