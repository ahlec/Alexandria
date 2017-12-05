// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.RequestHandles;

namespace Alexandria.Model
{
    /// <summary>
    /// A data model for information about this fanfic as a part of a series.
    /// </summary>
    public interface ISeriesEntry
    {
        /// <summary>
        /// Gets a request handle for the series itself, which will contain information
        /// and a full list of all entries in the series.
        /// </summary>
        ISeriesRequestHandle Series { get; }

        /// <summary>
        /// Gets which number entry this is. This is 1-based rather than 0-based, so the
        /// first entry in a series would be 1.
        /// </summary>
        int EntryNumber { get; }

        /// <summary>
        /// Gets a request handle for the previous fanfic in this series. If there is
        /// no previous entry in the series, then this will be null.
        /// </summary>
        IFanficRequestHandle PreviousEntry { get; }

        /// <summary>
        /// Gets a request handle for the next fanfic in this series. If there is no
        /// subsequent entry in the series, then this will be null.
        /// </summary>
        IFanficRequestHandle NextEntry { get; }
    }
}
