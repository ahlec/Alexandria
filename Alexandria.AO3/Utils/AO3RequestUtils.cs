using System;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3.Utils
{
	public static class AO3RequestUtils
	{
		public static IFanficRequestHandle MakeFanficRequest( String handle )
		{
			if ( String.IsNullOrEmpty( handle ) )
			{
				throw new ArgumentNullException( nameof( handle ) );
			}

			foreach ( Char character in handle )
			{
				if ( !Char.IsDigit( character ) )
				{
					throw new ArgumentException( "Handles to fanfics on AO3 may only consist of numbers.", nameof( handle ) );
				}
			}

			return new AO3FanficRequestHandle( handle );
		}
	}
}
