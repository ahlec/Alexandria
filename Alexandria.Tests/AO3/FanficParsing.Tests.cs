// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3;
using Alexandria.AO3.Utils;
using Alexandria.Model;
using Alexandria.Net;
using Alexandria.RequestHandles;
using NUnit.Framework;

namespace Alexandria.Tests.AO3
{
    [TestFixture]
    [Category( UnitTestConstants.FanficParsingTestsCategory )]
    public class FanficParsingTests
    {
        [Test]
        public void AO3Fanfic_AnonymousAuthorIsNull()
        {
            IFanficRequestHandle request = _source.MakeFanficRequest( UnitTestConstants.FicHandleTwoWeeksSleepIsADistantDream );
            IFanfic fanfic = request.Request();
            Assert.IsNull( fanfic.Author );
        }

        readonly LibrarySource _source = new AO3Source( new HttpWebClient(), null );
    }
}
