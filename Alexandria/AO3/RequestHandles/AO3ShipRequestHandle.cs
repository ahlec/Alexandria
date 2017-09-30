// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3.Model;
using Alexandria.AO3.Utils;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.RequestHandles
{
    internal sealed class AO3ShipRequestHandle : RequestHandleBase<IShip, AO3Source>, IShipRequestHandle
    {
        public AO3ShipRequestHandle( AO3Source source, string shipTag )
            : base( source )
        {
            ShipTag = shipTag;
        }

        public string ShipTag { get; }

        /// <inheritdoc />
        protected override string RequestUri => AO3RequestUtils.GetRequestUriForTag( ShipTag );

        /// <inheritdoc />
        protected override string RequestCacheHandle => $"ao3-ship-{ShipTag}";

        public override string ToString()
        {
            return ShipTag;
        }

        /// <inheritdoc />
        protected override IShip ParseRequest( HtmlCacheableDocument requestDocument )
        {
            return AO3Ship.Parse( requestDocument );
        }
    }
}
