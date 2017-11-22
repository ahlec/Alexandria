// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Exceptions.Parsing
{
    /// <summary>
    /// A base class for all errors and exceptions that deal with parsing data from the websites.
    /// Any exception that inherits from this class shouldn't be left up to the enduser of this library
    /// but instead can be reported as an issue at https://github.com/ahlec/Alexandria/issues, as this
    /// likely indicates either an error with the logic used for parsing data, or that the format of
    /// the endpoint has changed and the API needs to be updated.
    /// <para />
    /// NOTE: Please attempt to update Alexandria to the latest version first before reporting errors.
    /// </summary>
    public abstract class AlexandriaParseException : AlexandriaException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlexandriaParseException"/> class.
        /// </summary>
        /// <param name="website">The website whose data appears to be unrecognized.</param>
        /// <param name="url">The URL that was accessed and whose data produced the parsing error.</param>
        protected AlexandriaParseException( Website website, Uri url )
        {
            Website = website;
            Url = url;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlexandriaParseException"/> class.
        /// </summary>
        /// <param name="website">The website whose data appears to be unrecognized.</param>
        /// <param name="url">The URL that was accessed and whose data produced the parsing error.</param>
        /// <param name="message">The human-readable error message to display with this exception.</param>
        protected AlexandriaParseException( Website website, Uri url, string message )
            : base( message )
        {
            Website = website;
            Url = url;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlexandriaParseException"/> class.
        /// </summary>
        /// <param name="website">The website whose data appears to be unrecognized.</param>
        /// <param name="url">The URL that was accessed and whose data produced the parsing error.</param>
        /// <param name="message">The human-readable error message to display with this exception.</param>
        /// <param name="innerException">The native exception that was thrown that indicates what the exception
        /// actually is/where it originated from.</param>
        protected AlexandriaParseException( Website website, Uri url, string message, Exception innerException )
            : base( message, innerException )
        {
            Website = website;
            Url = url;
        }

        /// <summary>
        /// Gets the website that the data that produced this error originated from.
        /// </summary>
        public Website Website { get; }

        /// <summary>
        /// Gets the URL that was accessed by Alexandria and whose data produced the parsing error.
        /// </summary>
        public Uri Url { get; }
    }
}
