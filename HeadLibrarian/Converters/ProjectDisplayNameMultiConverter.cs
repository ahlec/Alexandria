using System;
using System.Globalization;
using System.Windows.Data;

namespace HeadLibrarian.Converters
{
	public class ProjectDisplayNameMultiConverter : IMultiValueConverter
	{
		public Object Convert( Object[] values, Type targetType, Object converterParameter, CultureInfo culture )
		{
			if ( !( values[0] is String ) )
			{
				throw new ArgumentException( nameof( values ) );
			}
			if ( !( values[1] is Boolean ) )
			{
				throw new ArgumentException( nameof( values ) );
			}
			String projectName = (String) values[0];
			Boolean hasUnsavedChanges = (Boolean) values[1];

			if ( !hasUnsavedChanges )
			{
				return projectName;
			}

			return String.Concat( projectName, "*" );
		}

		public Object[] ConvertBack( Object value, Type[] targetTypes, Object converterParameter, CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}
}
