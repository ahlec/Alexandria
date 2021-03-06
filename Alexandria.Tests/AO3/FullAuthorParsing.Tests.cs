﻿// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3;
using Alexandria.Languages;
using Alexandria.Model;
using Alexandria.Net;
using Alexandria.Querying;
using Alexandria.RequestHandles;
using NUnit.Framework;

namespace Alexandria.Tests.AO3
{
    [TestFixture]
    public class Test_AuthorParsing
    {
        readonly LibrarySource _source;

        public Test_AuthorParsing()
        {
            HttpWebClient webClient = new HttpWebClient();
            WebLanguageManager languageManager = new WebLanguageManager( webClient );
            _source = new AO3Source( webClient, languageManager, null );
        }

        [Test]
        public void AO3Author_Crossroadswrite()
        {
            IAuthorRequestHandle request = _source.MakeAuthorRequest( UnitTestConstants.AuthorUsernameCrossroadswrite );
            IAuthor author = request.Request();

            Assert.IsNotNull( author );
            Assert.AreEqual( "crossroadswrite", author.Name );
            Assert.IsNotNull( author.Nicknames );
            Assert.AreEqual( 1, author.Nicknames.Count );
            Assert.AreEqual( "crossroadswrite", author.Nicknames[0] );
            AO3Assert.IsDate( 2013, 11, 15, author.DateJoined );
            Assert.AreEqual( "Portugal", author.Location );
            Assert.IsNull( author.Birthday );

            Assert.AreEqual(
                @"All works featured here are fan fiction, destined for the enjoyment of the fandom and it's in no way a profitable business. All characters, canon situations, worlds, etc, belong to their rightful owners, writers, producers.
I do not, however, give permition for these works of fiction to be re-published anywhere else without my consent. Nor do I give permition for them to be read out-loud or exposed to any type of media excluding this plataform in any way. Writers, producers, other fans, any kind of external source that wants to use this and take it themselves to use is not allowed to do so.", author.Biography );

            Assert.IsTrue( author.NumberFanfics >= 97 );

            IQueryResultsPage<IFanfic, IFanficRequestHandle> fanfics = author.QueryFanfics();
            Assert.IsNotNull( fanfics );
            Assert.IsTrue( fanfics.HasMoreResults );
            Assert.AreEqual( 20, fanfics.Results.Count );
        }
    }
}
