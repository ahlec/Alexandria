// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Model;
using Alexandria.RequestHandles;
using NUnit.Framework;

namespace Alexandria.Tests.AO3
{
    internal static class AO3Assert
    {
        public static void IsDate( int expectedYear, int expectedMonth, int expectedDay, DateTime actualDate )
        {
            Assert.AreEqual( expectedYear, actualDate.Year );
            Assert.AreEqual( expectedMonth, actualDate.Month );
            Assert.AreEqual( expectedDay, actualDate.Day );
            Assert.AreEqual( 0, actualDate.Hour );
            Assert.AreEqual( 0, actualDate.Minute );
            Assert.AreEqual( 0, actualDate.Second );
            Assert.AreEqual( 0, actualDate.Millisecond );
        }

        public static void IsFanficRequest( string expectedHandle, IFanficRequestHandle requestHandle )
        {
            Assert.IsNotNull( requestHandle );
            Assert.AreEqual( expectedHandle, requestHandle.Handle );
        }

        public static void IsShipSterek( IShip ship, bool isDerekFirst = true )
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
