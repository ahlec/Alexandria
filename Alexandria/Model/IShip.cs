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
    /// A relationship between two or more characters. This can be a platonic, romantic,
    /// or other relationship.
    /// <para />
    /// Fanfics and ships have a 1:many relationship. Additionally, characters and ships
    /// have a many:many relationship.
    /// </summary>
    public interface IShip : IRequestable
    {
        /// <summary>
        /// Gets the full name of the ship, as requested. There can be many valid names
        /// for a ship, and this will only be the name of the ship as it was requested.
        /// While it might be *A* valid name for the ship, this is not guaranteed to be the
        /// canonical, "official" name for that ship.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type of the ship: whether this ship is platonic, romantic, or some other
        /// type of relationship.
        /// </summary>
        ShipType Type { get; }

        /// <summary>
        /// Gets a list of the characters that are involved in this ship. This is not guaranteed
        /// to return all of the characters involved in the ship, or even ANY characters; the
        /// population of this field will be based entirely on what data is available on the website.
        /// </summary>
        IReadOnlyList<ICharacterRequestHandle> Characters { get; }
    }
}
