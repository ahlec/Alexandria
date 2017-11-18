// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Model
{
    /// <summary>
    /// An enum for the different classifications that a tag could have, as indicated
    /// by the website that the data came from.
    /// </summary>
    public enum TagType
    {
        /// <summary>
        /// A tag that cannot be categorized. This might be a freeform tag (such as an
        /// author's dialogue tag) or a standard tag for something such as a trope or
        /// something which hasn't been categorized on that website yet.
        /// </summary>
        Miscellaneous,

        /// <summary>
        /// A character.
        /// </summary>
        Character,

        /// <summary>
        /// A relationship.
        /// </summary>
        Relationship
    }
}
