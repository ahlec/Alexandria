// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Searching
{
    /// <summary>
    /// An enum that determines what unit of time measurement the units of
    /// <seealso cref="DateSearchCriteria"/> are measured in.
    /// </summary>
    public enum DateField
    {
        /// <summary>
        /// The units are measured in hours.
        /// </summary>
        Hour,

        /// <summary>
        /// The units are measured in days.
        /// </summary>
        Day,

        /// <summary>
        /// The units are measured in weeks.
        /// </summary>
        Week,

        /// <summary>
        /// The units are measured in months.
        /// </summary>
        Month,

        /// <summary>
        /// The units are measured in years.
        /// </summary>
        Year
    }
}
