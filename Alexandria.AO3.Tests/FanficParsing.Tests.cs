using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.AO3;
using Alexandria.Model;

namespace Alexandria.AO3.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		[TestCategory( "AO3" )]
		public void AO3Source_PrinceAmongWolves()
		{
			AO3Source source = new AO3Source();
			IFanfic fanfic = source.GetFanfic( "538425" );

			Assert.IsNotNull( fanfic );
			Assert.AreEqual( fanfic.Title, "Prince Among Wolves" );
			Assert.AreEqual( fanfic.Rating, MaturityRating.Explicit );
			Assert.AreEqual( fanfic.ContentWarnings, ContentWarnings.Undetermined );
			Assert.AreEqual( fanfic.NumberWords, 101000 );
			Assert.AreEqual( fanfic.DateStartedUtc.Date, new DateTime( 2012, 10, 16 ) );
			Assert.IsTrue( fanfic.NumberComments > 2000 );
			Assert.IsTrue( fanfic.NumberLikes > 17000 );
			Assert.IsNull( fanfic.SeriesInfo );
		}

		[TestMethod]
		[TestCategory( "AO3" )]
		public void AO3Source_PossibilityOfSilence()
		{
			AO3Source source = new AO3Source();
			IFanfic fanfic = source.GetFanfic( "3592305" );

			Assert.IsNotNull( fanfic );
			Assert.AreEqual( fanfic.Title, "The Possibility of Silence and the Reality of Sound" );
			Assert.AreEqual( fanfic.Rating, MaturityRating.Teen );
			Assert.AreEqual( fanfic.ContentWarnings, ContentWarnings.None );
			Assert.AreEqual( fanfic.NumberWords, 4084 );
			Assert.AreEqual( fanfic.DateStartedUtc.Date, new DateTime( 2015, 3, 22 ) );
			Assert.IsTrue( fanfic.NumberComments > 80 );
			Assert.IsTrue( fanfic.NumberLikes > 2400 );
			Assert.IsNotNull( fanfic.SeriesInfo );
			Assert.AreEqual( fanfic.SeriesInfo.EntryNumber, 8 );
			Assert.AreEqual( fanfic.SeriesInfo.PreviousEntryHandle, "3476975" );
			Assert.AreEqual( fanfic.SeriesInfo.NextEntryHandle, "3626367" );
		}

		[TestMethod]
		[TestCategory( "AO3" )]
		public void AO3Source_ItsNotMyLovestory()
		{
			AO3Source source = new AO3Source();
			IFanfic fanfic = source.GetFanfic( "6598738" );

			Assert.IsNotNull( fanfic );
			Assert.AreEqual( fanfic.Title, "It's Not My Lovestory" );
			Assert.AreEqual( fanfic.Rating, MaturityRating.General );
			Assert.AreEqual( fanfic.ContentWarnings, ContentWarnings.None );
			Assert.AreEqual( fanfic.NumberWords, 1653 );
			Assert.AreEqual( fanfic.DateStartedUtc.Date, new DateTime( 2016, 4, 19 ) );
			Assert.IsTrue( fanfic.NumberComments > 50 );
			Assert.IsTrue( fanfic.NumberLikes > 1800 );
			Assert.IsNotNull( fanfic.SeriesInfo );
			Assert.AreEqual( fanfic.SeriesInfo.EntryNumber, 2 );
			Assert.AreEqual( fanfic.SeriesInfo.PreviousEntryHandle, "4034680" );
			Assert.AreEqual( fanfic.SeriesInfo.NextEntryHandle, "8702479" );
		}
	}
}
