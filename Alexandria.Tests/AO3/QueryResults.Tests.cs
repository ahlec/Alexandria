// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.AO3;
using Alexandria.AO3.Utils;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alexandria.Tests.AO3
{
    [TestClass]
    [TestCategory( UnitTestConstants.QueryResultsTestsCategory )]
    public class Test_QueryResults
    {
        [TestMethod]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public void AO3QueryResults_ThrowsWhenRetrievingAtEnd()
        {
            IQueryResultsPage<IFanfic, IFanficRequestHandle> results = null;
            try
            {
                ITagRequestHandle requestHandle = AO3RequestUtils.MakeTagRequest( UnitTestConstants.TagPOVJackFrost );
                ITag tag = _source.MakeRequest( requestHandle );
                results = tag.QueryFanfics();
            }
            catch
            {
                // In case one of the above functions throws InvalidOperationException, we don't want the test to pass.
                Assert.Fail();
            }

            Assert.IsNotNull( results );
            Assert.IsFalse( results.HasMoreResults );
            results.RetrieveNextPage();
        }

        readonly LibrarySource _source = new AO3Source( LibrarySourceConfig.Default );
    }
}
