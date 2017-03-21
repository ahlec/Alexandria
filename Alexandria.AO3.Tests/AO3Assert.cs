using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;

namespace Alexandria.AO3.Tests
{
	internal static class AO3Assert
	{
		public static void IsShipSterek( IShip ship, Boolean isDerekFirst = true )
		{
			Assert.IsNotNull( ship );
			Assert.AreEqual( ( isDerekFirst ? "Derek Hale/Stiles Stilinski" : "Stiles Stilinski/Derek Hale" ), ship.Name );
			Assert.AreEqual( ShipType.Romantic, ship.Type );
			Assert.IsNotNull( ship.Characters );
			Assert.AreEqual( 2, ship.Characters.Count );
			Assert.AreEqual( ( isDerekFirst ? "Derek Hale" : "Stiles Stilinski" ), ship.Characters[0].FullName );
			Assert.AreEqual( ( isDerekFirst ? "Stiles Stilinski" : "Derek Hale" ), ship.Characters[1].FullName );
			Assert.IsNotNull( ship.Info );
			Assert.AreEqual( ship.Name, ship.Info.ShipTag ); // We already validated it
		}
	}
}
