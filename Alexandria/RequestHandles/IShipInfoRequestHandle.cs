using System;
using Alexandria.Model;

namespace Alexandria.RequestHandles
{
	public interface IShipInfoRequestHandle : IRequestHandle<IShipInfo>
	{
		String ShipTag { get; }
	}
}
