// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3.Model;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.RequestHandles
{
    internal sealed class AO3FanficRequestHandle : RequestHandleBase<IFanfic, AO3Source>, IFanficRequestHandle
    {
        public AO3FanficRequestHandle( AO3Source source, string handle )
            : base( source )
        {
            Handle = handle;
        }

        public string Handle { get; }

        /// <inheritdoc />
        protected override string RequestUri => $"http://www.archiveofourown.org/works/{Handle}?view_adult=true";

        /// <inheritdoc />
        protected override string RequestCacheHandle => $"ao3-fanfic-{Handle}";

        public override string ToString()
        {
            return Handle;
        }

        internal static AO3FanficRequestHandle ParseFromWorkLi( AO3Source source, HtmlNode li )
        {
            string fanficHandle = li.GetAttributeValue( "id", null ).Substring( "work_".Length );
            return new AO3FanficRequestHandle( source, fanficHandle );
        }

        /// <inheritdoc />
        protected override IFanfic ParseRequest( HtmlCacheableDocument requestDocument )
        {
            if ( requestDocument.Html.SelectSingleNode( "//div[@id='workskin']" ) == null )
            {
                Source.PurgeHandleFromCache( RequestCacheHandle );
                string retryUrl = requestDocument.Url + "?view_adult=true";
                requestDocument = Source.GetCacheableHtmlWebPage( RequestCacheHandle, retryUrl );
            }

            return AO3Fanfic.Parse( Source, requestDocument );
        }
    }
}
