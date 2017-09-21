// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
    public interface IFanfic : IRequestable
    {
        string Title { get; }

        IAuthorRequestHandle Author { get; }

        MaturityRating Rating { get; }

        ContentWarnings ContentWarnings { get; }

        IReadOnlyList<IShipRequestHandle> Ships { get; }

        IReadOnlyList<ICharacterRequestHandle> Characters { get; }

        IReadOnlyList<ITagRequestHandle> Tags { get; }

        int NumberWords { get; }

        DateTime DateStarted { get; }

        DateTime DateLastUpdated { get; }

        int NumberLikes { get; }

        int NumberComments { get; }

        ISeriesEntry SeriesInfo { get; }

        IChapterInfo ChapterInfo { get; }

        Language Language { get; }

        string Summary { get; }

        string AuthorsNote { get; }

        string Footnote { get; }

        string Text { get; }
    }
}
