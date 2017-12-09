// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Exceptions.Net
{
    /// <summary>
    /// A standard base class for all networking-related exceptions.
    /// </summary>
    public abstract class NetException : AlexandriaException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetException"/> class.
        /// </summary>
        /// <param name="message">The error message that should accompany the exception.</param>
        /// <param name="uri">The URL of the website that was being requested.</param>
        protected NetException( string message, string uri )
            : base( message )
        {
            Uri = uri;
        }

        /// <summary>
        /// Gets the URL of the website that was being requested.
        /// </summary>
        public string Uri { get; }
    }
}
