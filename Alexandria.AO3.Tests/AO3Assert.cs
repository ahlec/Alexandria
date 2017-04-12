using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.Tests
{
	internal static class AO3Assert
	{
		public static void IsDate( Int32 expectedYear, Int32 expectedMonth, Int32 expectedDay, DateTime actualDate )
		{
			Assert.AreEqual( expectedYear, actualDate.Year );
			Assert.AreEqual( expectedMonth, actualDate.Month );
			Assert.AreEqual( expectedDay, actualDate.Day );
			Assert.AreEqual( 0, actualDate.Hour );
			Assert.AreEqual( 0, actualDate.Minute );
			Assert.AreEqual( 0, actualDate.Second );
			Assert.AreEqual( 0, actualDate.Millisecond );
		}

		public static void IsFanficRequest( String expectedHandle, IFanficRequestHandle requestHandle )
		{
			Assert.IsNotNull( requestHandle );
			Assert.AreEqual( expectedHandle, requestHandle.Handle );
		}

		public static void IsShipSterek( IShip ship, Boolean isDerekFirst = true )
		{
			Assert.IsNotNull( ship );
			Assert.AreEqual( ( isDerekFirst ? "Derek Hale/Stiles Stilinski" : "Stiles Stilinski/Derek Hale" ), ship.Name );
			Assert.AreEqual( ShipType.Romantic, ship.Type );
			Assert.IsNotNull( ship.Characters );
			Assert.AreEqual( 2, ship.Characters.Count );
			Assert.AreEqual( ( isDerekFirst ? "Derek Hale" : "Stiles Stilinski" ), ship.Characters[0].FullName );
			Assert.AreEqual( ( isDerekFirst ? "Stiles Stilinski" : "Derek Hale" ), ship.Characters[1].FullName );
		}
	}
}
