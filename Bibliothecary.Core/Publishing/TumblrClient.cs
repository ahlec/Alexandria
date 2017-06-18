using System;
using System.Collections.Generic;
using System.Text;
using Alexandria;
using Alexandria.Model;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.OAuth;
using TumblrSharpTumblrClient = DontPanic.TumblrSharp.Client.TumblrClient;

namespace Bibliothecary.Core.Publishing
{
	public sealed class TumblrClient
	{
		public String ConsumerKey { get; set; }

		public String ConsumerSecret { get; set; }

		public String OauthToken { get; set; }

		public String OauthTokenSecret { get; set; }

		public String BlogName { get; set; }

		public Boolean ArePostsQueued { get; set; }

		public IReadOnlyList<TumblrTagRule> TagRules { get; set; }

		public void Post( IFanfic fanfic, LibrarySource source )
		{
			IAuthor author = source.MakeRequest( fanfic.Author );
			StringBuilder builder = new StringBuilder();
			builder.AppendLine( $"<strong>Title:</strong> {fanfic.Title}<br />" );
			builder.AppendLine( $"<strong>Author:</strong> <a href=\"{author.Url}\" target=\"_blank\">{author.Name}</a><br />" );
			builder.AppendLine( $"<strong>Date:</strong> {fanfic.DateLastUpdated}" );
			builder.AppendLine( $"<strong>Word Count:</strong> {fanfic.NumberWords}" );

			HashSet<String> tags = new HashSet<String>( StringComparer.CurrentCultureIgnoreCase );
			foreach ( TumblrTagRule tagRule in TagRules )
			{
				tags.Add( tagRule.Tag );
			}

			TumblrSharpTumblrClient client = _clientFactory.Create<TumblrSharpTumblrClient>( ConsumerKey, ConsumerSecret, new Token( OauthToken, OauthTokenSecret ) );
			PostData post = PostData.CreateText( builder.ToString(), fanfic.Title, tags );
			post.State = ( ArePostsQueued ? PostCreationState.Queue : PostCreationState.Published );
			post.Format = PostFormat.Html;
			client.CreatePostAsync( BlogName, post );
		}

		static readonly TumblrClientFactory _clientFactory = new TumblrClientFactory();
	}
}
