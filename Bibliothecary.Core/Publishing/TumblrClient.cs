using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using Alexandria;
using Alexandria.Model;
using Alexandria.Utils;
using Bibliothecary.Core.Utils;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.OAuth;
using TumblrSharpTumblrClient = DontPanic.TumblrSharp.Client.TumblrClient;

namespace Bibliothecary.Core.Publishing
{
	public sealed class TumblrClient
	{
		class TumblrPhotosetDetail
		{
			public TumblrPhotosetDetail( Graphics g, Font font, String text, Boolean isSeparator = false )
			{
				Text = text;
				IsIncluded = !String.IsNullOrWhiteSpace( Text );
				Font = font;
				SizeF size = g.MeasureString( text, font );
				Width = size.Width;
				Height = size.Height;
				if ( Width > UsableSpaceWidth )
				{
					throw new NotImplementedException( "TODO: The ability to split an individual piece of text across multiple lines is not implemented yet (do we even need it? Maybe for like OT4/OT5/etc)" );
				}
				IsSeparator = isSeparator;
			}

			public String Text { get; }

			public Boolean IsIncluded { get; }

			public Font Font { get; }

			public Single Width { get; }

			public Single Height { get; }

			public Boolean IsSeparator { get; }
		}

		class TumblrPhotosetDetailLine : IEnumerable<TumblrPhotosetDetail>
		{
			public Single Width => _list.Sum( detail => detail.Width );

			public Single Height => _list.Max( detail => detail.Height );

			public Boolean CanAdd( TumblrPhotosetDetail detail )
			{
				return ( Width + detail.Width <= UsableSpaceWidth );
			}

			public void Add( TumblrPhotosetDetail detail )
			{
				if ( detail == null )
				{
					throw new ArgumentNullException( nameof( detail ) );
				}

				if ( !CanAdd( detail ) )
				{
					throw new InvalidOperationException();
				}

				_list.Add( detail );
			}

			public void TrimSeparators()
			{
				while ( _list.Count > 0 )
				{
					if ( !_list[_list.Count - 1].IsSeparator )
					{
						break;
					}

					_list.RemoveAt( _list.Count - 1 );
				}
			}

			public IEnumerator<TumblrPhotosetDetail> GetEnumerator()
			{
				return _list.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return _list.GetEnumerator();
			}

			readonly List<TumblrPhotosetDetail> _list = new List<TumblrPhotosetDetail>();
		}

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
					DrawPhotosetSummary( g, ref currentY, fanfic );
					DrawPhotosetDetails( g, ref currentY, fanfic );
				}

				return TumblrPhotosetUtils.SliceForPhotoset( workspace, (Int32) Math.Ceiling( currentY ) ).ToList();
			}
		}

		static void DrawPhotosetHeader( Graphics g, ref Single currentY, IFanfic fanfic, IAuthor author )
		{
			Brush titleBrush = new SolidBrush( Color.FromArgb( 42, 42, 42 ) );
			Brush authorBrush = new SolidBrush( Color.FromArgb( 17, 17, 17 ) );

			// Title
			Font titleFont = TumblrPhotosetUtils.GetFont( TumblrPhotosetFont.Title, 20 );
			Single titleTextHeight = g.MeasureString( fanfic.Title, titleFont, new SizeF( UsableSpaceWidth, 0 ) ).Height;
			g.DrawString( fanfic.Title, titleFont, titleBrush, new RectangleF( PhotosetPadding, currentY, UsableSpaceWidth, titleTextHeight ), _horizontalCenterAlignment );
			currentY += titleTextHeight;

			// Author
			Font authorFont = TumblrPhotosetUtils.GetFont( TumblrPhotosetFont.Title, 12, FontStyle.Italic );
			Single authorTextHeight = g.MeasureString( author.Name, authorFont, new SizeF( UsableSpaceWidth, 0 ) ).Height;
			g.DrawString( author.Name, authorFont, authorBrush, new RectangleF( PhotosetPadding, currentY, UsableSpaceWidth, authorTextHeight ), _horizontalCenterAlignment );
			currentY += authorTextHeight;
			currentY += PhotosetPadding;
		}

		static void DrawPhotosetSummary( Graphics g, ref Single currentY, IFanfic fanfic )
		{
			const Int32 SummaryWidth = UsableSpaceWidth - 100;
			const Int32 LineLeft = UsableSpaceWidth / 2 - SummaryWidth / 2 + ( TumblrPhotosetUtils.TumblrPhotoWidth - UsableSpaceWidth ) / 2;
			const Int32 LineRight = LineLeft + SummaryWidth;
			Brush darkBrush = new SolidBrush( Color.FromArgb( 42, 42, 42 ) );
			Pen linePen = new Pen( darkBrush, 1 );

			g.DrawLine( linePen, LineLeft, currentY, LineRight, currentY );
			currentY += PhotosetPadding;

			if ( String.IsNullOrWhiteSpace( fanfic.Summary ) )
			{
				return;
			}

			Font summaryFont = TumblrPhotosetUtils.GetFont( TumblrPhotosetFont.Body, 9 );
			Single summaryHeight = g.MeasureString( fanfic.Summary, summaryFont, new SizeF( SummaryWidth, 0 ) ).Height;
			g.DrawString( fanfic.Summary, summaryFont, darkBrush, new RectangleF( LineLeft, currentY, SummaryWidth, summaryHeight ), _horizontalCenterAlignment );
			currentY += summaryHeight;
			currentY += PhotosetPadding;

			g.DrawLine( linePen, LineLeft, currentY, LineRight, currentY );
			currentY += PhotosetPadding;
		}

		static void DrawPhotosetDetails( Graphics g, ref Single currentY, IFanfic fanfic )
		{
			const Int32 FontSize = 9;

			Font ratingFont = TumblrPhotosetUtils.GetFont( TumblrPhotosetFont.Body, FontSize, FontStyle.Bold );
			Font regularFont = TumblrPhotosetUtils.GetFont( TumblrPhotosetFont.Body, FontSize );

			// Get all of the strings
			TumblrPhotosetDetail separator = new TumblrPhotosetDetail( g, regularFont, " • ", true );
			TumblrPhotosetDetail rating = new TumblrPhotosetDetail( g, ratingFont, fanfic.Rating.GetDisplayName() );
			TumblrPhotosetDetail wordCount = new TumblrPhotosetDetail( g, regularFont,
				String.Concat( fanfic.NumberWords.ToString( "N0", CultureInfo.CurrentCulture ), " word", ( fanfic.NumberWords != 1 ? "s" : null ) ) );
			String chapterText = null;
			if ( fanfic.ChapterInfo != null && fanfic.ChapterInfo.TotalNumberChapters != 1 )
			{
				chapterText = String.Concat( fanfic.ChapterInfo.ChapterNumber, "/", fanfic.ChapterInfo.TotalNumberChapters, " chapters" );
			}
			TumblrPhotosetDetail chapter = new TumblrPhotosetDetail( g, regularFont, chapterText );
			TumblrPhotosetDetail mainRelationship = new TumblrPhotosetDetail( g, regularFont, ( fanfic.Ships.Count > 0 ? fanfic.Ships[0].ShipTag : null ) );

			// Create the lines
			List<TumblrPhotosetDetailLine> lines = new List<TumblrPhotosetDetailLine>
			{
				new TumblrPhotosetDetailLine()
			};
			AddDetailToLines( lines, rating, separator, false );
			AddDetailToLines( lines, wordCount, separator );
			AddDetailToLines( lines, chapter, separator );
			AddDetailToLines( lines, mainRelationship, separator );
			foreach ( TumblrPhotosetDetailLine line in lines )
			{
				line.TrimSeparators();
			}

			// Draw the lines
			Brush darkBrush = new SolidBrush( Color.FromArgb( 42, 42, 42 ) );
			foreach ( TumblrPhotosetDetailLine line in lines )
			{
				Single startX = UsableSpaceWidth / 2.0f - line.Width / 2.0f;

				Single currentX = startX;
				foreach ( TumblrPhotosetDetail detail in line )
				{
					Single yOffset = line.Height - detail.Height;
					g.DrawString( detail.Text, detail.Font, darkBrush, currentX, currentY + yOffset );
					currentX += detail.Width;
				}

				currentY += line.Height;
			}

			currentY += PhotosetPadding;
		}

		static void AddDetailToLines( IList<TumblrPhotosetDetailLine> lines, TumblrPhotosetDetail detail, TumblrPhotosetDetail separator, Boolean goToNextLineIfDoesntFit = true )
		{
			if ( !detail.IsIncluded )
			{
				return;
			}

			Boolean canAddToLine = lines[lines.Count - 1].CanAdd( detail );
			if ( canAddToLine )
			{
				lines[lines.Count - 1].Add( detail );
				if ( lines[lines.Count - 1].CanAdd( separator ) )
				{
					lines[lines.Count - 1].Add( separator );
				}
				return;
			}

			if ( !goToNextLineIfDoesntFit )
			{
				throw new ApplicationException( "Unable to fit a detail into any lines." );
			}

			lines.Add( new TumblrPhotosetDetailLine() );
			AddDetailToLines( lines, detail, separator, false );
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
