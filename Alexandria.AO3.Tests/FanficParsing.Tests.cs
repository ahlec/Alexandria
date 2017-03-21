using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.ParsingTestsCategory )]
	public class AO3FanficParsingTests
	{
		[TestMethod]
		public void AO3Source_PrinceAmongWolves()
		{
			LibrarySource source = new AO3Source();
			IFanficRequestHandle request = AO3RequestUtils.MakeFanficRequest( UnitTestConstants.FicHandle_PrinceAmongWolves );

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
			Assert.AreEqual( "Stiles Stilinski", fanfic.Characters[0].FullName );
			Assert.AreEqual( "Derek Hale", fanfic.Characters[1].FullName );

			Assert.IsNotNull( fanfic.Tags );
			Assert.AreEqual( 10, fanfic.Tags.Count );
			Assert.AreEqual( "Family", fanfic.Tags[0].Text );
			Assert.AreEqual( "Single Parents", fanfic.Tags[1].Text );
			Assert.AreEqual( "Babysitting", fanfic.Tags[2].Text );
			Assert.AreEqual( "Learning to be a parent", fanfic.Tags[3].Text );
			Assert.AreEqual( "Broken Family", fanfic.Tags[4].Text );
			Assert.AreEqual( "Transgender Child", fanfic.Tags[5].Text );
			Assert.AreEqual( "Gender Issues", fanfic.Tags[6].Text );
			Assert.AreEqual( "Acceptance", fanfic.Tags[7].Text );
			Assert.AreEqual( "Scent Marking", fanfic.Tags[8].Text );
			Assert.AreEqual( "Mild Transphobia", fanfic.Tags[9].Text );

			Assert.AreEqual( 101000, fanfic.NumberWords );
			Assert.AreEqual( new DateTime( 2012, 10, 16 ), fanfic.DateStartedUtc.Date );
			Assert.IsTrue( fanfic.NumberComments > 2000 );
			Assert.IsTrue( fanfic.NumberLikes > 17000 );
			Assert.IsNull( fanfic.SeriesInfo );

			Assert.AreEqual( Language.English, fanfic.Language );
		}

		[TestMethod]
		public void AO3Source_PossibilityOfSilence()
		{
			LibrarySource source = new AO3Source();
			IFanficRequestHandle request = AO3RequestUtils.MakeFanficRequest( UnitTestConstants.FicHandle_PossibilityOfSilence );

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
			Assert.AreEqual( "Minor or Background Relationship(s)", fanfic.Ships[1].Info.ShipTag );

			Assert.IsNotNull( fanfic.Characters );
			Assert.AreEqual( 9, fanfic.Characters.Count );
			Assert.AreEqual( "Stiles Stilinski", fanfic.Characters[0].FullName );
			Assert.AreEqual( "Talia Hale", fanfic.Characters[1].FullName );
			Assert.AreEqual( "Derek Hale", fanfic.Characters[2].FullName );
			Assert.AreEqual( "Cora Hale", fanfic.Characters[3].FullName );
			Assert.AreEqual( "Claudia Stilinski", fanfic.Characters[4].FullName );
			Assert.AreEqual( "Isaac Lahey", fanfic.Characters[5].FullName );
			Assert.AreEqual( "Scott McCall", fanfic.Characters[6].FullName );
			Assert.AreEqual( "Sheriff Stilinski", fanfic.Characters[7].FullName );
			Assert.AreEqual( "The Hale Family - Character", fanfic.Characters[8].FullName );

			Assert.IsNotNull( fanfic.Tags );
			Assert.AreEqual( fanfic.Tags.Count, 9 );
			Assert.AreEqual( "Alternate Universe - Human", fanfic.Tags[0].Text );
			Assert.AreEqual( "Alternate Universe - Soulmates", fanfic.Tags[1].Text );
			Assert.AreEqual( "Minor OC Death", fanfic.Tags[2].Text );
			Assert.AreEqual( "EMT Derek Hale", fanfic.Tags[3].Text );
			Assert.AreEqual( "Injured Stiles Stilinski", fanfic.Tags[4].Text );
			Assert.AreEqual( "Slow Build", fanfic.Tags[5].Text );
			Assert.AreEqual( "Angst", fanfic.Tags[6].Text );
			Assert.AreEqual( "Happy Ending", fanfic.Tags[7].Text );
			Assert.AreEqual( "Week 9", fanfic.Tags[8].Text );

			Assert.AreEqual( 4084, fanfic.NumberWords );
			Assert.AreEqual( new DateTime( 2015, 3, 22 ), fanfic.DateStartedUtc.Date );
			Assert.IsTrue( fanfic.NumberComments > 80 );
			Assert.IsTrue( fanfic.NumberLikes > 2400 );
			Assert.IsNotNull( fanfic.SeriesInfo );
			Assert.IsNotNull( fanfic.SeriesInfo.Series );
			Assert.AreEqual( 8, fanfic.SeriesInfo.EntryNumber );
			Assert.AreEqual( "3476975", fanfic.SeriesInfo.PreviousEntry.Handle );
			Assert.AreEqual( "3626367", fanfic.SeriesInfo.NextEntry.Handle );

			Assert.AreEqual( Language.English, fanfic.Language );
		}

		[TestMethod]
		public void AO3Source_ItsNotMyLovestory()
		{
			LibrarySource source = new AO3Source();
			IFanficRequestHandle request = AO3RequestUtils.MakeFanficRequest( UnitTestConstants.FicHandle_ItsNotMyLovestory );

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
			Assert.AreEqual( "Derek Hale", fanfic.Ships[1].Characters[0].FullName );
			Assert.AreEqual( "Kira Yukimura", fanfic.Ships[1].Characters[1].FullName );
			Assert.IsNotNull( fanfic.Ships[2] );
			Assert.AreEqual( "(Derek-Kira friendship)", fanfic.Ships[2].Name );
			Assert.AreEqual( ShipType.Unknown, fanfic.Ships[2].Type );
			Assert.IsNotNull( fanfic.Ships[2].Characters );
			Assert.AreEqual( 0, fanfic.Ships[2].Characters.Count );

			Assert.IsNotNull( fanfic.Characters );
			Assert.AreEqual( 3, fanfic.Characters.Count );
			Assert.AreEqual( "Derek Hale", fanfic.Characters[0].FullName );
			Assert.AreEqual( "Stiles Stilinski", fanfic.Characters[1].FullName );
			Assert.AreEqual( "Kira Yukimura", fanfic.Characters[2].FullName );

			Assert.IsNotNull( fanfic.Tags );
			Assert.AreEqual( 6, fanfic.Tags.Count );
			Assert.AreEqual( "Alternate Universe - Soulmates", fanfic.Tags[0].Text );
			Assert.AreEqual( "Soulmate-Identifying Marks", fanfic.Tags[1].Text );
			Assert.AreEqual( "First Meetings", fanfic.Tags[2].Text );
			Assert.AreEqual( "Angst with a Happy Ending", fanfic.Tags[3].Text );
			Assert.AreEqual( "Light Angst", fanfic.Tags[4].Text );
			Assert.AreEqual( "Humor", fanfic.Tags[5].Text );

			Assert.AreEqual( 1653, fanfic.NumberWords );
			Assert.AreEqual( new DateTime( 2016, 4, 19 ), fanfic.DateStartedUtc.Date );
			Assert.IsTrue( fanfic.NumberComments > 50 );
			Assert.IsTrue( fanfic.NumberLikes > 1800 );
			Assert.IsNotNull( fanfic.SeriesInfo );
			Assert.IsNotNull( fanfic.SeriesInfo.Series );
			Assert.AreEqual( 2, fanfic.SeriesInfo.EntryNumber );
			Assert.AreEqual( "4034680", fanfic.SeriesInfo.PreviousEntry.Handle );
			Assert.AreEqual( "8702479", fanfic.SeriesInfo.NextEntry.Handle );

			Assert.AreEqual( Language.English, fanfic.Language );
		}
	}
}
