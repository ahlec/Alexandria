// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Net;

namespace Alexandria.Exceptions.Net
{
    /// <summary>
    /// An exception that indicates that the URL could not be found when doing an HTTP request
    /// on it.
    /// <para />
    /// HTTP Status Code: 404
    /// </summary>
    public sealed class WebsiteNotFoundNetException : HttpStatusNetException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteNotFoundNetException"/> class.
        /// </summary>
        /// <param name="uri">The URL of the website that could not be found.</param>
        public WebsiteNotFoundNetException( string uri )
            : base( $"{uri} could not be found.", uri, HttpStatusCode.NotFound )
        {
        }
    }
}
