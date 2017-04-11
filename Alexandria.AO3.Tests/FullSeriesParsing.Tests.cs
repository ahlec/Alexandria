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
			Assert.AreEqual( new DateTime( 2014, 1, 25 ), series.DateStarted.Date );
		}

		readonly LibrarySource _source = new AO3Source();
	}
}
