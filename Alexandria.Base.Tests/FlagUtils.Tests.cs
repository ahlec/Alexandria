using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Utils;

namespace Alexandria.Base.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.UtilTestsCategory )]
	public class FlagUtilsTests
	{
		[TestMethod]
		public void FlagUtils_DetectsMultipleFlags()
		{
			Assert.IsFalse( FlagUtils.HasMultipleFlagsSet( CacheableObjects.FanficHtml ) );
			Assert.IsTrue( FlagUtils.HasMultipleFlagsSet( CacheableObjects.FanficHtml | CacheableObjects.TagHtml ) );
			Assert.IsTrue( FlagUtils.HasMultipleFlagsSet( CacheableObjects.All ) );
			Assert.IsFalse( FlagUtils.HasMultipleFlagsSet( CacheableObjects.None ) );
		}
	}
}
