// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Model;

namespace Alexandria.Exceptions.Parsing
{
    /// <summary>
    /// An exception indicating that while parsing the type of a tag (<seealso cref="TagType"/>), the website
    /// used a value which wasn't understood by Alexandria.
    /// </summary>
    public sealed class UnrecognizedTagTypeAlexandriaException : AlexandriaParseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnrecognizedTagTypeAlexandriaException"/> class.
        /// </summary>
        /// <param name="value">The invalid value that is used on the website as a tag type but which
        /// wasn't recognized by Alexandria.</param>
        /// <param name="website">The website whose data appears to be unrecognized.</param>
        /// <param name="url">The URL that was accessed and whose data produced the parsing error.</param>
        internal UnrecognizedTagTypeAlexandriaException( string value, Website website, Uri url )
            : base( website, url )
        {
            InvalidValue = value;
        }

        /// <summary>
        /// Gets the invalid value that is used on the website as a tag type but which wasn't recognized
        /// by Alexandria.
        /// </summary>
        public string InvalidValue { get; }
    }
}
