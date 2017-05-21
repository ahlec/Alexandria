using System;
using System.Windows;
using System.Windows.Controls;
using Alexandria.Searching;

namespace HeadLibrarian.Controls
{
	internal sealed class DateSearchCriteriaTemplateSelector : DataTemplateSelector
	{
		/// <inheritdoc />
		public override DataTemplate SelectTemplate( Object item, DependencyObject container )
		{
			if ( item == null )
			{
				return null;
			}

			if ( !( item is DateSearchCriteriaType ) )
			{
				throw new ArgumentException();
			}

			DateSearchCriteriaType type = (DateSearchCriteriaType) item;
			switch ( type )
			{
				case DateSearchCriteriaType.Exactly:
					return ExactlyDataTemplate;
				case DateSearchCriteriaType.Before:
					return BeforeDataTemplate;
				case DateSearchCriteriaType.After:
					return AfterDataTemplate;
				case DateSearchCriteriaType.Between:
					return BetweenDataTemplate;
				default:
					throw new NotImplementedException();
			}
		}

		public DataTemplate ExactlyDataTemplate { get; set; }
		public DataTemplate BeforeDataTemplate { get; set; }
		public DataTemplate AfterDataTemplate { get; set; }
		public DataTemplate BetweenDataTemplate { get; set; }
	}
}
