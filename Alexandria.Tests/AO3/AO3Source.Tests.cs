// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.AO3;
using Alexandria.Exceptions.Input;
using Alexandria.RequestHandles;
using Alexandria.Tests.Mocks;
using NUnit.Framework;

namespace Alexandria.Tests.AO3
{
    [TestFixture]
    public sealed class AO3SourceTests
    {
        [Test]
        public void AO3Source_MakeAuthorRequest_ThrowsOnNull()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            Assert.Throws<ArgumentNullException>( () => source.MakeAuthorRequest( null ) );
        }

        [Test]
        public void AO3Source_MakeAuthorRequest_ThrowsOnInvalid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            Assert.Throws<InvalidAuthorAlexandriaException>( () => source.MakeAuthorRequest( string.Empty ) );
        }

        [Test]
        public void AO3Source_MakeFanficRequestThrowsOnNull()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            Assert.Throws<ArgumentNullException>( () => source.MakeFanficRequest( null ) );
        }

        [Test]
        public void AO3Source_MakeFanficRequest_ThrowsOnInvalid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            Assert.Throws<InvalidFanficAlexandriaException>( () => source.MakeFanficRequest( string.Empty ) );
            Assert.Throws<InvalidFanficAlexandriaException>( () => source.MakeFanficRequest( "hello world" ) );
        }

        [Test]
        public void AO3Source_MakeFanficRequestValid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            IFanficRequestHandle requestHandle = source.MakeFanficRequest( UnitTestConstants.FicHandlePrinceAmongWolves );

            Assert.IsNotNull( requestHandle );
            Assert.AreEqual( UnitTestConstants.FicHandlePrinceAmongWolves, requestHandle.Handle );
        }

        [Test]
        public void AO3Source_MakeSeriesRequest_ThrowsOnNull()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            Assert.Throws<ArgumentNullException>( () => source.MakeSeriesRequest( null ) );
        }

        [Test]
        public void AO3Source_MakeSeriesRequest_ThrowsOnInvalid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            Assert.Throws<InvalidSeriesAlexandriaException>( () => source.MakeSeriesRequest( string.Empty ) );
            Assert.Throws<InvalidSeriesAlexandriaException>( () => source.MakeSeriesRequest( "hello world" ) );
        }

        [Test]
        public void AO3Source_MakeSeriesRequestValid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            ISeriesRequestHandle requestHandle = source.MakeSeriesRequest( UnitTestConstants.SeriesHandleJanuaryJackrabbitWeek2014 );

            Assert.IsNotNull( requestHandle );
            Assert.AreEqual( UnitTestConstants.SeriesHandleJanuaryJackrabbitWeek2014, requestHandle.Handle );
        }

        [Test]
        public void AO3Source_MakeShipRequest_ThrowsOnNull()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            Assert.Throws<ArgumentNullException>( () => source.MakeShipRequest( null ) );
        }

        [Test]
        public void AO3Source_MakeShipRequest_ThrowsOnInvalid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            Assert.Throws<InvalidTagAlexandriaException>( () => source.MakeShipRequest( string.Empty ) );
        }

        [Test]
        public void AO3Source_MakeShipRequestValid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            IShipRequestHandle requestHandle = source.MakeShipRequest( UnitTestConstants.ShipSterek );

            Assert.IsNotNull( requestHandle );
            Assert.AreEqual( UnitTestConstants.ShipSterek, requestHandle.ShipTag );
        }

        [Test]
        public void AO3Source_MakeTagRequest_ThrowsOnNull()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            Assert.Throws<ArgumentNullException>( () => source.MakeTagRequest( null ) );
        }

        public void AO3Source_MakeTagRequest_ThrowsOnInvalid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            Assert.Throws<InvalidTagAlexandriaException>( () => source.MakeTagRequest( string.Empty ) );
        }

        [Test]
        public void AO3Source_MakeTagRequestValid()
        {
            AO3Source source = new AO3Source( new IgnoredWebClient(), new IgnoredLanguageManager(), null );
            ITagRequestHandle requestHandle = source.MakeTagRequest( UnitTestConstants.TagStilesStilinski );

            Assert.IsNotNull( requestHandle );
            Assert.AreEqual( UnitTestConstants.TagStilesStilinski, requestHandle.Text );
        }
    }
}
