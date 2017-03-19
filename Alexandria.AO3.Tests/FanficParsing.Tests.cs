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
			Assert.AreEqual( "Prince Among Wolves", fanfic.Title );
			Assert.IsNotNull( fanfic.Author );
			Assert.AreEqual( MaturityRating.Explicit, fanfic.Rating );
			Assert.AreEqual( ContentWarnings.Undetermined, fanfic.ContentWarnings );

			Assert.IsNotNull( fanfic.Ships );
			Assert.AreEqual( 1, fanfic.Ships.Count );
			AO3Assert.IsShipSterek( fanfic.Ships[0] );

			Assert.IsNotNull( fanfic.Tags );
			Assert.AreEqual( 10, fanfic.Tags.Count );
			AO3Assert.MatchesTag( fanfic.Tags[0], "Family" );
			AO3Assert.MatchesTag( fanfic.Tags[1], "Single Parents" );
			AO3Assert.MatchesTag( fanfic.Tags[2], "Babysitting" );
			AO3Assert.MatchesTag( fanfic.Tags[3], "Learning to be a parent" );
			AO3Assert.MatchesTag( fanfic.Tags[4], "Broken Family" );
			AO3Assert.MatchesTag( fanfic.Tags[5], "Transgender Child" );
			AO3Assert.MatchesTag( fanfic.Tags[6], "Gender Issues" );
			AO3Assert.MatchesTag( fanfic.Tags[7], "Acceptance" );
			AO3Assert.MatchesTag( fanfic.Tags[8], "Scent Marking" );
			AO3Assert.MatchesTag( fanfic.Tags[9], "Mild Transphobia" );

			Assert.AreEqual( 101000, fanfic.NumberWords );
			Assert.AreEqual( new DateTime( 2012, 10, 16 ), fanfic.DateStartedUtc.Date );
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
			Assert.AreEqual( "The Possibility of Silence and the Reality of Sound", fanfic.Title );
			Assert.IsNotNull( fanfic.Author );
			Assert.AreEqual( MaturityRating.Teen, fanfic.Rating );
			Assert.AreEqual( ContentWarnings.None, fanfic.ContentWarnings );

			Assert.IsNotNull( fanfic.Ships );
			Assert.AreEqual( 2, fanfic.Ships.Count );
			AO3Assert.IsShipSterek( fanfic.Ships[0], false );
			Assert.IsNotNull( fanfic.Ships[1] );
			Assert.AreEqual( "Minor or Background Relationship(s)", fanfic.Ships[1].Name );
			Assert.AreEqual( ShipType.Unknown, fanfic.Ships[1].Type );
			Assert.IsNotNull( fanfic.Ships[1].Characters );
			Assert.AreEqual( 0, fanfic.Ships[1].Characters.Count );
			Assert.IsNotNull( fanfic.Ships[1].Info as AO3TagInfoRequestHandle );
			Assert.AreEqual( "Minor or Background Relationship(s)", ( (AO3TagInfoRequestHandle) fanfic.Ships[1].Info ).TagName );

			Assert.IsNotNull( fanfic.Tags );
			Assert.AreEqual( fanfic.Tags.Count, 9 );
			AO3Assert.MatchesTag( fanfic.Tags[0], "Alternate Universe - Human" );
			AO3Assert.MatchesTag( fanfic.Tags[1], "Alternate Universe - Soulmates" );
			AO3Assert.MatchesTag( fanfic.Tags[2], "Minor OC Death" );
			AO3Assert.MatchesTag( fanfic.Tags[3], "EMT Derek Hale" );
			AO3Assert.MatchesTag( fanfic.Tags[4], "Injured Stiles Stilinski" );
			AO3Assert.MatchesTag( fanfic.Tags[5], "Slow Build" );
			AO3Assert.MatchesTag( fanfic.Tags[6], "Angst" );
			AO3Assert.MatchesTag( fanfic.Tags[7], "Happy Ending" );
			AO3Assert.MatchesTag( fanfic.Tags[8], "Week 9" );

			Assert.AreEqual( 4084, fanfic.NumberWords );
			Assert.AreEqual( new DateTime( 2015, 3, 22 ), fanfic.DateStartedUtc.Date );
			Assert.IsTrue( fanfic.NumberComments > 80 );
			Assert.IsTrue( fanfic.NumberLikes > 2400 );
			Assert.IsNotNull( fanfic.SeriesInfo );
			Assert.IsNotNull( fanfic.SeriesInfo.Series );
			Assert.AreEqual( 8, fanfic.SeriesInfo.EntryNumber );
			Assert.IsNotNull( fanfic.SeriesInfo.PreviousEntry as AO3FanficRequestHandle );
			Assert.AreEqual( "3476975", ( (AO3FanficRequestHandle) fanfic.SeriesInfo.PreviousEntry ).Handle );
			Assert.IsNotNull( fanfic.SeriesInfo.NextEntry as AO3FanficRequestHandle );
			Assert.AreEqual( "3626367", ( (AO3FanficRequestHandle) fanfic.SeriesInfo.NextEntry ).Handle );
		}

		[TestMethod]
		[TestCategory( "AO3" )]
		public void AO3Source_ItsNotMyLovestory()
		{
			LibrarySource source = new AO3Source();
			IRequestHandle<IFanfic> request = new AO3FanficRequestHandle( "6598738" );

			IFanfic fanfic = source.MakeRequest( request );

			Assert.IsNotNull( fanfic );
			Assert.AreEqual( "It's Not My Lovestory", fanfic.Title );
			Assert.IsNotNull( fanfic.Author );
			Assert.AreEqual( MaturityRating.General, fanfic.Rating );
			Assert.AreEqual( ContentWarnings.None, fanfic.ContentWarnings );

			Assert.IsNotNull( fanfic.Ships );
			Assert.AreEqual( 3, fanfic.Ships.Count );
			AO3Assert.IsShipSterek( fanfic.Ships[0] );
			Assert.IsNotNull( fanfic.Ships[1] );
			Assert.AreEqual( "Derek Hale & Kira Yukimura", fanfic.Ships[1].Name );
			Assert.AreEqual( ShipType.Platonic, fanfic.Ships[1].Type );
			Assert.IsNotNull( fanfic.Ships[1].Characters );
			Assert.AreEqual( 2, fanfic.Ships[1].Characters.Count );
			Assert.IsNotNull( fanfic.Ships[1].Characters[0] as AO3CharacterRequestHandle );
			Assert.AreEqual( "Derek Hale", ( (AO3CharacterRequestHandle) fanfic.Ships[1].Characters[0] ).Name );
			Assert.IsNotNull( fanfic.Ships[1].Characters[1] as AO3CharacterRequestHandle );
			Assert.AreEqual( "Kira Yukimura", ( (AO3CharacterRequestHandle) fanfic.Ships[1].Characters[1] ).Name );
			Assert.IsNotNull( fanfic.Ships[2] );
			Assert.AreEqual( "(Derek-Kira friendship)", fanfic.Ships[2].Name );
			Assert.AreEqual( ShipType.Unknown, fanfic.Ships[2].Type );
			Assert.IsNotNull( fanfic.Ships[2].Characters );
			Assert.AreEqual( 0, fanfic.Ships[2].Characters.Count );

			Assert.IsNotNull( fanfic.Tags );
			Assert.AreEqual( 6, fanfic.Tags.Count );
			AO3Assert.MatchesTag( fanfic.Tags[0], "Alternate Universe - Soulmates" );
			AO3Assert.MatchesTag( fanfic.Tags[1], "Soulmate-Identifying Marks" );
			AO3Assert.MatchesTag( fanfic.Tags[2], "First Meetings" );
			AO3Assert.MatchesTag( fanfic.Tags[3], "Angst with a Happy Ending" );
			AO3Assert.MatchesTag( fanfic.Tags[4], "Light Angst" );
			AO3Assert.MatchesTag( fanfic.Tags[5], "Humor" );

			Assert.AreEqual( 1653, fanfic.NumberWords );
			Assert.AreEqual( new DateTime( 2016, 4, 19 ), fanfic.DateStartedUtc.Date );
			Assert.IsTrue( fanfic.NumberComments > 50 );
			Assert.IsTrue( fanfic.NumberLikes > 1800 );
			Assert.IsNotNull( fanfic.SeriesInfo );
			Assert.IsNotNull( fanfic.SeriesInfo.Series );
			Assert.AreEqual( 2, fanfic.SeriesInfo.EntryNumber );
			Assert.IsNotNull( fanfic.SeriesInfo.PreviousEntry as AO3FanficRequestHandle );
			Assert.AreEqual( "4034680", ( (AO3FanficRequestHandle) fanfic.SeriesInfo.PreviousEntry ).Handle );
			Assert.IsNotNull( fanfic.SeriesInfo.NextEntry as AO3FanficRequestHandle );
			Assert.AreEqual( "8702479", ( (AO3FanficRequestHandle) fanfic.SeriesInfo.NextEntry ).Handle );
		}
	}
}
