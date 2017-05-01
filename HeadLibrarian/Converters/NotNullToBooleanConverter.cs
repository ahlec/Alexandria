using System;
using System.Globalization;
using System.Windows.Data;

namespace HeadLibrarian.Converters
{
	public class NotNullToBooleanConverter : IValueConverter
	{
		public Object Convert( Object value, Type targetType, Object converterParameter, CultureInfo culture )
		{
			return ( value != null );
		}

		public Object ConvertBack( Object value, Type targetType, Object converterParameter, CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}
}
