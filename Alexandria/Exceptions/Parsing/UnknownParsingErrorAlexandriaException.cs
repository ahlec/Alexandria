// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Exceptions.Parsing
{
    /// <summary>
    /// An exception which wraps around any non-Alexandria exception that comes from parsing the response
    /// from a website. These errors are more or less the domain of Alexandria's developer(s), and something
    /// that end users should not need to worry about (so long as they're using Alexandria correctly). Errors
    /// of this nature should be reported as bugs on the repository, as it likely indicates that the website's
    /// format has had a breaking change with Alexandria's code.
    /// </summary>
    public sealed class UnknownParsingErrorAlexandriaException : AlexandriaParseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownParsingErrorAlexandriaException"/> class.
        /// </summary>
        /// <param name="website">The website whose data appears to be unrecognized.</param>
        /// <param name="url">The URL that was accessed and whose data produced the parsing error.</param>
        /// <param name="exception">The native exception that was thrown that indicates what the exception
        /// actually is/where it originated from.</param>
        internal UnknownParsingErrorAlexandriaException( Website website, Uri url, Exception exception )
            : base( website, url, $"The format of the {website} page was unrecognized in some manner.", exception )
        {
        }
    }
}
