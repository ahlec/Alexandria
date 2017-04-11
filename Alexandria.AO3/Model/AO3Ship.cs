using System;
using System.Collections.Generic;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3Ship : IShip
	{
		AO3Ship()
		{
		}

		public String Name { get; private set; }

		public ShipType Type { get; private set; }

		public IReadOnlyList<ICharacterRequestHandle> Characters { get; private set; }

		public IShipRequestHandle Info { get; private set; }

		internal static AO3Ship Parse( String shipTag )
		{
			AO3Ship parsed = new AO3Ship
			{
				Name = shipTag,
				Info = new AO3ShipRequestHandle( shipTag )
			};

			parsed.Characters = ParseUtils.ParseShipCharacters( shipTag, out ShipType type );
			parsed.Type = type;

			return parsed;
		}
	}
}
