using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Alexandria;
using Alexandria.Model;
using Bibliothecary.Core.Utils;
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

			IEnumerable<BinaryFile> photos = CreatePostPhotos( fanfic, author );
			String caption = $"Read the fanfic on AO3 <a href=\"{fanfic.Url}\" target=\"_blank\">here</a>";
			PostData post = PostData.CreatePhoto( photos, caption, fanfic.Url.AbsoluteUri, tags );
			post.State = ( ArePostsQueued ? PostCreationState.Queue : PostCreationState.Published );
			post.Format = PostFormat.Html;
			client.CreatePostAsync( BlogName, post );
		}

		static IReadOnlyList<BinaryFile> CreatePostPhotos( IFanfic fanfic, IAuthor author )
		{
			using ( Bitmap workspace = new Bitmap( TumblrPhotosetUtils.TumblrPhotoWidth, PhotosetWorkspaceHeight ) )
			{
				Single currentY = PhotosetPadding;
				using ( Graphics g = Graphics.FromImage( workspace ) )
				{
					g.TextRenderingHint = TextRenderingHint.AntiAlias;
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.CompositingMode = CompositingMode.SourceCopy;
					g.CompositingQuality = CompositingQuality.HighQuality;

					DrawPhotosetHeader( g, ref currentY, fanfic, author );
				}

				return TumblrPhotosetUtils.SliceForPhotoset( workspace, (Int32) Math.Ceiling( currentY ) ).ToList();
			}
		}

		static void DrawPhotosetHeader( Graphics g, ref Single currentY, IFanfic fanfic, IAuthor author )
		{
			// Title
			Font titleFont = TumblrPhotosetUtils.GetFont( TumblrPhotosetFont.Title, 24 );
			Single titleTextHeight = g.MeasureString( fanfic.Title, titleFont, new SizeF( UsableSpaceWidth, 0 ) ).Height;
			g.DrawString( fanfic.Title, titleFont, Brushes.Black, new RectangleF( PhotosetPadding, currentY, UsableSpaceWidth, titleTextHeight ), _horizontalCenterAlignment );
			currentY += titleTextHeight;

			// Author
			Font authorFont = TumblrPhotosetUtils.GetFont( TumblrPhotosetFont.Title, 11, FontStyle.Italic );
			Single authorTextHeight = g.MeasureString( author.Name, authorFont, new SizeF( UsableSpaceWidth, 0 ) ).Height;
			g.DrawString( author.Name, authorFont, Brushes.Gray, new RectangleF( PhotosetPadding, currentY, UsableSpaceWidth, authorTextHeight ), _horizontalCenterAlignment );
			currentY += authorTextHeight;
			currentY += PhotosetPadding;
		}

		static readonly TumblrClientFactory _clientFactory = new TumblrClientFactory();
		const Int32 PhotosetPadding = 6;
		const Int32 MaxNumberTumblrPhotos = 3;
		const Int32 PhotosetWorkspaceHeight = TumblrPhotosetUtils.TumblrPhotoHeight * MaxNumberTumblrPhotos;
		const Int32 UsableSpaceWidth = TumblrPhotosetUtils.TumblrPhotoWidth - PhotosetPadding * 2;
		static readonly StringFormat _horizontalCenterAlignment = new StringFormat
		{
			Alignment = StringAlignment.Center,
			Trimming = StringTrimming.None
		};
	}
}
