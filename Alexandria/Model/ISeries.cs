// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
    /// <summary>
    /// A data model for information about a series from a particular website.
    /// </summary>
    public interface ISeries : IRequestable
    {
        /// <summary>
        /// Gets a list of all of the author(s) involved in working on this series.
        /// This list will never be null, but it can be empty (because some websites
        /// will allow authors to remove their names from a fanfic without deleting
        /// the fanfic completely).
        /// </summary>
        IReadOnlyList<IAuthorRequestHandle> Authors { get; }

        /// <summary>
        /// Gets the date that the series was started. While it is most likely going to
        /// be the date that the earliest fanfic was posted, it isn't guaranteed that
        /// it will be the date that the FIRST fanfic was posted, as fanfics in a series
        /// can be reordered without regard to when they were chronologically posted.
        /// </summary>
        DateTime DateStarted { get; }

        /// <summary>
        /// Gets the date that the series was most recently updated, either by a fanfic
        /// being added or a fanfic in the series being updated.
        /// </summary>
        DateTime DateLastUpdated { get; }

        /// <summary>
        /// Gets a value indicating whether the author(s) consider this series completed,
        /// or if there are still more works in the series potentially coming.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Gets a list of request handles for all of the fanfics that are currently in the
        /// series.
        /// </summary>
        IReadOnlyList<IFanficRequestHandle> Fanfics { get; }
    }
}
