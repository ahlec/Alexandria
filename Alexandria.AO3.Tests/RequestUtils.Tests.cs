using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.UtilTestsCategory )]
	public class RequestUtils
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
	}
}
