// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.RequestHandles
{
    internal sealed class AO3AuthorRequestHandle : IAuthorRequestHandle
    {
        public AO3AuthorRequestHandle( string username, string pseud )
        {
            Username = username;
            Pseud = pseud;
        }

        public string Username { get; }

        public string Pseud { get; }

        internal static AO3AuthorRequestHandle Parse( HtmlNode authorA )
        {
            string[] hrefPieces = authorA.GetAttributeValue( "href", null ).Split( '/', '\\' );
            string username = hrefPieces[2];
            string pseud = ( hrefPieces.Length >= 5 ? hrefPieces[4] : null );

            return new AO3AuthorRequestHandle( username, pseud );
        }
    }
}
