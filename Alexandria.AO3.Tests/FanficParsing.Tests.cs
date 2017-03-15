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
			//Assert.AreEqual( fanfic.NumberWords, 10100 );
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
			//Assert.AreEqual( fanfic.NumberWords, 4084 );
		}
	}
}
