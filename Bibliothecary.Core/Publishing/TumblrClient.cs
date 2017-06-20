//#define POST_AS_TEXT

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
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

			HashSet<String> tags = new HashSet<String>( StringComparer.CurrentCultureIgnoreCase );
			foreach ( TumblrTagRule tagRule in TagRules )
			{
				tags.Add( tagRule.Tag );
			}

			TumblrSharpTumblrClient client = _clientFactory.Create<TumblrSharpTumblrClient>( ConsumerKey, ConsumerSecret, new Token( OauthToken, OauthTokenSecret ) );

#if POST_AS_TEXT
			String body = ComposePostBox( fanfic, author );
			PostData post = PostData.CreateText( body, null, tags );
#else
			IEnumerable<BinaryFile> photos = CreatePostPhotos( fanfic, author );
			String caption = $"Read the fanfic on AO3 <a href=\"{fanfic.Url}\" target=\"_blank\">here</a>";
			PostData post = PostData.CreatePhoto( photos, caption, fanfic.Url.AbsoluteUri, tags );
#endif
			post.State = ( ArePostsQueued ? PostCreationState.Queue : PostCreationState.Published );
			post.Format = PostFormat.Html;
			client.CreatePostAsync( BlogName, post );
		}

#if POST_AS_TEXT
		static String ComposePostBox( IFanfic fanfic, IAuthor author )
		{
			StringBuilder body = new StringBuilder();
			body.AppendLine( $"<a href=\"{fanfic.Url}\" target=\"_blank\"><h2>{fanfic.Title}</h2></a>" );
			body.AppendLine( "<br />" );
			body.AppendLine( $"<i><small>by <a href=\"{author.Url}\" target=\"_blank\">{author.Name}</a></small></i>" );
			body.AppendLine( "<hr />" );
			body.Append( "<p><small>" );
			body.Append( $"<strong>{fanfic.Rating}</strong> &bull; " );
			if ( fanfic.ContentWarnings != ContentWarnings.None )
			{
				foreach ( ContentWarnings warning in Enum.GetValues( typeof( ContentWarnings ) ) )
				{
					if ( fanfic.ContentWarnings.HasFlag( warning ) )
					{
						body.Append( $"{warning} &bull; " );
					}
				}
			}
			body.AppendLine( $"<strong>{fanfic.NumberWords.ToString( CultureInfo.InvariantCulture.NumberFormat )}</strong>" );
			body.AppendLine( "</small></p>" );
			body.AppendLine( "<hr />" );
			body.Append( "<blockquote>" );
			body.Append( fanfic.Summary );
			body.Append( "</blockquote>" );
			body.Append( $"<small><a href=\"{fanfic.Url}\" target=\"_blank\">Read on AO3</a></small>" );
			return body.ToString();
		}
#else
		static IReadOnlyList<BinaryFile> CreatePostPhotos( IFanfic fanfic, IAuthor author )
		{
			using ( TumblrPhotosetGenerator generator = new TumblrPhotosetGenerator() )
			{
				Int32 currentY = PhotosetPadding;
				currentY = generator.DrawCenteredWrappingText( PhotosetPadding, currentY,
					TumblrPhotosetGenerator.TumblrPhotoWidth - PhotosetPadding * 2, fanfic.Title, TumblrPhotosetFont.Title, 24,
					Brushes.Black ).BottomY;
				currentY = generator.DrawHorizontalLine( currentY + PhotosetPadding, Brushes.Black, 2 ).BottomY;
				return generator.BinaryFiles.ToList();
			}
		}
#endif

		static readonly TumblrClientFactory _clientFactory = new TumblrClientFactory();
		const Int32 PhotosetPadding = 6;
	}
}
