using System;
using System.Windows;
using System.Windows.Controls;

namespace HeadLibrarian.Controls
{
	internal sealed class ValueNullTemplateSelector : DataTemplateSelector
	{
		/// <inheritdoc />
		public override DataTemplate SelectTemplate( Object item, DependencyObject container )
		{
			return ( item == null ? NullTemplate : NotNullTemplate );
		}

		public DataTemplate NullTemplate { get; set; }
		public DataTemplate NotNullTemplate { get; set; }
	}
}
