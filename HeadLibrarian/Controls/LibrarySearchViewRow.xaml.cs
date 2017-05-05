using System;
using System.Windows;
using System.Windows.Markup;

namespace HeadLibrarian.Controls
{
	[ContentProperty( nameof( FieldEditor ) )]
	public partial class LibrarySearchViewRow
	{
		public LibrarySearchViewRow()
		{
			InitializeComponent();
		}

		public String FieldName
		{
			get => GetValue( FieldNameProperty ) as String;
			set => SetValue( FieldNameProperty, value );
		}

		public FrameworkElement FieldEditor
		{
			get => GetValue( FieldEditorProperty ) as FrameworkElement;
			set => SetValue( FieldEditorProperty, value );
		}

		public static readonly DependencyProperty FieldNameProperty = DependencyProperty.Register( "FieldName", typeof( String ), typeof( LibrarySearchViewRow ) );
		public static readonly DependencyProperty FieldEditorProperty = DependencyProperty.Register( "FieldEditor", typeof( FrameworkElement ), typeof( LibrarySearchViewRow ) );
	}
}
