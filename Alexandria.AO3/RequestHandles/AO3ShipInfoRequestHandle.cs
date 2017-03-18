using System;
using Alexandria.Model;

namespace Alexandria.AO3.RequestHandles
{
	public sealed class AO3ShipInfoRequestHandle : IRequestHandle<IShipInfo>
	{
		public AO3ShipInfoRequestHandle( String tagName )
		{
			TagName = tagName;
		}

		public String TagName { get; }
	}
}
