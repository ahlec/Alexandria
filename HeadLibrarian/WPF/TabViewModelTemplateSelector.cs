using System;
using System.Windows;
using System.Windows.Controls;
using HeadLibrarian.ViewModels;

namespace HeadLibrarian.WPF
{
	public sealed class TabViewModelTemplateSelector : DataTemplateSelector
	{
		/// <inheritdoc />
		public override DataTemplate SelectTemplate( Object item, DependencyObject container )
		{
			switch ( item )
			{
				case IntroductionTabViewModel tab:
					{
						return IntroductionDataTemplate;
					}
				case ProjectViewModel tab:
					{
						return ProjectDataTemplate;
					}
				default:
					throw new NotSupportedException();
			}
		}

		public DataTemplate IntroductionDataTemplate { get; set; }

		public DataTemplate ProjectDataTemplate { get; set; }
	}
}
