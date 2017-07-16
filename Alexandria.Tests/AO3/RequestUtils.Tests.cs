using System;
using Alexandria.AO3.Utils;
using Alexandria.RequestHandles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alexandria.Tests.AO3
{
	[TestClass]
	[TestCategory( UnitTestConstants.UtilTestsCategory )]
	public class Test_RequestUtils
	{
		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void AO3RequestUtils_MakeFanficRequestThrowsOnNull()
		{
			AO3RequestUtils.MakeFanficRequest( null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void AO3RequestUtils_MakeFanficRequestThrowsOnEmpty()
		{
			AO3RequestUtils.MakeFanficRequest( String.Empty );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentException ), AllowDerivedTypes = false )]
		public void AO3RequestUtils_MakeFanficRequestThrowsOnAlphaCharacters()
		{
			AO3RequestUtils.MakeFanficRequest( "hello world" );
		}

		[TestMethod]
		public void AO3RequestUtils_MakeFanficRequestValid()
		{
			IFanficRequestHandle requestHandle = AO3RequestUtils.MakeFanficRequest( UnitTestConstants.FicHandle_PrinceAmongWolves );

			Assert.IsNotNull( requestHandle );
			Assert.AreEqual( UnitTestConstants.FicHandle_PrinceAmongWolves, requestHandle.Handle );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void AO3RequestUtils_MakeAuthorRequestThrowsOnNull()
		{
			AO3RequestUtils.MakeAuthorRequest( null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void AO3RequestUtils_MakeAuthorRequestThrowsOnEmpty()
		{
			AO3RequestUtils.MakeAuthorRequest( String.Empty );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void AO3RequestUtils_MakeTagRequestThrowsOnNull()
		{
			AO3RequestUtils.MakeTagRequest( null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void AO3RequestUtils_MakeTagRequestThrowsOnEmpty()
		{
			AO3RequestUtils.MakeTagRequest( String.Empty );
		}

		[TestMethod]
		public void AO3RequestUtils_MakeTagRequestValid()
		{
			ITagRequestHandle requestHandle = AO3RequestUtils.MakeTagRequest( UnitTestConstants.Tag_StilesStilinski );

			Assert.IsNotNull( requestHandle );
			Assert.AreEqual( UnitTestConstants.Tag_StilesStilinski, requestHandle.Text );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void AO3RequestUtils_MakeShipRequestThrowsOnNull()
		{
			AO3RequestUtils.MakeShipRequest( null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void AO3RequestUtils_MakeShipRequestThrowsOnEmpty()
		{
			AO3RequestUtils.MakeShipRequest( String.Empty );
		}

		[TestMethod]
		public void AO3RequestUtils_MakeShipRequestValid()
		{
			IShipRequestHandle requestHandle = AO3RequestUtils.MakeShipRequest( UnitTestConstants.Ship_Sterek );

			Assert.IsNotNull( requestHandle );
			Assert.AreEqual( UnitTestConstants.Ship_Sterek, requestHandle.ShipTag );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void AO3RequestUtils_MakeSeriesRequestThrowsOnNull()
		{
			AO3RequestUtils.MakeSeriesRequest( null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void AO3RequestUtils_MakeSeriesRequestThrowsOnEmpty()
		{
			AO3RequestUtils.MakeSeriesRequest( String.Empty );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentException ), AllowDerivedTypes = false )]
		public void AO3RequestUtils_MakeSeriesRequestThrowsOnAlphaCharacters()
		{
			AO3RequestUtils.MakeSeriesRequest( "hello world" );
		}

		[TestMethod]
		public void AO3RequestUtils_MakeSeriesRequestValid()
		{
			ISeriesRequestHandle requestHandle = AO3RequestUtils.MakeSeriesRequest( UnitTestConstants.SeriesHandle_JanuaryJackrabbitWeek2014 );

			Assert.IsNotNull( requestHandle );
			Assert.AreEqual( UnitTestConstants.SeriesHandle_JanuaryJackrabbitWeek2014, requestHandle.Handle );
		}
	}
}
