using System;
using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
	public interface IShipInfo : IRequestable
	{
		String Name { get; }

		ShipType Type { get; }

		IReadOnlyList<ICharacterRequestHandle> Characters { get; }
	}
}
