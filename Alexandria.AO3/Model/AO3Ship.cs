using System;
using System.Collections.Generic;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Utils;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3Ship : IShip
	{
		AO3Ship( Uri url )
		{
			Url = url;
		}

		#region IRequestable

		public Uri Url { get; }

		#endregion

		#region IShip

		public String Name { get; private set; }

		public ShipType Type { get; private set; }

		public IReadOnlyList<ICharacterRequestHandle> Characters { get; private set; }

		#endregion

		internal static AO3Ship Parse( Uri url, HtmlDocument document )
		{
			AO3Ship parsed = new AO3Ship( url );

			throw new NotImplementedException(); ;

			return parsed;
		}
	}
}
