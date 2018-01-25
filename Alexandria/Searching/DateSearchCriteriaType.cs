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
    public enum DateSearchCriteriaType
    {
        /// <summary>
        /// Only values which, measured in <seealso cref="DateSearchCriteria.DateUnit"/>, were exactly
        /// <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number1"/> units ago will be included in the results from
        /// <seealso cref="LibrarySearch"/>.
        /// <para />
        /// <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number2"/> is not used by this type.
        /// </summary>
        Exactly,

        /// <summary>
        /// Only values which, measured in <seealso cref="DateSearchCriteria.DateUnit"/>, chronologically
        /// happened less than <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number1"/> units ago will be included in
        /// the results from <seealso cref="LibrarySearch"/>.
        /// <para />
        /// <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number2"/> is not used by this type.
        /// </summary>
        Before,

        /// <summary>
        /// Only values which, measured in <seealso cref="DateSearchCriteria.DateUnit"/>, chronologically
        /// happened more than <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number1"/> units ago will be included in
        /// the results from <seealso cref="LibrarySearch"/>.
        /// <para />
        /// <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number2"/> is not used by this type.
        /// </summary>
        After,

        /// <summary>
        /// Only values which, measured in <seealso cref="DateSearchCriteria.DateUnit"/>, chronologically
        /// happened between <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number1"/> and
        /// <seealso cref="SearchCriteriaBase{TSelf,TTypeEnum}.Number2"/> (happened on or after the date of the former and
        /// before or on the date of the latter) will be included in the results from
        /// <seealso cref="LibrarySearch"/>.
        /// </summary>
        Between
    }
}
