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

			Assert.IsNotNull( fanfic.Characters );
			Assert.AreEqual( 2, fanfic.Characters.Count );
			AO3Assert.IsCharacterRequest( "Stiles Stilinski", fanfic.Characters[0] );
			AO3Assert.IsCharacterRequest( "Derek Hale", fanfic.Characters[1] );

			Assert.IsNotNull( fanfic.Tags );
			Assert.AreEqual( 10, fanfic.Tags.Count );
			AO3Assert.MatchesTag( "Family", fanfic.Tags[0] );
			AO3Assert.MatchesTag( "Single Parents", fanfic.Tags[1] );
			AO3Assert.MatchesTag( "Babysitting", fanfic.Tags[2] );
			AO3Assert.MatchesTag( "Learning to be a parent", fanfic.Tags[3] );
			AO3Assert.MatchesTag( "Broken Family", fanfic.Tags[4] );
			AO3Assert.MatchesTag( "Transgender Child", fanfic.Tags[5] );
			AO3Assert.MatchesTag( "Gender Issues", fanfic.Tags[6] );
			AO3Assert.MatchesTag( "Acceptance", fanfic.Tags[7] );
			AO3Assert.MatchesTag( "Scent Marking", fanfic.Tags[8] );
			AO3Assert.MatchesTag( "Mild Transphobia", fanfic.Tags[9] );

			Assert.AreEqual( 101000, fanfic.NumberWords );
			Assert.AreEqual( new DateTime( 2012, 10, 16 ), fanfic.DateStartedUtc.Date );
			Assert.IsTrue( fanfic.NumberComments > 2000 );
			Assert.IsTrue( fanfic.NumberLikes > 17000 );
			Assert.IsNull( fanfic.SeriesInfo );

			Assert.AreEqual( Language.English, fanfic.Language );
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

			Assert.IsNotNull( fanfic.Characters );
			Assert.AreEqual( 9, fanfic.Characters.Count );
			AO3Assert.IsCharacterRequest( "Stiles Stilinski", fanfic.Characters[0] );
			AO3Assert.IsCharacterRequest( "Talia Hale", fanfic.Characters[1] );
			AO3Assert.IsCharacterRequest( "Derek Hale", fanfic.Characters[2] );
			AO3Assert.IsCharacterRequest( "Cora Hale", fanfic.Characters[3] );
			AO3Assert.IsCharacterRequest( "Claudia Stilinski", fanfic.Characters[4] );
			AO3Assert.IsCharacterRequest( "Isaac Lahey", fanfic.Characters[5] );
			AO3Assert.IsCharacterRequest( "Scott McCall", fanfic.Characters[6] );
			AO3Assert.IsCharacterRequest( "Sheriff Stilinski", fanfic.Characters[7] );
			AO3Assert.IsCharacterRequest( "The Hale Family - Character", fanfic.Characters[8] );

			Assert.IsNotNull( fanfic.Tags );
			Assert.AreEqual( fanfic.Tags.Count, 9 );
			AO3Assert.MatchesTag( "Alternate Universe - Human", fanfic.Tags[0] );
			AO3Assert.MatchesTag( "Alternate Universe - Soulmates", fanfic.Tags[1] );
			AO3Assert.MatchesTag( "Minor OC Death", fanfic.Tags[2] );
			AO3Assert.MatchesTag( "EMT Derek Hale", fanfic.Tags[3] );
			AO3Assert.MatchesTag( "Injured Stiles Stilinski", fanfic.Tags[4] );
			AO3Assert.MatchesTag( "Slow Build", fanfic.Tags[5] );
			AO3Assert.MatchesTag( "Angst", fanfic.Tags[6] );
			AO3Assert.MatchesTag( "Happy Ending", fanfic.Tags[7] );
			AO3Assert.MatchesTag( "Week 9", fanfic.Tags[8] );

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

			Assert.AreEqual( Language.English, fanfic.Language );
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
			AO3Assert.IsCharacterRequest( "Derek Hale", fanfic.Ships[1].Characters[0] );
			AO3Assert.IsCharacterRequest( "Kira Yukimura", fanfic.Ships[1].Characters[1] );
			Assert.IsNotNull( fanfic.Ships[2] );
			Assert.AreEqual( "(Derek-Kira friendship)", fanfic.Ships[2].Name );
			Assert.AreEqual( ShipType.Unknown, fanfic.Ships[2].Type );
			Assert.IsNotNull( fanfic.Ships[2].Characters );
			Assert.AreEqual( 0, fanfic.Ships[2].Characters.Count );

			Assert.IsNotNull( fanfic.Characters );
			Assert.AreEqual( 3, fanfic.Characters.Count );
			AO3Assert.IsCharacterRequest( "Derek Hale", fanfic.Characters[0] );
			AO3Assert.IsCharacterRequest( "Stiles Stilinski", fanfic.Characters[1] );
			AO3Assert.IsCharacterRequest( "Kira Yukimura", fanfic.Characters[2] );

			Assert.IsNotNull( fanfic.Tags );
			Assert.AreEqual( 6, fanfic.Tags.Count );
			AO3Assert.MatchesTag( "Alternate Universe - Soulmates", fanfic.Tags[0] );
			AO3Assert.MatchesTag( "Soulmate-Identifying Marks", fanfic.Tags[1] );
			AO3Assert.MatchesTag( "First Meetings", fanfic.Tags[2] );
			AO3Assert.MatchesTag( "Angst with a Happy Ending", fanfic.Tags[3] );
			AO3Assert.MatchesTag( "Light Angst", fanfic.Tags[4] );
			AO3Assert.MatchesTag( "Humor", fanfic.Tags[5] );

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

			Assert.AreEqual( Language.English, fanfic.Language );
		}
	}
}
