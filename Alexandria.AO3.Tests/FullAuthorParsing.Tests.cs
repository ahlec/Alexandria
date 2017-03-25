using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.FullAuthorParsingTestsCategory )]
	public class AuthorParsing
	{
		[TestMethod]
		public void AO3Author_Crossroadswrite()
		{
			IAuthorRequestHandle request = AO3RequestUtils.MakeAuthorRequest( UnitTestConstants.AuthorUsername_Crossroadswrite );

			IAuthor author = _source.MakeRequest( request );
			Assert.IsNotNull( author );

		}

		readonly LibrarySource _source = new AO3Source();
	}
}
