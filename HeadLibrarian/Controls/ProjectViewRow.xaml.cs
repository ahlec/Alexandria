using System;
using System.Windows;
using System.Windows.Markup;

namespace HeadLibrarian.Controls
{
	[ContentProperty( nameof( FieldEditor ) )]
	public partial class ProjectViewRow
	{
		public ProjectViewRow()
		{
			InitializeComponent();
		}

		public String FieldName
		{
			get => GetValue( FieldNameProperty ) as String;
			set => SetValue( FieldNameProperty, value );
		}

		public Boolean IsFieldEnabled
		{
			get => (Boolean) GetValue( IsFieldEnabledProperty );
			set => SetValue( IsFieldEnabledProperty, value );
		}

		public FrameworkElement FieldEditor
		{
			get => GetValue( FieldEditorProperty ) as FrameworkElement;
			set => SetValue( FieldEditorProperty, value );
		}

		public static readonly DependencyProperty FieldNameProperty = DependencyProperty.Register( "FieldName", typeof( String ), typeof( ProjectViewRow ) );
		public static readonly DependencyProperty IsFieldEnabledProperty = DependencyProperty.Register( "IsFieldEnabled", typeof( Boolean ), typeof( ProjectViewRow ) );
		public static readonly DependencyProperty FieldEditorProperty = DependencyProperty.Register( "FieldEditor", typeof( FrameworkElement ), typeof( ProjectViewRow ) );
	}
}
