// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3.Model;
using Alexandria.Model;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.RequestHandles
{
    internal sealed class AO3SeriesRequestHandle : RequestHandleBase<ISeries, AO3Source>, ISeriesRequestHandle
    {
        public AO3SeriesRequestHandle( AO3Source source, string handle )
            : base( source )
        {
            Handle = handle;
        }

        public string Handle { get; }

        /// <inheritdoc />
        protected override string RequestUri => $"http://archiveofourown.org/series/{Handle}";

        /// <inheritdoc />
        protected override string RequestCacheHandle => $"ao3-series-{Handle}";

        /// <inheritdoc />
        protected override ISeries ParseRequest( Document requestDocument )
        {
            return AO3Series.Parse( Source, requestDocument );
        }
    }
}
