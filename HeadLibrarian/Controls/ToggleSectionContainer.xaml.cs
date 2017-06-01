using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Markup;

namespace HeadLibrarian.Controls
{
	[ContentProperty( nameof( Contents ) )]
	public partial class ToggleSectionContainer
	{
		public ToggleSectionContainer()
		{
			InitializeComponent();
		}

		public String Header
		{
			get => (String) GetValue( HeaderProperty );
			set => SetValue( HeaderProperty, value );
		}

		public Boolean IsSectionEnabled
		{
			get => (Boolean) GetValue( IsSectionEnabledProperty );
			set => SetValue( IsSectionEnabledProperty, value );
		}

		public ObservableCollection<FrameworkElement> Contents { get; } = new ObservableCollection<FrameworkElement>();

		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register( "Header", typeof( String ), typeof( ToggleSectionContainer ) );
		public static readonly DependencyProperty IsSectionEnabledProperty = DependencyProperty.Register( "IsSectionEnabled", typeof( Boolean ), typeof( ToggleSectionContainer ) );
	}
}
