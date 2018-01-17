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
    /// <summary>
    /// A concrete class for requesting a series from AO3.
    /// </summary>
    internal sealed class AO3SeriesRequestHandle : RequestHandleBase<ISeries, AO3Source>, ISeriesRequestHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AO3SeriesRequestHandle"/> class.
        /// </summary>
        /// <param name="source">The source that the HTML page came from, which is then used to
        /// request the model from the website. It is also passed along to the model when parsed
        /// as well in order to make future chained requests or query fanfics.</param>
        /// <param name="handle">The handle of the series that is being requested.</param>
        public AO3SeriesRequestHandle( AO3Source source, string handle )
            : base( source )
        {
            Handle = handle;
        }

        /// <inheritdoc />
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
