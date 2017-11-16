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
    /// (so long as they're using Alexandria correctly). Errors of this nature should be reported as
    /// bugs on the repository, as it likely indicates that the website's format has had a breaking change
    /// with Alexandria's code.
    /// </summary>
    public sealed class UnrecognizedFormatAlexandriaException : AlexandriaException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnrecognizedFormatAlexandriaException"/> class.
        /// </summary>
        /// <param name="website">The website whose data appears to be unrecognized.</param>
        /// <param name="exception">The native exception that was thrown that indicates what the exception
        /// actually is/where it originated from.</param>
        internal UnrecognizedFormatAlexandriaException( Website website, Exception exception )
            : base( $"The format of the {website} page was unrecognized in some manner.", exception )
        {
            Website = website;
        }

        /// <summary>
        /// Gets the website whose data appears to be unrecognized.
        /// </summary>
        public Website Website { get; }
    }
}
