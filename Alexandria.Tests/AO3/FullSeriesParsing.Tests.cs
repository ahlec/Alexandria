// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3;
using Alexandria.AO3.Tests;
using Alexandria.AO3.Utils;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alexandria.Tests.AO3
{
    [TestClass]
    [TestCategory( UnitTestConstants.FullSeriesParsingTestsCategory )]
    public class Test_FullSeriesParsing
    {
        [TestMethod]
        public void AO3Tag_JanuaryJackrabbitWeek2014()
        {
            ISeriesRequestHandle request = AO3RequestUtils.MakeSeriesRequest( UnitTestConstants.SeriesHandleJanuaryJackrabbitWeek2014 );
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

        readonly LibrarySource _source = new AO3Source( null );
    }
}
