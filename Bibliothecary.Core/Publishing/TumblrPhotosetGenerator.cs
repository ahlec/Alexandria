using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using DontPanic.TumblrSharp;

namespace Bibliothecary.Core.Publishing
{
	internal sealed class TumblrPhotosetGenerator : IDisposable
	{
		class Photo : IDisposable
		{
			public Photo( Int32 photoNumber )
			{
				Bitmap = new Bitmap( TumblrPhotoWidth, TumblrPhotoHeight );
				Graphics = Graphics.FromImage( Bitmap );
				Graphics.FillRectangle( Brushes.White, 0, 0, TumblrPhotoWidth, TumblrPhotoHeight );
				Graphics.TranslateTransform( 0, -photoNumber * TumblrPhotoHeight );
				Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
				Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				Graphics.CompositingMode = CompositingMode.SourceCopy;
				Graphics.CompositingQuality = CompositingQuality.HighQuality;
			}

			~Photo()
			{
				Dispose( false );
			}

			public void Dispose()
			{
				Dispose( true );
				GC.SuppressFinalize( this );
			}

			void Dispose( Boolean disposing )
			{
				if ( !_isDisposed )
				{
					Graphics.Dispose();
					Bitmap.Dispose();
					_isDisposed = true;
				}
			}

			public Bitmap Bitmap { get; }
			public Graphics Graphics { get; }

			Boolean _isDisposed;
		}

		static TumblrPhotosetGenerator()
		{
			_fontCollection = new PrivateFontCollection();
			_fontCollection.AddFontFile( "Lato-Regular.ttf" );
			_fontCollection.AddFontFile( "SourceSansPro-Regular.ttf" );
		}

		public void Dispose()
		{
			if ( _isDisposed )
			{
				return;
			}

			foreach ( Photo photo in _photos )
			{
				photo.Dispose();
			}

			_isDisposed = true;
		}

		public IEnumerable<BinaryFile> BinaryFiles
		{
			get
			{
				for ( Int32 index = 0; index < _photos.Count - 1; ++index )
				{
					yield return ConvertBitmapToBinaryFile( _photos[index].Bitmap );
				}
				if ( _photos.Count > 0 )
				{
					using ( Bitmap trimmedBitmap = TrimWhitespaceFromEnd( _photos[_photos.Count - 1].Bitmap ) )
					{
						yield return ConvertBitmapToBinaryFile( trimmedBitmap );
					}
				}
			}
		}

		public VerticalBounds DrawText( Int32 x, Int32 y, String text, TumblrPhotosetFont font, Int32 size, Brush color )
		{
			Font drawingFont = GetFont( font, size );
			VerticalBounds bounds = new VerticalBounds
			{
				TopY = y,
				BottomY = y + drawingFont.Height
			};
			foreach ( Graphics g in GetGraphicsForBoundingBox( bounds ) )
			{
				g.DrawString( text, drawingFont, color, x, y );
			}
			return bounds;
		}

		public VerticalBounds DrawCenteredWrappingText( Int32 x, Int32 y, Int32 wrapWidth, String text, TumblrPhotosetFont font, Int32 size, Brush color )
		{
			Font drawingFont = GetFont( font, size );
			SizeF measureBounds = _measurePhoto.Graphics.MeasureString( text, drawingFont, new Size( wrapWidth, 0 ) );
			VerticalBounds bounds = new VerticalBounds
			{
				TopY = y,
				BottomY = y + (Int32) Math.Ceiling( measureBounds.Height )
			};
			RectangleF boundingBox = new RectangleF
			{
				X = x,
				Y = y,
				Width = wrapWidth,
				Height = measureBounds.Height
			};
			StringFormat stringFormat = new StringFormat
			{
				Alignment = StringAlignment.Center
			};
			foreach ( Graphics g in GetGraphicsForBoundingBox( bounds ) )
			{
				g.DrawString( text, drawingFont, color, boundingBox, stringFormat );
			}
			return bounds;
		}

		public VerticalBounds DrawHorizontalLine( Int32 y, Brush color, Int32 thickness )
		{
			const Int32 X1 = 20;
			const Int32 X2 = TumblrPhotoWidth - 20;
			Int32 topHalfOfThickness = (Int32) Math.Floor( thickness / 2.0f );
			VerticalBounds bounds = new VerticalBounds
			{
				TopY = y - topHalfOfThickness,
				BottomY = y + ( thickness - topHalfOfThickness )
			};
			foreach ( Graphics g in GetGraphicsForBoundingBox( bounds ) )
			{
				g.DrawLine( new Pen( color, thickness ), X1, y, X2, y );
			}
			return bounds;
		}

		static Font GetFont( TumblrPhotosetFont font, Int32 fontSize )
		{
			switch ( font )
			{
				case TumblrPhotosetFont.Title:
					{
						Font drawingFont;
						if ( !_titleFonts.TryGetValue( fontSize, out drawingFont ) )
						{
							drawingFont = CreateTitleFont( fontSize );
							_titleFonts.Add( fontSize, drawingFont );
						}
						return drawingFont;
					}
				case TumblrPhotosetFont.Body:
					{
						Font drawingFont;
						if ( !_bodyFonts.TryGetValue( fontSize, out drawingFont ) )
						{
							drawingFont = CreateBodyFont( fontSize );
							_bodyFonts.Add( fontSize, drawingFont );
						}
						return drawingFont;
					}
				default:
					throw new NotImplementedException();
			}
		}

		static Font CreateTitleFont( Int32 size )
		{
			return new Font( _fontCollection.Families[0], size );
		}

		static Font CreateBodyFont( Int32 size )
		{
			return new Font( _fontCollection.Families[1], size );
		}

		IEnumerable<Graphics> GetGraphicsForBoundingBox( VerticalBounds bounds )
		{
			Int32 photoStartIndex = (Int32) Math.Floor( bounds.TopY / (Single) TumblrPhotoHeight );
			Int32 photoEndIndex = (Int32) Math.Floor( bounds.BottomY / (Single) TumblrPhotoHeight );

			// Create any that are necessary now
			for ( Int32 index = _photos.Count; index <= photoEndIndex; ++index )
			{
				_photos.Add( new Photo( index ) );
			}

			// Return the appropriate Graphic(s)
			for ( Int32 index = photoStartIndex; index <= photoEndIndex; ++index )
			{
				yield return _photos[index].Graphics;
			}
		}

		static Bitmap TrimWhitespaceFromEnd( Bitmap bitmap )
		{
			Int32 yMax = TumblrPhotoHeight;
			BitmapData data = null;
			try
			{
				data = bitmap.LockBits( new Rectangle( 0, 0, bitmap.Width, bitmap.Height ), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb );
				Byte[] buffer = new Byte[data.Height * data.Stride];
				Marshal.Copy( data.Scan0, buffer, 0, buffer.Length );
				
				for ( Int32 y = data.Height - 1; y >= 0; --y )
				{
					Boolean stop = false;
					for ( Int32 x = 0; x < TumblrPhotoWidth; ++x )
					{
						Byte r = buffer[y * data.Stride + 4 * x];
						Byte g = buffer[y * data.Stride + 4 * x + 1];
						Byte b = buffer[y * data.Stride + 4 * x + 2];
						if ( r != 255 || g != 255 || b != 255 )
						{
							yMax = y;
							stop = true;
							break;
						}
					}
					if ( stop )
					{
						break;
					}
				}
			}
			finally
			{
				if ( data != null )
				{
					bitmap.UnlockBits( data );
				}
			}

			Bitmap trimmed = new Bitmap( TumblrPhotoWidth, yMax );
			using ( Graphics g = Graphics.FromImage( trimmed ) )
			{
				Rectangle contentRect = new Rectangle( 0, 0, trimmed.Width, trimmed.Height );
				g.DrawImage( bitmap, contentRect, contentRect, GraphicsUnit.Pixel );
			}
			return trimmed;
		}

		static BinaryFile ConvertBitmapToBinaryFile( Bitmap bitmap )
		{
			using ( MemoryStream stream = new MemoryStream() )
			{
				bitmap.Save( stream, ImageFormat.Png );
				stream.Position = 0;
				return new BinaryFile( stream );
			}
		}

		public const Int32 TumblrPhotoWidth = 500;
		const Int32 TumblrPhotoHeight = 750;
		static readonly PrivateFontCollection _fontCollection;
		static readonly Dictionary<Int32, Font> _titleFonts = new Dictionary<Int32, Font>();
		static readonly Dictionary<Int32, Font> _bodyFonts = new Dictionary<Int32, Font>();
		static readonly Photo _measurePhoto = new Photo( 0 ); // Used for measuring and other things and such
		readonly List<Photo> _photos = new List<Photo>();
		Boolean _isDisposed;
	}
}
