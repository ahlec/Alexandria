// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Searching
{
    /// <summary>
    /// An enum that describes the relationship between <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number1"/>
    /// and <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number2"/> when used to perform a search for
    /// <seealso cref="LibrarySearch"/>.
    /// </summary>
    public enum NumberSearchCriteriaType
    {
        /// <summary>
        /// Only values which exactly match <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number1"/>
        /// will be included in the results from <seealso cref="LibrarySearch"/>.
        /// <para />
        /// <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number2"/> is not used by this type.
        /// </summary>
        ExactMatch,

        /// <summary>
        /// Only values which are less than <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number1"/>
        /// will be included in the results from <seealso cref="LibrarySearch"/>.
        /// <para />
        /// <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number2"/> is not used by this type.
        /// </summary>
        LessThan,

        /// <summary>
        /// Only values which are greater than <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number1"/>
        /// will be included in the results from <seealso cref="LibrarySearch"/>.
        /// <para />
        /// <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number2"/> is not used by this type.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Only values which are between a range of <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number1"/>
        /// and <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number2"/> (greater than or equal to the value
        /// of the former and less than or equal to the value of the latter) will be included in the
        /// results from <seealso cref="LibrarySearch"/>.
        /// </summary>
        Range
    }
}
