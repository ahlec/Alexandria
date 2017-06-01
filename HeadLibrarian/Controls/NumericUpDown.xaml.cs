using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HeadLibrarian.WPF;

namespace HeadLibrarian.Controls
{
	public partial class NumericUpDown : INotifyPropertyChanged
	{
		public NumericUpDown()
		{
			InitializeComponent();
		}

		public Int32 Value
		{
			get => (Int32) GetValue( ValueProperty );
			set => SetValue( ValueProperty, value );
		}

		public Int32 MinValue
		{
			get => (Int32) GetValue( MinValueProperty );
			set => SetValue( MinValueProperty, value );
		}

		public Int32 MaxValue
		{
			get => (Int32) GetValue( MaxValueProperty );
			set => SetValue( MaxValueProperty, value );
		}

		public Int32 Step
		{
			get => (Int32) GetValue( StepProperty );
			set => SetValue( StepProperty, value );
		}

		public Boolean CanDecrease
		{
			get => _canDecrease;
			private set => SetProperty( ref _canDecrease, value );
		}

		public Boolean CanIncrease
		{
			get => _canIncrease;
			private set => SetProperty( ref _canIncrease, value );
		}

		public ICommand DecreaseCommand => ( _decreaseCommand ?? ( _decreaseCommand = new Command( null, CommandDecrease ) ) );
		public ICommand IncreaseCommand => ( _increaseCommand ?? ( _increaseCommand = new Command( null, CommandIncrease ) ) );

		void CommandDecrease( Object o )
		{
			Value -= Step;
		}

		void CommandIncrease( Object o )
		{
			Value += Step;
		}

		void RefreshDecreaseIncrease()
		{
			CanDecrease = ( Value > MinValue );
			CanIncrease = ( Value < MaxValue );
		}

		static Boolean IsTextAllowed( String text )
		{
			if ( String.IsNullOrEmpty( text ) )
			{
				return true;
			}

			return text.All( Char.IsDigit );
		}

		void PreviewValueTextInput( Object sender, TextCompositionEventArgs e )
		{
			e.Handled = !IsTextAllowed( e.Text );
		}

		void ValueTextBoxPasting( Object sender, DataObjectPastingEventArgs e )
		{
			if ( e.DataObject.GetDataPresent( typeof( Int32 ) ) )
			{
				return;
			}

			if ( e.DataObject.GetDataPresent( typeof( String ) ) )
			{
				String text = (String) e.DataObject.GetData( typeof( String ) );
				if ( !IsTextAllowed( text ) )
				{
					e.CancelCommand();
				}
				return;
			}

			e.CancelCommand();
		}

		void OnPreviewTextBoxKeyDown( Object sender, KeyEventArgs e )
		{
			if ( e.Key == Key.Up )
			{
				if ( CanIncrease )
				{
					CommandIncrease( null );
				}
			}
			else if ( e.Key == Key.Down )
			{
				if ( CanDecrease )
				{
					CommandDecrease( null );
				}
			}
		}

		void OnPreviewMouseWheel( Object sender, MouseWheelEventArgs e )
		{
			TextBox textBox = (TextBox) sender;
			if ( !textBox.IsFocused )
			{
				return;
			}

			if ( e.Delta > 0 )
			{
				if ( CanIncrease )
				{
					CommandIncrease( null );
					e.Handled = true;
				}
			}
			else if ( e.Delta < 0 )
			{
				if ( CanDecrease )
				{
					CommandDecrease( null );
					e.Handled = true;
				}
			}
		}

		void SetProperty<T>( ref T field, T value, [CallerMemberName] String propertyName = null )
		{
			if ( EqualityComparer<T>.Default.Equals( field, value ) )
			{
				return;
			}

			field = value;
			OnPropertyChanged( propertyName );
		}

		void OnPropertyChanged( String propertyName )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
		}

		public event PropertyChangedEventHandler PropertyChanged;

		static Object CoerceValue( DependencyObject o, Object input )
		{
			NumericUpDown numericUpDown = (NumericUpDown) o;

			Int32 value;
			if ( input is Int32 )
			{
				value = (Int32) input;
			}
			else if ( !Int32.TryParse( input?.ToString(), out value ) )
			{
				value = 0;
			}

			if ( value < numericUpDown.MinValue )
			{
				value = numericUpDown.MinValue;
			}

			if ( value > numericUpDown.MaxValue )
			{
				value = numericUpDown.MaxValue;
			}

			return value;
		}

		static void OnValueChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			NumericUpDown numericUpDown = (NumericUpDown) o;
			numericUpDown.RefreshDecreaseIncrease();
		}

		static void OnMinValueChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			NumericUpDown numericUpDown = (NumericUpDown) o;
			if ( numericUpDown.MinValue > numericUpDown.MaxValue )
			{
				numericUpDown.MaxValue = numericUpDown.MinValue;
			}
			if ( numericUpDown.Value < numericUpDown.MinValue )
			{
				numericUpDown.Value = numericUpDown.MinValue;
			}

			numericUpDown.RefreshDecreaseIncrease();
		}

		static void OnMaxValueChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			NumericUpDown numericUpDown = (NumericUpDown) o;
			if ( numericUpDown.MaxValue < numericUpDown.MinValue )
			{
				numericUpDown.MinValue = numericUpDown.MaxValue;
			}
			if ( numericUpDown.Value > numericUpDown.MaxValue )
			{
				numericUpDown.Value = numericUpDown.MaxValue;
			}

			numericUpDown.RefreshDecreaseIncrease();
		}

		static Object CoerceStepValue( DependencyObject o, Object value )
		{
			if ( !( value is Int32 ) )
			{
				throw new ArgumentException();
			}

			Int32 step = (Int32) value;
			if ( step <= 0 )
			{
				throw new ArgumentOutOfRangeException( nameof( value ) );
			}

			return step;
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register( "Value", typeof( Int32 ), typeof( NumericUpDown ),
			new PropertyMetadata( 0, OnValueChanged, CoerceValue ) );
		public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register( "MinValue", typeof( Int32 ), typeof( NumericUpDown ),
			new PropertyMetadata( Int32.MinValue, OnMinValueChanged ) );
		public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register( "MaxValue", typeof( Int32 ), typeof( NumericUpDown ),
			new PropertyMetadata( Int32.MaxValue, OnMaxValueChanged ) );
		public static readonly DependencyProperty StepProperty = DependencyProperty.Register( "Step", typeof( Int32 ), typeof( NumericUpDown ),
			new PropertyMetadata( 1, null, CoerceStepValue ) );

		ICommand _decreaseCommand;
		ICommand _increaseCommand;
		Boolean _canDecrease;
		Boolean _canIncrease;
	}
}
