// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Model;

namespace Alexandria.RequestHandles
{
    /// <summary>
    /// A request handle that is configured to retrieve data about a character in
    /// a fanfiction from the website.
    /// </summary>
    public interface ICharacterRequestHandle : IRequestHandle<ICharacter>
    {
        /// <summary>
        /// Gets the name of the charater that is being requested. Note that this is
        /// the name of the character as was originally presented to the request handle,
        /// and won't have additional data that wasn't otherwise present (for instance,
        /// "Harry Potter" will not (cough) magically (cough) become "Harry James Potter"
        /// here, and "Harry Potter" will be considered the <seealso cref="FullName"/>.
        /// </summary>
        string FullName { get; }
    }
}
