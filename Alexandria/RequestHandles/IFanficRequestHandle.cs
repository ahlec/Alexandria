// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Model;

namespace Alexandria.RequestHandles
{
    /// <summary>
    /// A request handle that is configured to retrieve data about a fanfic from the
    /// website.
    /// </summary>
    public interface IFanficRequestHandle : IRequestHandle<IFanfic>
    {
        /// <summary>
        /// Gets the unique handle of the fanfiction that is being requested. The format
        /// of this can change between different websites, but it is a guarantee that, within
        /// the website itself, a fanfic will be uniquely identified by a particular handle
        /// (there is only one handle per fanfic and only one fanfic per handle).
        /// </summary>
        string Handle { get; }
    }
}
