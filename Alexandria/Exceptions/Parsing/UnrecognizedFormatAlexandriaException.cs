// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Exceptions.Parsing
{
    /// <summary>
    /// An exception which indicates that the data or the format of the data that was downloaded from
    /// the website in question is not understandable by the current version of Alexandria. This is a
    /// general "unknown data format" error, and something that end users should not need to worry about
    /// (so long as they're using Alexandria correctly).
    /// </summary>
    public sealed class UnrecognizedFormatAlexandriaException : AlexandriaParseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnrecognizedFormatAlexandriaException"/> class.
        /// </summary>
        /// <param name="website">The website whose data appears to be unrecognized.</param>
        /// <param name="url">The URL that was accessed and whose data was unrecognized in some manner.</param>
        /// <param name="message">A human-readable message detailing what the unrecognized data was.</param>
        internal UnrecognizedFormatAlexandriaException( Website website, Uri url, string message )
            : base( website, url, message )
        {
        }
    }
}
