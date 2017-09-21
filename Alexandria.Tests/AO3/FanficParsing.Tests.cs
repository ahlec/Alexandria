﻿// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3;
using Alexandria.AO3.Utils;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alexandria.Tests.AO3
{
    [TestClass]
    [TestCategory( UnitTestConstants.FanficParsingTestsCategory )]
    public class Test_FanficParsing
    {
        [TestMethod]
        public void AO3Fanfic_AnonymousAuthorIsNull()
        {
            IFanficRequestHandle request = AO3RequestUtils.MakeFanficRequest( UnitTestConstants.FicHandleHomesick );
            IFanfic fanfic = _source.MakeRequest( request );
            Assert.IsNull( fanfic.Author );
        }

        readonly LibrarySource _source = new AO3Source( LibrarySourceConfig.Default );
    }
}
