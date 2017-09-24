// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using HtmlAgilityPack;

namespace Alexandria.Documents
{
    internal sealed class HtmlCacheableDocument : CacheableDocument
    {
        public HtmlCacheableDocument( string handle, HtmlDocument document )
            : base( handle )
        {
            _htmlDocument = document ?? throw new ArgumentNullException( nameof( document ) );
        }

        public static HtmlCacheableDocument ReadFromStream( string handle, Stream stream )
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.Load( stream, Encoding.UTF8 );
            return new HtmlCacheableDocument( handle, htmlDocument );
        }

        /// <inheritdoc />
        public override void Write( Stream stream )
        {
            _htmlDocument.Save( stream, Encoding.UTF8 );
        }

        readonly HtmlDocument _htmlDocument;
    }
}
