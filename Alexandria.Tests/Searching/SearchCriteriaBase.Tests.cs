// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using NUnit.Framework;

namespace Alexandria.Tests.Searching
{
    [TestFixture]
    public class SearchCriteriaBaseTests
    {
        [Test]
        public void SearchCriteriaBase_MaximumNumberNonZeroPositive()
        {
            Assert.That( SearchCriteriaBaseMock.MaximumNumberValue, Is.Not.Zero );
            Assert.That( SearchCriteriaBaseMock.MaximumNumberValue, Is.Not.Negative );
        }

        [Test]
        public void SearchCriteriaBase_MaximumNumberLessThanInt32Max()
        {
            // Other unit tests will try to test the bounds by adding + 1 to the max value. Prevent overflow.
            Assert.That( SearchCriteriaBaseMock.MaximumNumberValue, Is.LessThan( int.MaxValue ) );
        }

        [Test]
        public void SearchCriteriaBase_Number1_MustStayWithinValidRange()
        {
            SearchCriteriaBaseMock searchCriteriaBase = new SearchCriteriaBaseMock();

            Assert.Throws<ArgumentOutOfRangeException>( () => searchCriteriaBase.Number1 = -1 );
            Assert.Throws<ArgumentOutOfRangeException>( () => searchCriteriaBase.Number1 = -17 );

            Assert.Throws<ArgumentOutOfRangeException>( () => searchCriteriaBase.Number1 = SearchCriteriaBaseMock.MaximumNumberValue + 1 );
            Assert.Throws<ArgumentOutOfRangeException>( () => searchCriteriaBase.Number1 = int.MaxValue );
        }

        [Test]
        public void SearchCriteriaBase_Number1_ValidValues()
        {
            SearchCriteriaBaseMock searchCriteriaBase = new SearchCriteriaBaseMock
            {
                Number1 = 0
            };
            Assert.That( searchCriteriaBase.Number1, Is.Zero );

            searchCriteriaBase.Number1 = 10;
            Assert.That( searchCriteriaBase.Number1, Is.EqualTo( 10 ) );

            searchCriteriaBase.Number1 = SearchCriteriaBaseMock.MaximumNumberValue;
            Assert.That( searchCriteriaBase.Number1, Is.EqualTo( SearchCriteriaBaseMock.MaximumNumberValue ) );
        }

        [Test]
        public void SearchCriteriaBase_Number2_MustStayWithinValidRange()
        {
            SearchCriteriaBaseMock searchCriteriaBase = new SearchCriteriaBaseMock();

            Assert.Throws<ArgumentOutOfRangeException>( () => searchCriteriaBase.Number2 = -1 );
            Assert.Throws<ArgumentOutOfRangeException>( () => searchCriteriaBase.Number2 = -17 );

            Assert.Throws<ArgumentOutOfRangeException>( () => searchCriteriaBase.Number2 = SearchCriteriaBaseMock.MaximumNumberValue + 1 );
            Assert.Throws<ArgumentOutOfRangeException>( () => searchCriteriaBase.Number2 = int.MaxValue );
        }

        [Test]
        public void SearchCriteriaBase_Number2_ValidValues()
        {
            SearchCriteriaBaseMock searchCriteriaBase = new SearchCriteriaBaseMock
            {
                Number2 = 0
            };
            Assert.That( searchCriteriaBase.Number2, Is.Zero );

            searchCriteriaBase.Number2 = 30;
            Assert.That( searchCriteriaBase.Number2, Is.EqualTo( 30 ) );

            searchCriteriaBase.Number2 = SearchCriteriaBaseMock.MaximumNumberValue;
            Assert.That( searchCriteriaBase.Number2, Is.EqualTo( SearchCriteriaBaseMock.MaximumNumberValue ) );
        }
    }
}
