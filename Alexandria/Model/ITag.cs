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
    /// A fanfic tag. Tags, often signified with a preceding # (hash sign), are metadata which
    /// is used for classification of a fanfic. This might be an indication of which tropes are
    /// used in the fanfic, or which ships or characters are included, but can also be ways of
    /// communicating something to the reader. Tags are commonly used not only for organisational
    /// purposes but also to provide informal dialogue with the reader.
    /// <para />
    /// Fanfics and tags have a 1:many relationship.
    /// </summary>
    public interface ITag : IQueryable
    {
        /// <summary>
        /// Gets the type of the tag.
        /// </summary>
        TagType Type { get; }

        /// <summary>
        /// Gets the full, readible text for this tag (the name of the tag/the tag itself).
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets any parent tags that this tag might have (tags which conceptually would
        /// encompass this tag alongside other tags, if this website supports that).
        /// </summary>
        IReadOnlyList<ITagRequestHandle> ParentTags { get; }

        /// <summary>
        /// Gets any other tags which have the same meaning as this tag but which are perhaps
        /// written a different way (for instance, a ship tag AAAA/BBBB might have a synonymous
        /// tag of BBBB/AAAA).
        /// </summary>
        IReadOnlyList<ITagRequestHandle> SynonymousTags { get; }
    }
}
