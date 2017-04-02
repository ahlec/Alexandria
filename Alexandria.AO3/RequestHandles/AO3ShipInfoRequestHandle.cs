using System;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.RequestHandles
{
	internal sealed class AO3ShipInfoRequestHandle : IShipInfoRequestHandle
	{
		public AO3ShipInfoRequestHandle( String shipTag )
		{
			ShipTag = shipTag;
		}

		public String ShipTag { get; }

		public override string ToString()
		{
			return ShipTag;
		}
	}
}
