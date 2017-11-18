// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Model;

namespace Alexandria.RequestHandles
{
    /// <summary>
    /// A request handle that is configured to retrieve data about a tag from the website.
    /// </summary>
    public interface ITagRequestHandle : IRequestHandle<ITag>
    {
        /// <summary>
        /// Gets the text of the tag that can be requested.
        /// </summary>
        string Text { get; }
    }
}
