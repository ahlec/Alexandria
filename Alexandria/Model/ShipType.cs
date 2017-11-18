// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Model
{
    /// <summary>
    /// An enum for the different forms of relationships that a ship can come in
    /// (as documented by fanfiction websites).
    /// </summary>
    public enum ShipType
    {
        /// <summary>
        /// The type of the relationship could not be determined.
        /// </summary>
        Unknown,

        /// <summary>
        /// The relationship is platonic (that is, it is a friendship ship).
        /// </summary>
        Platonic,

        /// <summary>
        /// The relationship is romantic.
        /// </summary>
        Romantic
    }
}
