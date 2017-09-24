// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Caching;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.Searching;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria
{
    public abstract class LibrarySource
    {
        protected LibrarySource( Cache cache )
        {
            _cache = cache;
        }

        public abstract string SourceHandle { get; }

        public abstract T MakeRequest<T>( IRequestHandle<T> request )
            where T : IRequestable;

        public abstract IQueryResultsPage<IFanfic, IFanficRequestHandle> Search( LibrarySearch searchCriteria );

        internal TDocument GetWebPage<TDocument>( string handle, string endpoint )
            where TDocument : CacheableDocument
        {
            if ( _cache != null )
            {
                if ( _cache.TryReadFromCache( handle, out TDocument document ) )
                {
                    return document;
                }
            }

            throw new NotImplementedException();
        }

        protected HtmlDocument GetWebPage( CacheableObjects objectType, string cacheHandle, string endpoint, bool ignoreCache, out Uri responseUrl )
        {
            HtmlDocument document;

            if ( !CacheableObjectsUtils.IsHtmlObject( objectType ) )
            {
                throw new ArgumentException( "Only HTML objects can be requested by this function", nameof( objectType ) );
            }

            document = HtmlUtils.GetWebPage( endpoint, out responseUrl );
            return document;
        }

        readonly Cache _cache;
    }
}
