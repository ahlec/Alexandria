// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Alexandria.AO3;
using Alexandria.AO3.Utils;
using Alexandria.Model;
using Alexandria.Net;
using Alexandria.RequestHandles;
using NUnit.Framework;

namespace Alexandria.Tests.AO3
{
    [TestFixture]
    [Category( UnitTestConstants.FullTagParsingTestsCategory )]
    public class Test_FullTagParsing
    {
        readonly LibrarySource _source = new AO3Source( new HttpWebClient(), null );

        [Test]
        public void AO3Tag_StilesStilinski()
        {
            ITagRequestHandle request = _source.MakeTagRequest( UnitTestConstants.TagStilesStilinski );
            ITag tag = request.Request();

            Assert.IsNotNull( tag );
            Assert.AreEqual( TagType.Character, tag.Type );
            Assert.AreEqual( UnitTestConstants.TagStilesStilinski, tag.Text );

            Assert.IsNotNull( tag.ParentTags );
            Assert.AreEqual( 1, tag.ParentTags.Count );
            Assert.IsNotNull( tag.ParentTags[0] );
            Assert.AreEqual( "Teen Wolf (TV)", tag.ParentTags[0].Text );

            Assert.IsNotNull( tag.SynonymousTags );
            Assert.IsTrue( tag.SynonymousTags.Count >= 261 );

            HashSet<string> synonymousTags = new HashSet<string>( tag.SynonymousTags.Select( synonym => synonym.Text.ToLowerInvariant() ) );
            Assert.AreEqual( tag.SynonymousTags.Count, synonymousTags.Count );

            string[] selectSynonymousTags =
            {
                "Drunk Stiles - Character",
                "Stiles Stilinski (wolf)",
                "Stiles - Teen Wolf",
                "temporarily-a-cat!Stiles",
                "omega!stiles - Character",
                "fox!Stiles - Character",
                "blushing stiles",
                "Omega Stiles Stilinski - Character",
                "Alpha Stiles Stilinski - Character",
                "BAMF Stiles Stilinski - Character"
            };

            foreach ( string synonym in selectSynonymousTags )
            {
                Assert.IsTrue( synonymousTags.Contains( synonym.ToLowerInvariant() ) );
            }

            IQueryResultsPage<IFanfic, IFanficRequestHandle> fanfics = tag.QueryFanfics();
            Assert.IsNotNull( fanfics );
            Assert.IsTrue( fanfics.HasMoreResults );
            Assert.AreEqual( 20, fanfics.Results.Count );
            fanfics = fanfics.RetrieveNextPage();
            Assert.IsNotNull( fanfics );
            Assert.IsTrue( fanfics.HasMoreResults );
            Assert.AreEqual( 20, fanfics.Results.Count );
        }

        [Test]
        public void AO3Tag_POVJackFrost()
        {
            ITagRequestHandle request = _source.MakeTagRequest( UnitTestConstants.TagPOVJackFrost );
            ITag tag = request.Request();

            Assert.IsNotNull( tag );
            Assert.AreEqual( TagType.Miscellaneous, tag.Type );
            Assert.AreEqual( UnitTestConstants.TagPOVJackFrost, tag.Text );

            Assert.IsNotNull( tag.ParentTags );
            Assert.AreEqual( 1, tag.ParentTags.Count );
            Assert.IsNotNull( tag.ParentTags[0] );
            Assert.AreEqual( "Guardians of Childhood & Related Fandoms", tag.ParentTags[0].Text );

            Assert.IsNotNull( tag.SynonymousTags );
            Assert.AreEqual( 4, tag.SynonymousTags.Count );
            Assert.IsNotNull( tag.SynonymousTags[0] );
            Assert.AreEqual( "jack's pov (kinda)", tag.SynonymousTags[0].Text );
            Assert.IsNotNull( tag.SynonymousTags[1] );
            Assert.AreEqual( "Mainly Jack POV", tag.SynonymousTags[1].Text );
            Assert.IsNotNull( tag.SynonymousTags[2] );
            Assert.AreEqual( "Narrated by Jack", tag.SynonymousTags[2].Text );
            Assert.IsNotNull( tag.SynonymousTags[3] );
            Assert.AreEqual( "POV Jack Frost", tag.SynonymousTags[3].Text );

            IQueryResultsPage<IFanfic, IFanficRequestHandle> fanfics = tag.QueryFanfics();
            Assert.IsNotNull( fanfics );
            Assert.IsFalse( fanfics.HasMoreResults );
            Assert.AreEqual( UnitTestConstants.TagPOVJackFrostFanficHandles.Length, fanfics.Results.Count );
            for ( int index = 0; index < fanfics.Results.Count; ++index )
            {
                Assert.IsNotNull( fanfics.Results[index] );
                Assert.AreEqual( UnitTestConstants.TagPOVJackFrostFanficHandles[index], fanfics.Results[index].Handle );
            }
        }
    }
}
