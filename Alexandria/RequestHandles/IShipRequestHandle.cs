using System;
using Alexandria.Model;

namespace Alexandria.RequestHandles
{
	public interface IShipRequestHandle : IRequestHandle<IShip>
	{
		String ShipTag { get; }
	}
}
