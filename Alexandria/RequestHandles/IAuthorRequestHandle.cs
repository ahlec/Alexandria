// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Model;

namespace Alexandria.RequestHandles
{
    /// <summary>
    /// A request handle that is configured to retrieve data about an author (or a regular
    /// user) from the website.
    /// </summary>
    public interface IAuthorRequestHandle : IRequestHandle<IAuthor>
    {
        /// <summary>
        /// Gets the username of the author that is being requested.
        /// </summary>
        string Username { get; }
    }
}
