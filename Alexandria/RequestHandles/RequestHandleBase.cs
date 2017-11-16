// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Documents;
using Alexandria.Exceptions;
using Alexandria.Exceptions.Parsing;

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

        protected abstract string RequestUri { get; }

        protected abstract string RequestCacheHandle { get; }

        public TModel Request()
        {
            HtmlCacheableDocument document = Source.GetCacheableHtmlWebPage( RequestCacheHandle, RequestUri );

            try
            {
                return ParseRequest( document );
            }
            catch ( AlexandriaException )
            {
                throw;
            }
            catch ( Exception ex )
            {
                throw new UnrecognizedFormatAlexandriaException( Source.Website, ex );
            }
        }

        protected abstract TModel ParseRequest( HtmlCacheableDocument requestDocument );
    }
}
