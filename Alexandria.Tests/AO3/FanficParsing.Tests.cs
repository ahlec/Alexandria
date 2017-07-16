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
			IFanficRequestHandle request = AO3RequestUtils.MakeFanficRequest( UnitTestConstants.FicHandle_Homesick );
			IFanfic fanfic = _source.MakeRequest( request );
			Assert.IsNull( fanfic.Author );
		}

		readonly LibrarySource _source = new AO3Source( LibrarySourceConfig.Default );
	}
}
