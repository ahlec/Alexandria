// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Caching;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.Net;
using Alexandria.RequestHandles;
using Alexandria.Searching;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria
{
    public abstract class LibrarySource
    {
        protected LibrarySource( IWebClient webClient, Cache cache )
        {
            _webClient = webClient ?? throw new ArgumentNullException( nameof( webClient ) );
            _cache = cache;
        }

        public abstract string SourceHandle { get; }

        public abstract LibrarySearch MakeSearch();

        public abstract IAuthorRequestHandle MakeAuthorRequest( string username );

        public abstract ICharacterRequestHandle MakeCharacterRequest( string fullName );

        public abstract IFanficRequestHandle MakeFanficRequest( string handle );

        public abstract ISeriesRequestHandle MakeSeriesRequest( string handle );

        public abstract IShipRequestHandle MakeShipRequest( string tag );

        public abstract ITagRequestHandle MakeTagRequest( string tag );

        internal void PurgeHandleFromCache( string handle )
        {
            _cache?.RemoveItem<HtmlCacheableDocument>( handle );
        }

        internal HtmlCacheableDocument GetCacheableHtmlWebPage( string handle, string uri )
        {
            HtmlCacheableDocument document;
            if ( _cache != null )
            {
                if ( _cache.TryReadFromCache( handle, out document ) )
                {
                    return document;
                }
            }

            using ( WebResult webResult = _webClient.Get( uri ) )
            {
                HtmlDocument html = HtmlUtils.ParseHtmlDocument( webResult.ResponseText );
                document = new HtmlCacheableDocument( handle, webResult.ResponseUri, html );
            }

            _cache?.WriteToCache( document );
            return document;
        }

        internal HtmlDocument GetHtmlWebPage( string uri )
        {
            using ( WebResult webResult = _webClient.Get( uri ) )
            {
                return HtmlUtils.ParseHtmlDocument( webResult.ResponseText );
            }
        }

        readonly IWebClient _webClient;
        readonly Cache _cache;
    }
}
