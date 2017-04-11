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
			Assert.AreEqual( "crossroadswrite", author.Name );
			Assert.IsNotNull( author.Nicknames );
			Assert.AreEqual( 1, author.Nicknames.Count );
			Assert.AreEqual( "crossroadswrite", author.Nicknames[0] );
			Assert.AreEqual( new DateTime( 2013, 11, 15 ), author.DateJoined );
			Assert.AreEqual( "Portugal", author.Location );
			Assert.IsNull( author.Birthday );

			Assert.AreEqual( @"All works featured here are fan fiction, destined for the enjoyment of the fandom and it's in no way a profitable business. All characters, canon situations, worlds, etc, belong to their rightful owners, writers, producers.
I do not, however, give permition for these works of fiction to be re-published anywhere else without my consent. Nor do I give permition for them to be read out-loud or exposed to any type of media excluding this plataform in any way. Writers, producers, other fans, any kind of external source that wants to use this and take it themselves to use is not allowed to do so.", author.Biography );

			Assert.IsTrue( author.NumberFanfics >= 97 );

			IQueryResultsPage<IFanfic, IFanficRequestHandle> fanfics = author.QueryFanfics();
			Assert.IsNotNull( fanfics );
			Assert.IsTrue( fanfics.HasMoreResults );
			Assert.AreEqual( 20, fanfics.Results.Count );
		}

		readonly LibrarySource _source = new AO3Source();
	}
}
