using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3.Tests
{
	internal static class AO3Assert
	{
		public static void MatchesTag( ITag tag, String text )
		{
			Assert.IsNotNull( tag );
			Assert.AreEqual( text, tag.Text );
			Assert.IsNotNull( tag.Info as AO3TagInfoRequestHandle );
			Assert.AreEqual( text, ( (AO3TagInfoRequestHandle) tag.Info ).TagName );
		}

		public static void IsShipSterek( IShip ship, Boolean isDerekFirst = true )
		{
			Assert.IsNotNull( ship );
			Assert.AreEqual( ( isDerekFirst ? "Derek Hale/Stiles Stilinski" : "Stiles Stilinski/Derek Hale" ), ship.Name );
			Assert.AreEqual( ShipType.Romantic, ship.Type );
			Assert.IsNotNull( ship.Characters );
			Assert.AreEqual( 2, ship.Characters.Count );
			Assert.IsNotNull( ship.Characters[0] as AO3CharacterRequestHandle );
			Assert.AreEqual( ( isDerekFirst ? "Derek Hale" : "Stiles Stilinski" ), ( (AO3CharacterRequestHandle) ship.Characters[0] ).Name );
			Assert.IsNotNull( ship.Characters[1] as AO3CharacterRequestHandle );
			Assert.AreEqual( ( isDerekFirst ? "Stiles Stilinski" : "Derek Hale" ), ( (AO3CharacterRequestHandle) ship.Characters[1] ).Name );
			Assert.IsNotNull( ship.Info as AO3TagInfoRequestHandle );
		}
	}
}
