using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HeadLibrarian.Converters
{
	public class InvertedBooleanToVisibilityConverter : IValueConverter
	{
		public Object Convert( Object value, Type targetType, Object converterParameter, CultureInfo culture )
		{
			if ( !( value is Boolean ) )
			{
				throw new ArgumentException();
			}

			return ( (Boolean) value ? Visibility.Collapsed : Visibility.Visible );
		}

		public Object ConvertBack( Object value, Type targetType, Object converterParameter, CultureInfo culture )
		{
			if ( !( value is Visibility ) )
			{
				throw new ArgumentException();
			}

			return ( (Visibility) value == Visibility.Collapsed );
		}
	}
}
