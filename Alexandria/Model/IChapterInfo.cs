// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
    /// <summary>
    /// A data model for information about this fanfic as part of a multi-chapter fanfic.
    /// </summary>
    public interface IChapterInfo
    {
        /// <summary>
        /// Gets the title of this chapter, if there is one. This would be the unique name
        /// that the author gave to this chapter. If the author didn't give the chapter a
        /// name, then this will be null.
        /// </summary>
        string ChapterTitle { get; }

        /// <summary>
        /// Gets which number chapter this is. This is 1-based rather than 0-based, so the
        /// first chapter in a multi-chapter fanfic would be 1.
        /// </summary>
        int ChapterNumber { get; }

        /// <summary>
        /// Gets the total number of chapters in this multi-chapter fanfic, if it is known.
        /// If the fanfic is not finished or the author hasn't posted the total number of
        /// chapters in advance, then this will be null.
        /// </summary>
        int? TotalNumberChapters { get; }

        /// <summary>
        /// Gets a list of request handles for all of the individual chapters of the fanfic
        /// that have been posted so far. This list will never be null, the chapters are
        /// guaranteed to be in sequential order, and--by virtue of the fact that this fanfic
        /// is a chapter of a multi-chapter fanfic--this list will always have at least one
        /// item in it.
        /// </summary>
        IReadOnlyList<IFanficRequestHandle> Chapters { get; }
    }
}
