// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Documents;

namespace Alexandria.RequestHandles
{
    internal abstract class RequestHandleBase<TModel, TSource> : IRequestHandle<TModel>
        where TModel : IRequestable
        where TSource : LibrarySource
    {
        protected RequestHandleBase( TSource source )
        {
            Source = source ?? throw new ArgumentNullException( nameof( source ) );
        }

        protected TSource Source { get; }

        public TModel Request()
        {
            HtmlCacheableDocument document = Source.GetCacheableHtmlWebPage( RequestCacheHandle, RequestUri );
            return ParseRequest( document );
        }

        protected abstract string RequestUri { get; }

        protected abstract string RequestCacheHandle { get; }

        protected abstract TModel ParseRequest( HtmlCacheableDocument requestDocument );
    }
}
