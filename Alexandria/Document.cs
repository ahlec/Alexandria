// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text;
using Alexandria.Exceptions.Parsing;
using Alexandria.Net;
using HtmlAgilityPack;

namespace Alexandria
{
    /// <summary>
    /// An HTML document that was downloaded from a website.
    /// </summary>
    internal sealed class Document
    {
        readonly HtmlDocument _htmlDocument;

        Document( string handle, Uri url, HtmlDocument document )
        {
            if ( string.IsNullOrWhiteSpace( handle ) )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            _htmlDocument = document ?? throw new ArgumentNullException( nameof( document ) );

            Handle = handle;
            Url = url ?? throw new ArgumentNullException( nameof( url ) );
        }

        /// <summary>
        /// Gets the handle of this document as it is stored in the cache.
        /// </summary>
        public string Handle { get; }

        /// <summary>
        /// Gets the URL that this document was downloaded from.
        /// </summary>
        public Uri Url { get; }

        /// <summary>
        /// Gets the &lt;html&gt; node for the document that was downloaded.
        /// </summary>
        public HtmlNode Html => _htmlDocument.DocumentNode;

        /// <summary>
        /// Creates a new document from a <seealso cref="WebResult"/>. This is a utility function to
        /// make the construction of a document easier and more self-contained.
        /// </summary>
        /// <param name="website">The website that this document came from.</param>
        /// <param name="handle">The handle for this document in the cache.</param>
        /// <param name="result">The result that came back from executing the web request.</param>
        /// <returns>A new document that contains the parsed HTML document from the website.</returns>
        public static Document ParseFromWebResult( Website website, string handle, WebResult result )
        {
            HtmlDocument document = new HtmlDocument();
            byte[] bytes = Encoding.UTF8.GetBytes( result.ResponseText );

            using ( Stream textStream = new MemoryStream( bytes ) )
            {
                document.Load( textStream, Encoding.UTF8 );
            }

            if ( document.ParseErrors.Any() )
            {
                throw new BadHtmlDataAlexandriaException( website, result.ResponseUri, document );
            }

            return new Document( handle, result.ResponseUri, document );
        }

        /// <summary>
        /// Creates a new document from a stream. This is a utility function to make the construction of
        /// a document easier and more self-contained. Whereas the other static function is for reading
        /// from a web result, this is used for when we're creating a document that was previously stored
        /// in the cache. This is meant to be used in conjunction with <seealso cref="WriteHtmlToStream"/>.
        /// </summary>
        /// <param name="handle">The handle for this document in the cache.</param>
        /// <param name="url">The URL that this document was originally retrieved from.</param>
        /// <param name="stream">A stream that can be read to retrieve the full &lt;html&gt; document that
        /// was cached.</param>
        /// <returns>A new document that contained the parsed HTML document from the cache.</returns>
        public static Document ReadFromStream( string handle, Uri url, Stream stream )
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.Load( stream, Encoding.UTF8 );
            return new Document( handle, url, htmlDocument );
        }

        /// <summary>
        /// Writes the HTML in this document to the provided stream. This will ONLY write the HTML of the document
        /// to the stream, and nothing else. It's meant to be used in conjunction with <seealso cref="ReadFromStream"/>.
        /// </summary>
        /// <param name="stream">The stream that the HTML document should be written to.</param>
        public void WriteHtmlToStream( Stream stream )
        {
            _htmlDocument.Save( stream, Encoding.UTF8 );
        }
    }
}
