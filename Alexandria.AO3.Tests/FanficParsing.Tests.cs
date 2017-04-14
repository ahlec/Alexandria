using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.FanficParsingTestsCategory )]
	public class FanficParsingTests
	{
		[TestMethod]
		public void AO3Fanfic_AnonymousAuthorIsNull()
		{
			IFanficRequestHandle request = AO3RequestUtils.MakeFanficRequest( UnitTestConstants.FicHandle_Homesick );
			IFanfic fanfic = _source.MakeRequest( request );
			Assert.IsNull( fanfic.Author );
		}

		readonly LibrarySource _source = new AO3Source( LibrarySourceConfig.Default );
	}
}
