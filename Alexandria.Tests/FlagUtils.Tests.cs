// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Caching;
using Alexandria.Utils;
using NUnit.Framework;

namespace Alexandria.Tests
{
    [TestFixture]
    public class FlagUtilsTests
    {
        [Test]
        public void FlagUtils_DetectsMultipleFlags()
        {
            Assert.IsFalse( CacheableObjects.FanficHtml.HasMultipleFlagsSet() );
            Assert.IsTrue( ( CacheableObjects.FanficHtml | CacheableObjects.TagHtml ).HasMultipleFlagsSet() );
            Assert.IsTrue( CacheableObjects.All.HasMultipleFlagsSet() );
            Assert.IsFalse( CacheableObjects.None.HasMultipleFlagsSet() );
        }
    }
}
