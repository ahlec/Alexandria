// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Model
{
    /// <summary>
    /// A base class for all concrete classes of <seealso cref="IRequestable"/>. This contains shared
    /// code and share interface implementations that will allow for getting a model up without boilerplate.
    /// </summary>
    /// <typeparam name="TSource">The configured concrete class that can be used to make requests of
    /// the particular website.</typeparam>
    internal abstract class RequestableBase<TSource> : IRequestable
        where TSource : LibrarySource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestableBase{TSource}"/> class.
        /// </summary>
        /// <param name="source">The configured concrete class that can be used to make requests of
        /// the particular website.</param>
        /// <param name="url">The URL for this tag page on the website.</param>
        protected RequestableBase( TSource source, Uri url )
        {
            Source = source;
            Url = url;
        }

        /// <inheritdoc />
        public Website Website => Source.Website;

        /// <inheritdoc />
        public Uri Url { get; }

        /// <summary>
        /// Gets the configured concrete class that can be used to construct further request handles of
        /// the particular website.
        /// </summary>
        protected TSource Source { get; }
    }
}
