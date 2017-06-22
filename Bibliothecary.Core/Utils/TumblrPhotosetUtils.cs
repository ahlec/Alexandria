using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using DontPanic.TumblrSharp;

namespace Bibliothecary.Core.Utils
{
	internal static class TumblrPhotosetUtils
	{
		class CreatedFontKey
		{
			public CreatedFontKey( Int32 emSize, FontStyle style )
			{
				_emSize = emSize;
				_style = style;
			}

			/// <inheritdoc />
			public override Boolean Equals( Object obj )
			{
				CreatedFontKey other = obj as CreatedFontKey;
				if ( other == null )
				{
					return false;
				}

				return ( _emSize == other._emSize && _style == other._style );
			}

			/// <inheritdoc />
			public override Int32 GetHashCode()
			{
				Int32 hashCode = 17;
				hashCode += 31 * _emSize.GetHashCode();
				hashCode += 31 * _style.GetHashCode();
				return hashCode;
			}

			readonly Int32 _emSize;
			readonly FontStyle _style;
		}

		static TumblrPhotosetUtils()
		{
			_fontCollection = new PrivateFontCollection();
			_fontCollection.AddFontFile( "Lato-Regular.ttf" );
			_fontCollection.AddFontFile( "SourceSansPro-Regular.ttf" );
		}

		public static IEnumerable<BinaryFile> SliceForPhotoset( Bitmap bitmap, Int32 bitmapHeight )
		{
			if ( bitmap.Width > TumblrPhotoWidth )
			{
				throw new ArgumentException( $"{nameof( TumblrPhotosetUtils )} is not equipped for horizontal slicing." );
			}

			bitmapHeight = GetTrimmedHeight( bitmap, bitmapHeight );

			Int32 numTotalPhotos = (Int32) Math.Ceiling( bitmap.Height / (Single) TumblrPhotoHeight );
			for ( Int32 photoNumber = 0; photoNumber < numTotalPhotos; ++photoNumber )
			{
				Int32 subsectionHeight = Math.Min( TumblrPhotoHeight, bitmapHeight - photoNumber * TumblrPhotoHeight );
				if ( subsectionHeight <= 0 )
				{
					yield break;
				}

				using ( Bitmap subsection = new Bitmap( bitmap.Width, subsectionHeight ) )
				{
					using ( Graphics g = Graphics.FromImage( subsection ) )
					{
						Rectangle sourceRectangle = new Rectangle( 0, photoNumber * TumblrPhotoHeight, bitmap.Width, subsectionHeight );
						Rectangle destRectangle = new Rectangle( 0, 0, subsection.Width, subsection.Height );
						g.DrawImage( bitmap, destRectangle, sourceRectangle, GraphicsUnit.Pixel );
					}

					yield return ConvertBitmapToBinaryFile( subsection );
				}
			}
		}

		public static Font GetFont( TumblrPhotosetFont font, Int32 fontSize, FontStyle fontStyle = FontStyle.Regular )
		{
			CreatedFontKey key = new CreatedFontKey( fontSize, fontStyle );
			switch ( font )
			{
				case TumblrPhotosetFont.Title:
					{
						Font drawingFont;
						if ( !_titleFonts.TryGetValue( key, out drawingFont ) )
						{
							drawingFont = new Font( _fontCollection.Families[0], fontSize, fontStyle );
							_titleFonts.Add( key, drawingFont );
						}
						return drawingFont;
					}
				case TumblrPhotosetFont.Body:
					{
						Font drawingFont;
						if ( !_bodyFonts.TryGetValue( key, out drawingFont ) )
						{
							drawingFont = new Font( _fontCollection.Families[1], fontSize, fontStyle );
							_bodyFonts.Add( key, drawingFont );
						}
						return drawingFont;
					}
				default:
					throw new NotImplementedException();
			}
		}

		static Int32 GetTrimmedHeight( Bitmap bitmap, Int32 startY )
		{
			BitmapData data = null;
			try
			{
				data = bitmap.LockBits( new Rectangle( 0, 0, bitmap.Width, bitmap.Height ), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb );
				Byte[] buffer = new Byte[data.Height * data.Stride];
				Marshal.Copy( data.Scan0, buffer, 0, buffer.Length );

				startY = Math.Min( startY, data.Height - 1 );
				for ( Int32 y = startY; y >= 0; --y )
				{
					for ( Int32 x = 0; x < data.Width; ++x )
					{
						Byte r = buffer[y * data.Stride + 4 * x];
						Byte g = buffer[y * data.Stride + 4 * x + 1];
						Byte b = buffer[y * data.Stride + 4 * x + 2];
						if ( r != 255 || g != 255 || b != 255 )
						{
							return y;
						}
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

			return 0;
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
		public const Int32 TumblrPhotoHeight = 750;

		static readonly PrivateFontCollection _fontCollection;
		static readonly Dictionary<CreatedFontKey, Font> _titleFonts = new Dictionary<CreatedFontKey, Font>();
		static readonly Dictionary<CreatedFontKey, Font> _bodyFonts = new Dictionary<CreatedFontKey, Font>();
	}
}
