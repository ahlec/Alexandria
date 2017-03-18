using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		[TestCategory( "AO3" )]
		public void AO3Source_PrinceAmongWolves()
		{
			LibrarySource source = new AO3Source();
			IRequestHandle<IFanfic> request = new AO3FanficRequestHandle( "538425" );

			IFanfic fanfic = source.MakeRequest( request );

			Assert.IsNotNull( fanfic );
			Assert.AreEqual( fanfic.Title, "Prince Among Wolves" );
			Assert.IsNotNull( fanfic.Author );
			Assert.AreEqual( fanfic.Rating, MaturityRating.Explicit );
			Assert.AreEqual( fanfic.ContentWarnings, ContentWarnings.Undetermined );

			Assert.IsNotNull( fanfic.Ships );
			Assert.AreEqual( fanfic.Ships.Count, 1 );
			Assert.IsNotNull( fanfic.Ships[0] );
			Assert.AreEqual( fanfic.Ships[0].Name, "Derek Hale/Stiles Stilinski" );
			Assert.AreEqual( fanfic.Ships[0].Type, ShipType.Romantic );
			Assert.IsNotNull( fanfic.Ships[0].Characters );
			Assert.AreEqual( fanfic.Ships[0].Characters.Count, 2 );
			Assert.IsNotNull( fanfic.Ships[0].Characters[0] as AO3CharacterRequestHandle );
			Assert.AreEqual( ( (AO3CharacterRequestHandle) fanfic.Ships[0].Characters[0] ).Name, "Derek Hale" );
			Assert.IsNotNull( fanfic.Ships[0].Characters[1] as AO3CharacterRequestHandle );
			Assert.AreEqual( ( (AO3CharacterRequestHandle) fanfic.Ships[0].Characters[1] ).Name, "Stiles Stilinski" );

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
			LibrarySource source = new AO3Source();
			IRequestHandle<IFanfic> request = new AO3FanficRequestHandle( "3592305" );

			IFanfic fanfic = source.MakeRequest( request );

			Assert.IsNotNull( fanfic );
			Assert.AreEqual( fanfic.Title, "The Possibility of Silence and the Reality of Sound" );
			Assert.IsNotNull( fanfic.Author );
			Assert.AreEqual( fanfic.Rating, MaturityRating.Teen );
			Assert.AreEqual( fanfic.ContentWarnings, ContentWarnings.None );
			Assert.AreEqual( fanfic.NumberWords, 4084 );
			Assert.AreEqual( fanfic.DateStartedUtc.Date, new DateTime( 2015, 3, 22 ) );
			Assert.IsTrue( fanfic.NumberComments > 80 );
			Assert.IsTrue( fanfic.NumberLikes > 2400 );
			Assert.IsNotNull( fanfic.SeriesInfo );
			Assert.IsNotNull( fanfic.SeriesInfo.Series );
			Assert.AreEqual( fanfic.SeriesInfo.EntryNumber, 8 );
			Assert.IsNotNull( fanfic.SeriesInfo.PreviousEntry as AO3FanficRequestHandle );
			Assert.AreEqual( ( (AO3FanficRequestHandle) fanfic.SeriesInfo.PreviousEntry ).Handle, "3476975" );
			Assert.IsNotNull( fanfic.SeriesInfo.NextEntry as AO3FanficRequestHandle );
			Assert.AreEqual( ( (AO3FanficRequestHandle) fanfic.SeriesInfo.NextEntry ).Handle, "3626367" );
		}

		[TestMethod]
		[TestCategory( "AO3" )]
		public void AO3Source_ItsNotMyLovestory()
		{
			LibrarySource source = new AO3Source();
			IRequestHandle<IFanfic> request = new AO3FanficRequestHandle( "6598738" );

			IFanfic fanfic = source.MakeRequest( request );

			Assert.IsNotNull( fanfic );
			Assert.AreEqual( fanfic.Title, "It's Not My Lovestory" );
			Assert.IsNotNull( fanfic.Author );
			Assert.AreEqual( fanfic.Rating, MaturityRating.General );
			Assert.AreEqual( fanfic.ContentWarnings, ContentWarnings.None );
			Assert.AreEqual( fanfic.NumberWords, 1653 );
			Assert.AreEqual( fanfic.DateStartedUtc.Date, new DateTime( 2016, 4, 19 ) );
			Assert.IsTrue( fanfic.NumberComments > 50 );
			Assert.IsTrue( fanfic.NumberLikes > 1800 );
			Assert.IsNotNull( fanfic.SeriesInfo );
			Assert.IsNotNull( fanfic.SeriesInfo.Series );
			Assert.AreEqual( fanfic.SeriesInfo.EntryNumber, 2 );
			Assert.IsNotNull( fanfic.SeriesInfo.PreviousEntry as AO3FanficRequestHandle );
			Assert.AreEqual( ( (AO3FanficRequestHandle) fanfic.SeriesInfo.PreviousEntry ).Handle, "4034680" );
			Assert.IsNotNull( fanfic.SeriesInfo.NextEntry as AO3FanficRequestHandle );
			Assert.AreEqual( ( (AO3FanficRequestHandle) fanfic.SeriesInfo.NextEntry ).Handle, "8702479" );
		}
	}
}
