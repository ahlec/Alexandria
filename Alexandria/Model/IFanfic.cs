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
    /// A fanfic from a website.
    /// </summary>
    public interface IFanfic : IRequestable
    {
        /// <summary>
        /// Gets the title of this fanfic.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the author(s) of this fanfic, if there are any. Websites that allow
        /// fanfics to be "abandoned" by their original author -- removed from the author's
        /// profile but not deleted from the website -- will be listed as not having any
        /// authors.
        /// <para />
        /// This is never null. If there are no authors, this will be an empty list.
        /// </summary>
        IReadOnlyList<IAuthorRequestHandle> Authors { get; }

        /// <summary>
        /// Gets the rating that the fanfic is marked on the website as.
        /// </summary>
        MaturityRating Rating { get; }

        /// <summary>
        /// Gets any content warnings that the fanfic has been marked with on the website.
        /// </summary>
        ContentWarnings ContentWarnings { get; }

        /// <summary>
        /// Gets a list of the ships (romantic, platonic, or otherwise) that this fanfic
        /// contains, as listed on the website.
        /// </summary>
        IReadOnlyList<IShipRequestHandle> Ships { get; }

        /// <summary>
        /// Gets a list of the characters that are in this fanfic, as listed on the website.
        /// </summary>
        IReadOnlyList<ICharacterRequestHandle> Characters { get; }

        /// <summary>
        /// Gets a list of all of the miscellaneous tags that this fanfic is taged with, as
        /// listed on the website.
        /// </summary>
        IReadOnlyList<ITagRequestHandle> Tags { get; }

        /// <summary>
        /// Gets the total number of words in this fanfic.
        /// </summary>
        int NumberWords { get; }

        /// <summary>
        /// Gets the date/time that this fanfic was first published.
        /// </summary>
        DateTime DateStarted { get; }

        /// <summary>
        /// Gets the date/time that this fanfic was last updated (either corrections posted or a
        /// new chapter added).
        /// </summary>
        DateTime DateLastUpdated { get; }

        /// <summary>
        /// Gets the number of likes that this fanfic has received on the website.
        /// </summary>
        int NumberLikes { get; }

        /// <summary>
        /// Gets the number of comments that this fanfic has received on the website.
        /// </summary>
        int NumberComments { get; }

        /// <summary>
        /// Gets information about any series this fanfic is a part of. This will never be null;
        /// if the fanfic is not part of any series, this will just be an empty list.
        /// </summary>
        IReadOnlyList<ISeriesEntry> SeriesInfo { get; }

        /// <summary>
        /// Gets information about this fanfic as a particular chapter in a multi-chapter fanfic.
        /// If this fanfic is not part of a multi-chapter fanfic, then this will be null.
        /// </summary>
        IChapterInfo ChapterInfo { get; }

        /// <summary>
        /// Gets the language that this fanfic was written in.
        /// </summary>
        LanguageInfo Language { get; }

        /// <summary>
        /// Gets the author-provided summary of the fanfic, if one was provided.
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// Gets the author's note, if one was provided at the start of the fanfic.
        /// </summary>
        string AuthorsNote { get; }

        /// <summary>
        /// Gets any footnote that the author provided at the end of the fanfic.
        /// </summary>
        string Footnote { get; }

        /// <summary>
        /// Gets the full text of this fanfic/chapter of a multi-chapter fanfic.
        /// </summary>
        string Text { get; }
    }
}
