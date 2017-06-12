using System;
using System.Windows;

namespace HeadLibrarian.Controls
{
	public partial class TimeSpanEditor
	{
		public TimeSpanEditor()
		{
			InitializeComponent();
		}

		public TimeSpan TimeSpan
		{
			get => (TimeSpan) GetValue( TimeSpanProperty );
			set => SetValue( TimeSpanProperty, value );
		}

		public Int32 Days
		{
			get => (Int32) GetValue( DaysProperty );
			set => SetValue( DaysProperty, value );
		}

		public Int32 Hours
		{
			get => (Int32) GetValue( HoursProperty );
			set => SetValue( HoursProperty, value );
		}

		public Int32 Minutes
		{
			get => (Int32) GetValue( MinutesProperty );
			set => SetValue( MinutesProperty, value );
		}

		public static readonly DependencyProperty TimeSpanProperty = DependencyProperty.Register( "TimeSpan", typeof( TimeSpan ),
			typeof( TimeSpanEditor ), new PropertyMetadata( OnTimeSpanChanged ) );
		public static readonly DependencyProperty DaysProperty = DependencyProperty.Register( "Days", typeof( Int32 ),
			typeof( TimeSpanEditor ), new PropertyMetadata( OnTimeSpanComponentChanged ) );
		public static readonly DependencyProperty HoursProperty = DependencyProperty.Register( "Hours", typeof( Int32 ),
			typeof( TimeSpanEditor ), new PropertyMetadata( OnTimeSpanComponentChanged ) );
		public static readonly DependencyProperty MinutesProperty = DependencyProperty.Register( "Minutes", typeof( Int32 ),
			typeof( TimeSpanEditor ), new PropertyMetadata( OnTimeSpanComponentChanged ) );

		static void OnTimeSpanChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			TimeSpanEditor editor = (TimeSpanEditor) o;

			if ( editor._isUpdatingTimeProperties )
			{
				return;
			}

			editor._isUpdatingTimeProperties = true;
			editor.Days = editor.TimeSpan.Days;
			editor.Hours = editor.TimeSpan.Hours;
			editor.Minutes = editor.TimeSpan.Minutes;
			editor._isUpdatingTimeProperties = false;
		}

		static void OnTimeSpanComponentChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			TimeSpanEditor editor = (TimeSpanEditor) o;

			if ( editor._isUpdatingTimeProperties )
			{
				return;
			}

			editor._isUpdatingTimeProperties = true;
			editor.TimeSpan = new TimeSpan( editor.Days, editor.Hours, editor.Minutes, 0 );
			editor._isUpdatingTimeProperties = false;
		}

		Boolean _isUpdatingTimeProperties;
	}
}
