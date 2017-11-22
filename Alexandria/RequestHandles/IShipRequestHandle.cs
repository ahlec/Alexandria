// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Model;

namespace Alexandria.RequestHandles
{
    /// <summary>
    /// A request handle that is configured to retrieve data about a relationship (romantic,
    /// platonic, or otherwise) between two or more characters from the website.
    /// </summary>
    public interface IShipRequestHandle : IRequestHandle<IShip>
    {
        /// <summary>
        /// Gets the name of the ship that is being requested. This isn't guaranteed to have the
        /// names of the characters in it (for instance, this could be "sterek" instead of
        /// "Stiles Stilinski/Derek Hale"). The only guarantee is that, if this is a valid ship,
        /// that the ship tag will be synonymous with the official name of the ship (if it isn't
        /// the official name of the ship already).
        /// </summary>
        string ShipTag { get; }
    }
}
