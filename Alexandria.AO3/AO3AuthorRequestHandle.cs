using System;
using Alexandria.Model;
using HtmlAgilityPack;

namespace Alexandria.AO3
{
	public sealed class AO3AuthorRequestHandle : IRequestHandle<IAuthor>
	{
		public AO3AuthorRequestHandle( String username, String pseud )
		{
			Username = username;
			Pseud = pseud;
		}

		public String Username { get; }

		public String Pseud { get; }

		internal static AO3AuthorRequestHandle Parse( HtmlNode authorA )
		{
			String[] hrefPieces = authorA.GetAttributeValue( "href", null ).Split( '/', '\\' );
			String username = hrefPieces[2];
			String pseud = ( hrefPieces.Length >= 5 ? hrefPieces[4] : null );

			return new AO3AuthorRequestHandle( username, pseud );
		}
	}
}
