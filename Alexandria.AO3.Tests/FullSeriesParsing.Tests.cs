using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.FullSeriesParsingTestsCategory )]
	public class FullSeriesParsing
	{
		[TestMethod]
		public void AO3Tag_JanuaryJackrabbitWeek2014()
		{
			ISeriesRequestHandle request = AO3RequestUtils.MakeSeriesRequest( UnitTestConstants.SeriesHandle_JanuaryJackrabbitWeek2014 );
			ISeries series = _source.MakeRequest( request );
			
			Assert.IsNotNull( series );

			Assert.IsNotNull( series.Author );
			Assert.AreEqual( "Melissae", series.Author.Username );
			AO3Assert.IsDate( 2014, 1, 25, series.DateStarted );
			AO3Assert.IsDate( 2016, 3, 28, series.DateLastUpdated );
			Assert.IsFalse( series.IsCompleted );

			Assert.IsNotNull( series.Fanfics );
			Assert.AreEqual( 6, series.Fanfics.Count );
			AO3Assert.IsFanficRequest( "1153087", series.Fanfics[0] );
			AO3Assert.IsFanficRequest( "1153126", series.Fanfics[1] );
			AO3Assert.IsFanficRequest( "1155713", series.Fanfics[2] );
			AO3Assert.IsFanficRequest( "1158578", series.Fanfics[3] );
			AO3Assert.IsFanficRequest( "1160404", series.Fanfics[4] );
			AO3Assert.IsFanficRequest( "1169503", series.Fanfics[5] );
		}

		readonly LibrarySource _source = new AO3Source();
	}
}
