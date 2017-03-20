using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3.Tests
{
	internal static class AO3Assert
	{
		public static void MatchesTag( String text, ITag tag )
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
			IsCharacterRequest( ( isDerekFirst ? "Derek Hale" : "Stiles Stilinski" ), ship.Characters[0] );
			IsCharacterRequest( ( isDerekFirst ? "Stiles Stilinski" : "Derek Hale" ), ship.Characters[1] );
			Assert.IsNotNull( ship.Info as AO3TagInfoRequestHandle );
		}

		public static void IsCharacterRequest( String characterName, IRequestHandle<ICharacter> character )
		{
			Assert.IsNotNull( character as AO3CharacterRequestHandle );
			Assert.AreEqual( characterName, ( (AO3CharacterRequestHandle) character ).Name );
		}
	}
}
