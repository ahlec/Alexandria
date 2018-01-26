// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Searching
{
    /// <summary>
    /// A direction for sorting results of queries. See also: <seealso cref="SortField"/>, which are
    /// the fields that can be sorted upon.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// The results of the query should be returned in descending order based on the
        /// value used for sorting.
        /// </summary>
        Descending,

        /// <summary>
        /// The results of the query should be returned in ascending order based on the
        /// value used for sorting.
        /// </summary>
        Ascending
    }
}
