using System;
using System.Globalization;
using System.Windows.Data;

namespace HeadLibrarian.Converters
{
	public class BooleanToStringConverter : IValueConverter
	{
		public Object Convert( Object value, Type targetType, Object converterParameter, CultureInfo culture )
		{
			if ( !( value is Boolean ) )
			{
				throw new ArgumentException();
			}

			return ( (Boolean) value ? TrueString : FalseString );
		}

		public Object ConvertBack( Object value, Type targetType, Object converterParameter, CultureInfo culture )
		{
			throw new NotSupportedException();
		}

		public String TrueString { get; set; }

		public String FalseString { get; set; }
	}
}
