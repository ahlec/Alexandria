using System;
using System.Windows;
using System.Windows.Controls;
using HeadLibrarian.ViewModels;

namespace HeadLibrarian.Controls
{
	internal sealed class SearchCriteriaTemplateSelector : DataTemplateSelector
	{
		/// <inheritdoc />
		public override DataTemplate SelectTemplate( Object item, DependencyObject container )
		{
			if ( item == null )
			{
				return null;
			}

			if ( !( item is SearchCriteriaViewModelType ) )
			{
				throw new ArgumentException();
			}

			SearchCriteriaViewModelType type = (SearchCriteriaViewModelType) item;
			switch ( type )
			{
				case SearchCriteriaViewModelType.None:
					return NoneTemplate;
				case SearchCriteriaViewModelType.ExactMatch:
					return ExactDataTemplate;
				case SearchCriteriaViewModelType.LessThan:
					return LessThanDataTemplate;
				case SearchCriteriaViewModelType.LessThanOrEqual:
					return LessThanOrEqualDataTemplate;
				case SearchCriteriaViewModelType.GreaterThan:
					return GreaterThanDataTemplate;
				case SearchCriteriaViewModelType.GreaterThanOrEqual:
					return GreaterThanOrEqualDataTemplate;
				case SearchCriteriaViewModelType.Range:
					return RangeDataTemplate;
				default:
					throw new NotImplementedException();
			}
		}

		public DataTemplate NoneTemplate { get; set; }
		public DataTemplate ExactDataTemplate { get; set; }
		public DataTemplate LessThanDataTemplate { get; set; }
		public DataTemplate LessThanOrEqualDataTemplate { get; set; }
		public DataTemplate GreaterThanDataTemplate { get; set; }
		public DataTemplate GreaterThanOrEqualDataTemplate { get; set; }
		public DataTemplate RangeDataTemplate { get; set; }
	}
}
