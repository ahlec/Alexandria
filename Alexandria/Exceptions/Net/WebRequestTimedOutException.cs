// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Exceptions.Net
{
    /// <summary>
    /// An exception that is thrown when an HTTP request timed out.
    /// </summary>
    public sealed class WebRequestTimedOutException : NetException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequestTimedOutException"/> class.
        /// </summary>
        /// <param name="uri">The URL of the website that was being requested.</param>
        public WebRequestTimedOutException( string uri )
            : base( "The HTTP request timed out while being performed.", uri )
        {
        }
    }
}
