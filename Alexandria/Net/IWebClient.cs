// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Exceptions.Net;

namespace Alexandria.Net
{
    /// <summary>
    /// An interface for a dependency to Alexandria. This interface allows for Alexandria
    /// to have a means to communicate with websites; this class provides a client that can
    /// query URLs through HTTP.
    /// <para />
    /// This interface allows for different means of resolving URLs and providing data to
    /// Alexandria. The normal intention is a class which will make the HTTP requests with
    /// the live websites. This standard implementation is provided in <seealso cref="HttpWebClient"/>.
    /// Howevever, other implementations--such as one that rewrites live environment URLs
    /// to local testing environment URLs, or one that doesn't use HTTP at all (such as an
    /// implementation for unit tests) can also be provided.
    /// </summary>
    public interface IWebClient
    {
        /// <summary>
        /// Performs an HTTP GET operation on the URL provided.
        /// </summary>
        /// <param name="uri">The URL that should be retrieved.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="uri"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="uri"/> is empty or whitespace.</exception>
        /// <exception cref="WebsiteNotFoundNetException">Thrown when the URL could not be found.</exception>
        /// <exception cref="WebRequestTimedOutException">Thrown when the HTTP request timed out without completing.</exception>
        /// <exception cref="HttpStatusNetException">Thrown when a non-success HTTP status code is encountered.</exception>
        /// <returns>Returns the result of the HTTP GET request. THis should never be null.</returns>
        WebResult Get( string uri );
    }
}
