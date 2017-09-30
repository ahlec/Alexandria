// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Model;

namespace Alexandria.RequestHandles
{
    public interface IShipRequestHandle : IRequestHandle<IShip>
    {
        string ShipTag { get; }
    }
}
