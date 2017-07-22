using System;
using System.IO;
using System.Reflection;

namespace Bibliothecary.Core.Utils
{
	public static class BibliothecaryUtils
	{
		public static String AlexandriaDirectory
		{
			get
			{
				String alexandriaDirectory = Path.GetDirectoryName( Assembly.GetEntryAssembly().Location );
				if ( String.IsNullOrEmpty( alexandriaDirectory ) )
				{
					throw new ApplicationException( $"{nameof( AlexandriaDirectory )} is null (entry assembly location: {Assembly.GetEntryAssembly().Location})" );
				}
				return alexandriaDirectory;
			}
		}
	}
}
