// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Caching;
using Alexandria.Net;
using Alexandria.RequestHandles;
using Alexandria.Searching;
using HtmlAgilityPack;

namespace Alexandria
{
    /// <summary>
    /// A base class for all APIs. This class is the base entry point for the various different supported
    /// websites. It is through a LibrarySource that functions to querying fanfics, searching the websites,
    /// or retrieving tags or authors is done. Individual websites will inherit from this class and provide
    /// additional functionality as that website has available.
    /// </summary>
    public abstract class LibrarySource
    {
        readonly IWebClient _webClient;
        readonly Cache _cache;

        protected LibrarySource( IWebClient webClient, Cache cache )
        {
            _webClient = webClient ?? throw new ArgumentNullException( nameof( webClient ) );
            _cache = cache;
        }

        public abstract Website Website { get; }

        public abstract LibrarySearch MakeSearch();

        public abstract IAuthorRequestHandle MakeAuthorRequest( string username );

        public abstract ICharacterRequestHandle MakeCharacterRequest( string fullName );

        public abstract IFanficRequestHandle MakeFanficRequest( string handle );

        public abstract ISeriesRequestHandle MakeSeriesRequest( string handle );

        public abstract IShipRequestHandle MakeShipRequest( string tag );

        public abstract ITagRequestHandle MakeTagRequest( string tag );

        internal void PurgeHandleFromCache( string handle )
        {
            _cache?.RemoveItem( handle );
        }

        internal Document GetCacheableHtmlWebPage( string handle, string uri )
        {
            Document document;
            if ( _cache != null )
            {
                if ( _cache.TryReadFromCache( handle, out document ) )
                {
                    return document;
                }
            }

            using ( WebResult webResult = _webClient.Get( uri ) )
            {
                document = Document.ParseFromWebResult( Website, handle, webResult );
            }

            _cache?.WriteToCache( document );
            return document;
        }

        internal HtmlNode GetHtmlWebPage( string uri )
        {
            using ( WebResult webResult = _webClient.Get( uri ) )
            {
                Document doc = Document.ParseFromWebResult( Website, "$$ignore$$", webResult );
                return doc.Html;
            }
        }
    }
}
