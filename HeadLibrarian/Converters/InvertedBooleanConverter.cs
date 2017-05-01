using System;
using System.Globalization;
using System.Windows.Data;

namespace HeadLibrarian.Converters
{
	public class InvertedBooleanConverter : IValueConverter
	{
		public Object Convert( Object value, Type targetType, Object converterParameter, CultureInfo culture )
		{
			if ( !( value is Boolean ) )
			{
				throw new ArgumentException();
			}

			return !(Boolean) value;
		}

		public Object ConvertBack( Object value, Type targetType, Object converterParameter, CultureInfo culture )
		{
			if ( !( value is Boolean ) )
			{
				throw new ArgumentException();
			}

			return !(Boolean) value;
		}
	}
}
