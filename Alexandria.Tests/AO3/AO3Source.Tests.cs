// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.AO3;
using Alexandria.RequestHandles;
using Alexandria.Tests.Mocks;
using NUnit.Framework;

namespace Alexandria.Tests.AO3
{
    [TestFixture]
    public sealed class AO3SourceTests
    {
        [Test]
        public void AO3Source_MakeAuthorRequestThrowsOnNullOrEmpty()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), null );
            Assert.Throws<ArgumentNullException>( () => source.MakeAuthorRequest( null ) );
            Assert.Throws<ArgumentNullException>( () => source.MakeAuthorRequest( string.Empty ) );
        }

        [Test]
        public void AO3Source_MakeFanficRequestThrowsOnNullOrEmpty()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), null );
            Assert.Throws<ArgumentNullException>( () => source.MakeFanficRequest( null ) );
            Assert.Throws<ArgumentNullException>( () => source.MakeFanficRequest( string.Empty ) );
        }

        [Test]
        public void AO3Source_MakeFanficRequestThrowsOnAlphaCharacters()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), null );
            Assert.Throws<ArgumentException>( () => source.MakeFanficRequest( "hello world" ) );
        }

        [Test]
        public void AO3Source_MakeFanficRequestValid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), null );
            IFanficRequestHandle requestHandle = source.MakeFanficRequest( UnitTestConstants.FicHandlePrinceAmongWolves );

            Assert.IsNotNull( requestHandle );
            Assert.AreEqual( UnitTestConstants.FicHandlePrinceAmongWolves, requestHandle.Handle );
        }

        [Test]
        public void AO3Source_MakeSeriesRequestThrowsOnNullOrEmpty()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), null );
            Assert.Throws<ArgumentNullException>( () => source.MakeSeriesRequest( null ) );
            Assert.Throws<ArgumentNullException>( () => source.MakeSeriesRequest( string.Empty ) );
        }

        [Test]
        public void AO3Source_MakeSeriesRequestThrowsOnAlphaCharacters()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), null );
            Assert.Throws<ArgumentException>( () => source.MakeSeriesRequest( "hello world" ) );
        }

        [Test]
        public void AO3Source_MakeSeriesRequestValid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), null );
            ISeriesRequestHandle requestHandle = source.MakeSeriesRequest( UnitTestConstants.SeriesHandleJanuaryJackrabbitWeek2014 );

            Assert.IsNotNull( requestHandle );
            Assert.AreEqual( UnitTestConstants.SeriesHandleJanuaryJackrabbitWeek2014, requestHandle.Handle );
        }

        [Test]
        public void AO3Source_MakeShipRequestThrowsOnNullOrEmpty()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), null );
            Assert.Throws<ArgumentNullException>( () => source.MakeShipRequest( null ) );
            Assert.Throws<ArgumentNullException>( () => source.MakeShipRequest( string.Empty ) );
        }

        [Test]
        public void AO3Source_MakeShipRequestValid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), null );
            IShipRequestHandle requestHandle = source.MakeShipRequest( UnitTestConstants.ShipSterek );

            Assert.IsNotNull( requestHandle );
            Assert.AreEqual( UnitTestConstants.ShipSterek, requestHandle.ShipTag );
        }

        [Test]
        public void AO3Source_MakeTagRequestThrowsOnNullOrEmpty()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), null );
            Assert.Throws<ArgumentNullException>( () => source.MakeTagRequest( null ) );
            Assert.Throws<ArgumentNullException>( () => source.MakeTagRequest( string.Empty ) );
        }

        [Test]
        public void AO3Source_MakeTagRequestValid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), null );
            ITagRequestHandle requestHandle = source.MakeTagRequest( UnitTestConstants.TagStilesStilinski );

            Assert.IsNotNull( requestHandle );
            Assert.AreEqual( UnitTestConstants.TagStilesStilinski, requestHandle.Text );
        }
    }
}
