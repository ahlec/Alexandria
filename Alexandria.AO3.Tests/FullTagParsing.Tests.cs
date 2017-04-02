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
		}

		readonly LibrarySource _source = new AO3Source();
	}
}
