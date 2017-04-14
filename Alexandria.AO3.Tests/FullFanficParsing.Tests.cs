using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.FullFanficParsingTestsCategory )]
	public class AO3FanficParsingTests
	{
		[TestMethod]
		public void AO3Fanfic_PrinceAmongWolves()
		{
			IFanficRequestHandle request = AO3RequestUtils.MakeFanficRequest( UnitTestConstants.FicHandle_PrinceAmongWolves );

			IFanfic fanfic = _source.MakeRequest( request );

			Assert.IsNotNull( fanfic );
			Assert.AreEqual( "Prince Among Wolves", fanfic.Title );
			Assert.IsNotNull( fanfic.Author );
			Assert.AreEqual( MaturityRating.Explicit, fanfic.Rating );
			Assert.AreEqual( ContentWarnings.Undetermined, fanfic.ContentWarnings );

			Assert.IsNotNull( fanfic.Ships );
			Assert.AreEqual( 1, fanfic.Ships.Count );
			Assert.IsNotNull( fanfic.Ships[0] );
			Assert.AreEqual( "Derek Hale/Stiles Stilinski", fanfic.Ships[0].ShipTag );

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
			AO3Assert.IsDate( 2012, 10, 16, fanfic.DateStarted );
			AO3Assert.IsDate( 2013, 04, 25, fanfic.DateLastUpdated );
			Assert.IsTrue( fanfic.NumberComments > 2000 );
			Assert.IsTrue( fanfic.NumberLikes > 17000 );
			Assert.IsNull( fanfic.SeriesInfo );

			Assert.IsNotNull( fanfic.ChapterInfo );
			Assert.AreEqual( 1, fanfic.ChapterInfo.ChapterNumber );
			Assert.IsNull( fanfic.ChapterInfo.ChapterTitle );
			Assert.AreEqual( 20, fanfic.ChapterInfo.TotalNumberChapters );
			Assert.IsNotNull( fanfic.ChapterInfo.Chapters );
			Assert.AreEqual( 20, fanfic.ChapterInfo.Chapters.Count );
			Assert.AreEqual( "956260", fanfic.ChapterInfo.Chapters[0].Handle );
			Assert.AreEqual( "958923", fanfic.ChapterInfo.Chapters[1].Handle );
			Assert.AreEqual( "969987", fanfic.ChapterInfo.Chapters[2].Handle );
			Assert.AreEqual( "978501", fanfic.ChapterInfo.Chapters[3].Handle );
			Assert.AreEqual( "1013137", fanfic.ChapterInfo.Chapters[4].Handle );
			Assert.AreEqual( "1021020", fanfic.ChapterInfo.Chapters[5].Handle );
			Assert.AreEqual( "1039138", fanfic.ChapterInfo.Chapters[6].Handle );
			Assert.AreEqual( "1043245", fanfic.ChapterInfo.Chapters[7].Handle );
			Assert.AreEqual( "1067069", fanfic.ChapterInfo.Chapters[8].Handle );
			Assert.AreEqual( "1069071", fanfic.ChapterInfo.Chapters[9].Handle );
			Assert.AreEqual( "1113252", fanfic.ChapterInfo.Chapters[10].Handle );
			Assert.AreEqual( "1119371", fanfic.ChapterInfo.Chapters[11].Handle );
			Assert.AreEqual( "1136732", fanfic.ChapterInfo.Chapters[12].Handle );
			Assert.AreEqual( "1157366", fanfic.ChapterInfo.Chapters[13].Handle );
			Assert.AreEqual( "1168541", fanfic.ChapterInfo.Chapters[14].Handle );
			Assert.AreEqual( "1239449", fanfic.ChapterInfo.Chapters[15].Handle );
			Assert.AreEqual( "1266246", fanfic.ChapterInfo.Chapters[16].Handle );
			Assert.AreEqual( "1332473", fanfic.ChapterInfo.Chapters[17].Handle );
			Assert.AreEqual( "1418973", fanfic.ChapterInfo.Chapters[18].Handle );
			Assert.AreEqual( "1453053", fanfic.ChapterInfo.Chapters[19].Handle );

			Assert.AreEqual( Language.English, fanfic.Language );

			Assert.AreEqual( @"Looking for full day/evening sitter. 2 twin boys age 4.  Must have exp. w/werewolves. Must be human. No pedophiles. No teenage girls. Pay negotiable.", fanfic.Summary );
			Assert.IsNull( fanfic.AuthorsNote );
			Assert.IsNull( fanfic.Footnote );

			Assert.IsNotNull( fanfic.Text );
			Assert.IsTrue( fanfic.Text.StartsWith( "“I’m pretty sure you’re exaggerating the cost a little bit,” Stiles groused, one hand scrolling through online job postings while the mechanic on the line with him continued to list the multitude of things that were magically wrong with his jeep. Stiles wouldn’t put it past the guy to try and write off ‘unicorn in the alternator’ as one of the many fixes to add to the bill." ) );
			Assert.IsTrue( fanfic.Text.EndsWith( "That, of course, was when the front door opened and all Stiles heard was an infuriated roar of, “What the hell is this?” just as Andy’s pillow shield smacked him in the face and he sent the entire pillow fort crashing to the ground." ) );
			Assert.IsTrue( fanfic.Text.Contains( "The upstairs was blocked off as well, so Stiles really only had access to the kitchen, living room, laundry, garage, and a hallway that lead to a bathroom and the boys’ rooms. Stiles only chanced a quick peek into each one to see a proverbial vomit of toys and stuffed animals everywhere before he decided it would be best to act like he’d never seen any of it to begin with." ) );
		}

		[TestMethod]
		public void AO3Fanfic_PossibilityOfSilence()
		{
			IFanficRequestHandle request = AO3RequestUtils.MakeFanficRequest( UnitTestConstants.FicHandle_PossibilityOfSilence );

			IFanfic fanfic = _source.MakeRequest( request );

			Assert.IsNotNull( fanfic );
			Assert.AreEqual( "The Possibility of Silence and the Reality of Sound", fanfic.Title );
			Assert.IsNotNull( fanfic.Author );
			Assert.AreEqual( MaturityRating.Teen, fanfic.Rating );
			Assert.AreEqual( ContentWarnings.None, fanfic.ContentWarnings );

			Assert.IsNotNull( fanfic.Ships );
			Assert.AreEqual( 2, fanfic.Ships.Count );
			Assert.IsNotNull( fanfic.Ships[0] );
			Assert.AreEqual( "Stiles Stilinski/Derek Hale", fanfic.Ships[0].ShipTag );
			Assert.IsNotNull( fanfic.Ships[1] );
			Assert.AreEqual( "Minor or Background Relationship(s)", fanfic.Ships[1].ShipTag );

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
			AO3Assert.IsDate( 2015, 3, 22, fanfic.DateStarted );
			AO3Assert.IsDate( 2015, 3, 22, fanfic.DateLastUpdated );
			Assert.IsTrue( fanfic.NumberComments > 80 );
			Assert.IsTrue( fanfic.NumberLikes > 2400 );

			Assert.IsNotNull( fanfic.SeriesInfo );
			Assert.IsNotNull( fanfic.SeriesInfo.Series );
			Assert.AreEqual( 8, fanfic.SeriesInfo.EntryNumber );
			Assert.AreEqual( "3476975", fanfic.SeriesInfo.PreviousEntry.Handle );
			Assert.AreEqual( "3626367", fanfic.SeriesInfo.NextEntry.Handle );

			Assert.IsNull( fanfic.ChapterInfo );

			Assert.AreEqual( Language.English, fanfic.Language );

			Assert.AreEqual( @"Derek grew up knowing that soulmates are something to be cherished, so when he got a voice in his head, childish thoughts and flashes of color and objects, he’d excitedly jumped on his mother’s bed to tell her. She had smiled, ruffled his hair and told him how she was proud of him, even though Derek hadn’t really done anything.", fanfic.Summary );
			Assert.AreEqual( @"From the prompt:
 ➥a voice (not yours, your soulmate’s) appears in the back of your head, thinking about or commenting on whatever your soulmate happens to be doing at the time. But, you can’t converse with it, and it can’t tell you any details about who/where they are. Pretty frustrating tbh au", fanfic.AuthorsNote );
			Assert.AreEqual( @"Oh my god this fic messed my writing schedule so much it might just have turned me into a white rabbit with a pocket watch because dearie me am I late.", fanfic.Footnote );
		}

		[TestMethod]
		public void AO3Fanfic_ItsNotMyLovestory()
		{
			IFanficRequestHandle request = AO3RequestUtils.MakeFanficRequest( UnitTestConstants.FicHandle_ItsNotMyLovestory );

			IFanfic fanfic = _source.MakeRequest( request );

			Assert.IsNotNull( fanfic );
			Assert.AreEqual( "It's Not My Lovestory", fanfic.Title );
			Assert.IsNotNull( fanfic.Author );
			Assert.AreEqual( MaturityRating.General, fanfic.Rating );
			Assert.AreEqual( ContentWarnings.None, fanfic.ContentWarnings );

			Assert.IsNotNull( fanfic.Ships );
			Assert.AreEqual( 3, fanfic.Ships.Count );
			Assert.IsNotNull( fanfic.Ships[0] );
			Assert.AreEqual( "Derek Hale/Stiles Stilinski", fanfic.Ships[0].ShipTag );
			Assert.IsNotNull( fanfic.Ships[1] );
			Assert.AreEqual( "Derek Hale & Kira Yukimura", fanfic.Ships[1].ShipTag );
			Assert.IsNotNull( fanfic.Ships[2] );
			Assert.AreEqual( "(Derek-Kira friendship)", fanfic.Ships[2].ShipTag );

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
			AO3Assert.IsDate( 2016, 4, 19, fanfic.DateStarted );
			AO3Assert.IsDate( 2016, 4, 19, fanfic.DateLastUpdated );
			Assert.IsTrue( fanfic.NumberComments > 50 );
			Assert.IsTrue( fanfic.NumberLikes > 1800 );
			Assert.IsNotNull( fanfic.SeriesInfo );
			Assert.IsNotNull( fanfic.SeriesInfo.Series );
			Assert.AreEqual( 2, fanfic.SeriesInfo.EntryNumber );
			Assert.AreEqual( "4034680", fanfic.SeriesInfo.PreviousEntry.Handle );
			Assert.AreEqual( "8702479", fanfic.SeriesInfo.NextEntry.Handle );

			Assert.IsNull( fanfic.ChapterInfo );

			Assert.AreEqual( Language.English, fanfic.Language );

			Assert.AreEqual( @"When your soulmate’s first words to you were supposed to be etched on your wrist, a blank wrist was quite intriguing and an open invitation to be teased.
Derek’s wrist was missing a soulband.
Every single person in his acquaintance had a soulband, God! Even Greenberg had a soulband.", fanfic.Summary );
			Assert.AreEqual( @"Thank you Jonjo for the beta work. You are amazing <3", fanfic.AuthorsNote );
			Assert.AreEqual( @"I am on tumblr - Here - my Sterek blog.", fanfic.Footnote );
		}

		readonly LibrarySource _source = new AO3Source( LibrarySourceConfig.Default );
	}
}
