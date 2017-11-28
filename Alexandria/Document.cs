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
    internal sealed class Document
    {
        public const string OptionsHtmlTag = "my_option";

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

        public string Handle { get; }

        public Uri Url { get; }

        public HtmlNode Html => _htmlDocument.DocumentNode;

        public static Document ParseFromWebResult( Website website, string handle, WebResult result )
        {
            // There's some super-bizzaro thing with HtmlAgilityPack where it doesn't recognise </option>.
            // http://stackoverflow.com/questions/293342/htmlagilitypack-drops-option-end-tags
            // Just replacing the tag name altogether to something else, it doesn't matter, we're not going to pay attention
            // to it right now.
            const string OptionsOpenTagReplacement = "<" + OptionsHtmlTag + " ";
            const string OptionsCloseTagReplacement = OptionsHtmlTag + ">";
            string html = result.ResponseText.Replace( "<option ", OptionsOpenTagReplacement ).Replace( "option>", OptionsCloseTagReplacement );

            HtmlDocument document = new HtmlDocument();
            byte[] bytes = Encoding.UTF8.GetBytes( html );
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

        public static Document ReadFromStream( string handle, Uri url, Stream stream )
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.Load( stream, Encoding.UTF8 );
            return new Document( handle, url, htmlDocument );
        }

        /// <inheritdoc />
        public void Write( Stream stream )
        {
            _htmlDocument.Save( stream, Encoding.UTF8 );
        }
    }
}
