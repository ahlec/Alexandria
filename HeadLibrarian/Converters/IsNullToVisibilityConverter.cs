using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HeadLibrarian.Converters
{
	public class IsNullToVisibilityConverter : IValueConverter
	{
		public Object Convert( Object value, Type targetType, Object converterParameter, CultureInfo culture )
		{
			return ( value == null ? Visibility.Visible : Visibility.Collapsed );
		}

		public Object ConvertBack( Object value, Type targetType, Object converterParameter, CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}
}
