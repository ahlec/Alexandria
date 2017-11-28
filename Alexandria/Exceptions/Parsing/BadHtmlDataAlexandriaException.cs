// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Alexandria.Exceptions.Parsing
{
    /// <summary>
    /// An exception that wraps around errors that come from trying to parse the HTML that was received from the
    /// website into a model which Alexandria is capable of working with. This may contain one or many errors,
    /// depending on how many errors were encountered by the underlying library with the input data. Errors of
    /// this nature should be reported as bugs on the repository; however, for this specific exception, these
    /// reports will most likely be redirected to the library managing the interpretation of an HTML document
    /// in memory: HtmlAgilityPack (https://github.com/zzzprojects/html-agility-pack).
    /// </summary>
    public sealed class BadHtmlDataAlexandriaException : AlexandriaParseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BadHtmlDataAlexandriaException"/> class.
        /// </summary>
        /// <param name="website">The website whose data appears to be unrecognized.</param>
        /// <param name="url">The URL that was accessed and whose data produced the parsing error.</param>
        /// <param name="document">The document (not null) which contains one or more parse errors in
        /// <seealso cref="HtmlDocument.ParseErrors"/>.</param>
        internal BadHtmlDataAlexandriaException( Website website, Uri url, HtmlDocument document )
            : base( website, url, GetExceptionMessageFromParseErrors( document ) )
        {
        }

        static string GetExceptionMessageFromParseErrors( HtmlDocument document )
        {
            StringBuilder exceptionMessage = new StringBuilder( $"There were {document.ParseErrors.Count()} parsing errors:" );
            exceptionMessage.AppendLine();
            foreach ( HtmlParseError error in document.ParseErrors )
            {
                exceptionMessage.AppendLine( $"- Line {error.Line} Col {error.LinePosition}: {error.Reason} [{error.Code}]" );
            }

            return exceptionMessage.ToString();
        }
    }
}
