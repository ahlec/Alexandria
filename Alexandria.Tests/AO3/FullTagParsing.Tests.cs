using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.FullTagParsingTestsCategory )]
	public class FullTagParsing
	{
		[TestMethod]
		public void AO3Tag_StilesStilinski()
		{
			ITagRequestHandle request = AO3RequestUtils.MakeTagRequest( UnitTestConstants.Tag_StilesStilinski );
			ITag tag = _source.MakeRequest( request );

			Assert.IsNotNull( tag );
			Assert.AreEqual( TagType.Character, tag.Type );
			Assert.AreEqual( UnitTestConstants.Tag_StilesStilinski, tag.Text );

			Assert.IsNotNull( tag.ParentTags );
			Assert.AreEqual( 1, tag.ParentTags.Count );
			Assert.IsNotNull( tag.ParentTags[0] );
			Assert.AreEqual( "Teen Wolf (TV)", tag.ParentTags[0].Text );

			Assert.IsNotNull( tag.SynonymousTags );
			Assert.IsTrue( tag.SynonymousTags.Count >= 261 );

			HashSet<String> synonymousTags = new HashSet<String>( tag.SynonymousTags.Select( synonym => synonym.Text.ToLowerInvariant() ) );
			Assert.AreEqual( tag.SynonymousTags.Count, synonymousTags.Count );

			String[] selectSynonymousTags = 
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

			foreach ( String synonym in selectSynonymousTags )
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

		[TestMethod]
		public void AO3Tag_POVJackFrost()
		{
			ITagRequestHandle request = AO3RequestUtils.MakeTagRequest( UnitTestConstants.Tag_POVJackFrost );
			ITag tag = _source.MakeRequest( request );

			Assert.IsNotNull( tag );
			Assert.AreEqual( TagType.Miscellaneous, tag.Type );
			Assert.AreEqual( UnitTestConstants.Tag_POVJackFrost, tag.Text );

			Assert.IsNotNull( tag.ParentTags );
			Assert.AreEqual( 1, tag.ParentTags.Count );
			Assert.IsNotNull( tag.ParentTags[0] );
			Assert.AreEqual( "Guardians of Childhood & Related Fandoms", tag.ParentTags[0].Text );

			Assert.IsNotNull( tag.SynonymousTags );
			Assert.AreEqual( 3, tag.SynonymousTags.Count );
			Assert.IsNotNull( tag.SynonymousTags[0] );
			Assert.AreEqual( "jack's pov (kinda)", tag.SynonymousTags[0].Text );
			Assert.IsNotNull( tag.SynonymousTags[1] );
			Assert.AreEqual( "Narrated by Jack", tag.SynonymousTags[1].Text );
			Assert.IsNotNull( tag.SynonymousTags[2] );
			Assert.AreEqual( "POV Jack Frost", tag.SynonymousTags[2].Text );
			
			IQueryResultsPage<IFanfic, IFanficRequestHandle> fanfics = tag.QueryFanfics();
			Assert.IsNotNull( fanfics );
			Assert.IsFalse( fanfics.HasMoreResults );
			Assert.AreEqual( UnitTestConstants.Tag_POVJackFrostFanficHandles.Length, fanfics.Results.Count );
			for ( Int32 index = 0; index < fanfics.Results.Count; ++index )
			{
				Assert.IsNotNull( fanfics.Results[index] );
				Assert.AreEqual( UnitTestConstants.Tag_POVJackFrostFanficHandles[index], fanfics.Results[index].Handle );
			}
		}

		readonly LibrarySource _source = new AO3Source( LibrarySourceConfig.Default );
	}
}
