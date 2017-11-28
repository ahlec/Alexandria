// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Alexandria.AO3.Model;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.RequestHandles
{
    /// <summary>
    /// A concrete class for requesting a fanfic from AO3.
    /// </summary>
    internal sealed class AO3FanficRequestHandle : RequestHandleBase<IFanfic, AO3Source>, IFanficRequestHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AO3FanficRequestHandle"/> class.
        /// </summary>
        /// <param name="source">The source that the HTML page came from, which is then used to
        /// request the model from the website. It is also passed along to the model when parsed
        /// as well in order to make future chained requests or query fanfics.</param>
        /// <param name="handle">The handle of the fanfic that is being requested.</param>
        public AO3FanficRequestHandle( AO3Source source, string handle )
            : base( source )
        {
            Handle = handle;
        }

        /// <inheritdoc />
        public string Handle { get; }

        /// <inheritdoc />
        protected override string RequestUri => $"http://www.archiveofourown.org/works/{Handle}?view_adult=true";

        /// <inheritdoc />
        protected override string RequestCacheHandle => $"ao3-fanfic-{Handle}";

        /// <summary>
        /// Parses a list of fanfic request handle from the most commonly encountered format: a container element
        /// (either an &lt;ol&gt; or a &lt;ul&gt;) which contains &lt;li&gt; elements (each which will inside of it
        /// contain somewhere an &lt;a&gt; element) with an @id attribute that contains the fanfic handle.
        /// </summary>
        /// <param name="source">The configured class for accessing AO3.</param>
        /// <param name="listContainer">The HTML element that contains &lt;li&gt; elements (this will be either a
        /// &lt;ol&gt; or &lt;ul&gt;). This parameter can be null as well.</param>
        /// <returns>A list, never null, of valid fanfic request handle.</returns>
        public static IReadOnlyList<IFanficRequestHandle> ParseFanficLiList( AO3Source source, HtmlNode listContainer )
        {
            if ( listContainer == null )
            {
                return new List<IFanficRequestHandle>( 0 );
            }

            List<IFanficRequestHandle> list = new List<IFanficRequestHandle>();

            foreach ( HtmlNode li in listContainer.Elements( "li" ) )
            {
                AO3FanficRequestHandle requestHandle = ParseFromWorkLi( source, li );
                if ( requestHandle == null )
                {
                    continue;
                }

                list.Add( requestHandle );
            }

            return list;
        }

        /// <summary>
        /// Gets the string representation of this request handle. The functionality of this is
        /// here for the purposes of ease of debugging and the result of this function should not
        /// be relied on to be in any particular format as it is liable to change as necessary.
        /// </summary>
        /// <returns>A human-readible string that represents the data contained in this request handle.</returns>
        public override string ToString()
        {
            return Handle;
        }

        /// <inheritdoc />
        protected override IFanfic ParseRequest( Document requestDocument )
        {
            if ( requestDocument.Html.SelectSingleNode( "//div[@id='workskin']" ) == null )
            {
                Source.PurgeHandleFromCache( RequestCacheHandle );
                string retryUrl = requestDocument.Url + "?view_adult=true";
                requestDocument = Source.GetCacheableHtmlWebPage( RequestCacheHandle, retryUrl );
            }

            return AO3Fanfic.Parse( Source, requestDocument );
        }

        static AO3FanficRequestHandle ParseFromWorkLi( AO3Source source, HtmlNode li )
        {
            string fanficHandle = li.GetAttributeValue( "id", null ).Substring( "work_".Length );
            return new AO3FanficRequestHandle( source, fanficHandle );
        }
    }
}
