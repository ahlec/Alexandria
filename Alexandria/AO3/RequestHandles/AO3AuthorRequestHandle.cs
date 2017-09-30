// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3.Model;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.RequestHandles
{
    internal sealed class AO3AuthorRequestHandle : RequestHandleBase<IAuthor, AO3Source>, IAuthorRequestHandle
    {
        public AO3AuthorRequestHandle( AO3Source source, string username, string pseud )
            : base( source )
        {
            Username = username;
            Pseud = pseud;
        }

        /// <inheritdoc />
        public string Username { get; }

        public string Pseud { get; }

        /// <inheritdoc />
        protected override string RequestUri => $"http://archiveofourown.org/users/{Username}/profile";

        /// <inheritdoc />
        protected override string RequestCacheHandle => $"ao3-author-{Username}";

        internal static AO3AuthorRequestHandle Parse( AO3Source source, HtmlNode authorA )
        {
            string[] hrefPieces = authorA.GetAttributeValue( "href", null ).Split( '/', '\\' );
            string username = hrefPieces[2];
            string pseud = ( hrefPieces.Length >= 5 ? hrefPieces[4] : null );
            return new AO3AuthorRequestHandle( source, username, pseud );
        }

        /// <inheritdoc />
        protected override IAuthor ParseRequest( HtmlCacheableDocument requestDocument )
        {
            return AO3Author.Parse( Source, requestDocument );
        }
    }
}
