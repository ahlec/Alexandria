using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using HeadLibrarian.ViewModels;

namespace HeadLibrarian.Converters
{
	public class SaveProjectButtonMultiConverter : IMultiValueConverter
	{
		public Object Convert( Object[] values, Type targetType, Object converterParameter, CultureInfo culture )
		{
			if ( values[0] == null )
			{
				return false;
			}

			if ( !( values[0] is ProjectViewModel ) )
			{
				throw new ArgumentException( nameof( values ) );
			}

			if ( values[1] == DependencyProperty.UnsetValue )
			{
				return false;
			}

			if ( !( values[1] is Boolean ) )
			{
				throw new ArgumentException( nameof( values ) );
			}

			Boolean hasUnsavedChanges = (Boolean) values[1];
			return hasUnsavedChanges;
		}

		public Object[] ConvertBack( Object value, Type[] targetTypes, Object converterParameter, CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}
}
