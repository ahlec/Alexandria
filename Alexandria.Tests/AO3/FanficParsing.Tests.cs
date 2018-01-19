// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3;
using Alexandria.AO3.Utils;
using Alexandria.Languages;
using Alexandria.Model;
using Alexandria.Net;
using Alexandria.RequestHandles;
using NUnit.Framework;

namespace Alexandria.Tests.AO3
{
    [TestFixture]
    public class FanficParsingTests
    {
        readonly LibrarySource _source;

        public FanficParsingTests()
        {
            HttpWebClient webClient = new HttpWebClient();
            WebLanguageManager languageManager = new WebLanguageManager( webClient );
            _source = new AO3Source( webClient, languageManager, null );
        }

        [Test]
        public void AO3Fanfic_AnonymousAuthorIsEmpty()
        {
            IFanficRequestHandle request = _source.MakeFanficRequest( UnitTestConstants.FicHandleTwoWeeksSleepIsADistantDream );
            IFanfic fanfic = request.Request();
            Assert.That( fanfic.Authors, Is.Empty );
        }
    }
}
