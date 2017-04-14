using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.QueryResultsTestsCategory )]
	public class QueryResults
	{
		[TestMethod]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void AO3QueryResults_ThrowsWhenRetrievingAtEnd()
		{
			IQueryResultsPage<IFanfic, IFanficRequestHandle> results = null;
			try
			{
				ITagRequestHandle requestHandle = AO3RequestUtils.MakeTagRequest( UnitTestConstants.Tag_POVJackFrost );
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
