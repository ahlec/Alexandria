// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Exceptions;
using Alexandria.Exceptions.Parsing;

namespace Alexandria.RequestHandles
{
    /// <summary>
    /// A base class for all request handles that manages the work for retrieving and the data
    /// from the website in question.
    /// </summary>
    /// <typeparam name="TModel">The type of the data that is returned by requesting data from this
    /// request handle.</typeparam>
    /// <typeparam name="TSource">The configured concrete class that can be used to make requests of
    /// the particular website.</typeparam>
    internal abstract class RequestHandleBase<TModel, TSource> : IRequestHandle<TModel>
        where TModel : IRequestable
        where TSource : LibrarySource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandleBase{TModel, TSource}"/> class.
        /// </summary>
        /// <param name="source">The configured concrete class that can be used to make requests of
        /// the particular website.</param>
        protected RequestHandleBase( TSource source )
        {
            Source = source ?? throw new ArgumentNullException( nameof( source ) );
        }

        /// <inheritdoc />
        public Website Website => Source.Website;

        /// <summary>
        /// Gets the configured concrete class that can be used to make requests of the particular website.
        /// </summary>
        protected TSource Source { get; }

        /// <summary>
        /// Gets the endpoint URL to be requested in order to retrieve the necessary data.
        /// </summary>
        protected abstract string RequestUri { get; }

        /// <summary>
        /// Gets the handle that should uniquely identify this data that will be used to cache
        /// this data (if the <seealso cref="LibrarySource"/> has been configured to use caching).
        /// </summary>
        protected abstract string RequestCacheHandle { get; }

        /// <inheritdoc />
        public TModel Request()
        {
            string url = RequestUri;
            Document document = Source.GetCacheableHtmlWebPage( RequestCacheHandle, url );

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
                throw new UnknownParsingErrorAlexandriaException( Source.Website, new Uri( url ), ex );
            }
        }

        /// <summary>
        /// Parses the HTML result that was retrieved from the website into an actual Alexandria data model.
        /// </summary>
        /// <param name="requestDocument">The document that was retrieved from the website/endpoint.</param>
        /// <returns>A complete, valid data model filled with the data retrieved from the website/endpoint.</returns>
        protected abstract TModel ParseRequest( Document requestDocument );
    }
}
