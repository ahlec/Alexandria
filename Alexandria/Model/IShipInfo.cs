using System;
using System.Collections.Generic;

namespace Alexandria.Model
{
	public interface IShipInfo : IRequestable
	{
		String Name { get; }

		ShipType Type { get; }

		IEnumerable<IRequestHandle<ICharacter>> Characters { get; }
	}
}
