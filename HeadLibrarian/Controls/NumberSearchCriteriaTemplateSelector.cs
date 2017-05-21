using System;
using System.Windows;
using System.Windows.Controls;
using Alexandria.Searching;

namespace HeadLibrarian.Controls
{
	internal sealed class NumberSearchCriteriaTemplateSelector : DataTemplateSelector
	{
		/// <inheritdoc />
		public override DataTemplate SelectTemplate( Object item, DependencyObject container )
		{
			if ( item == null )
			{
				return null;
			}

			if ( !( item is NumberSearchCriteriaType ) )
			{
				throw new ArgumentException();
			}

			NumberSearchCriteriaType type = (NumberSearchCriteriaType) item;
			switch ( type )
			{
				case NumberSearchCriteriaType.ExactMatch:
					return ExactDataTemplate;
				case NumberSearchCriteriaType.LessThan:
					return LessThanDataTemplate;
				case NumberSearchCriteriaType.GreaterThan:
					return GreaterThanDataTemplate;
				case NumberSearchCriteriaType.Range:
					return RangeDataTemplate;
				default:
					throw new NotImplementedException();
			}
		}

		public DataTemplate ExactDataTemplate { get; set; }
		public DataTemplate LessThanDataTemplate { get; set; }
		public DataTemplate GreaterThanDataTemplate { get; set; }
		public DataTemplate RangeDataTemplate { get; set; }
	}
}
