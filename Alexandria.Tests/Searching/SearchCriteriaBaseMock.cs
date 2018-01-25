// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Searching;

namespace Alexandria.Tests.Searching
{
    internal enum SearchCriteriaBaseMockType
    {
        Test
    }

    internal sealed class SearchCriteriaBaseMock : SearchCriteriaBase<SearchCriteriaBaseMock, SearchCriteriaBaseMockType>
    {
        /// <inheritdoc />
        public override SearchCriteriaBaseMock Clone()
        {
            throw new NotSupportedException( "This function should not be used for a mock of the base class--child class functionality only!" );
        }

        /// <inheritdoc />
        public override bool Equals(SearchCriteriaBaseMock other)
        {
            throw new NotSupportedException( "This function should not be used for a mock of the base class--child class functionality only!" );
        }
    }
}
