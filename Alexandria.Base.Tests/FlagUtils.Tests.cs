using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Utils;

namespace Alexandria.Base.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.UtilTestsCategory )]
	public class Test_FlagUtils
	{
		[TestMethod]
		public void FlagUtils_DetectsMultipleFlags()
		{
			Assert.IsFalse( CacheableObjects.FanficHtml.HasMultipleFlagsSet() );
			Assert.IsTrue( ( CacheableObjects.FanficHtml | CacheableObjects.TagHtml ).HasMultipleFlagsSet() );
			Assert.IsTrue( CacheableObjects.All.HasMultipleFlagsSet() );
			Assert.IsFalse( CacheableObjects.None.HasMultipleFlagsSet() );
		}
	}
}
