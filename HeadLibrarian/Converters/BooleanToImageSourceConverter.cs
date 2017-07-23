using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HeadLibrarian.Converters
{
	public class BooleanToImageSourceConverter : IValueConverter
	{
		public Object Convert( Object value, Type targetType, Object converterParameter, CultureInfo culture )
		{
			if ( !( value is Boolean ) )
			{
				throw new ArgumentException();
			}

			return ( (Boolean) value ? TrueImage : FalseImage );
		}

		public Object ConvertBack( Object value, Type targetType, Object converterParameter, CultureInfo culture )
		{
			throw new NotSupportedException();
		}

		public ImageSource TrueImage { get; set; }

		public ImageSource FalseImage { get; set; }
	}
}
