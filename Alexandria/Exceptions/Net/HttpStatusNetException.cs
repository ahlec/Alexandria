// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Net;

namespace Alexandria.Exceptions.Net
{
    /// <summary>
    /// A general exception for any non-success HTTP status code encountered during an HTTP request.
    /// </summary>
    public class HttpStatusNetException : NetException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStatusNetException"/> class.
        /// </summary>
        /// <param name="uri">The URL of the website that was requested.</param>
        /// <param name="statusCode">The status code that was encountered when making the unsuccessful HTTP request.</param>
        public HttpStatusNetException( string uri, HttpStatusCode statusCode )
            : this( "An unsuccessful HTTP result was encountered when performing the HTTP request.", uri, statusCode )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStatusNetException"/> class.
        /// </summary>
        /// <param name="message">The error message that should accompany the exception.</param>
        /// <param name="uri">The URL of the website that was requested.</param>
        /// <param name="statusCode">The status code that was encountered when making the unsuccessful HTTP request.</param>
        protected HttpStatusNetException( string message, string uri, HttpStatusCode statusCode )
            : base( message, uri )
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Gets the status code of the error that was encountered when performing the HTTP request.
        /// </summary>
        public HttpStatusCode StatusCode { get; }
    }
}
