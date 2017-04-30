using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HeadLibrarian.ViewModels
{
	internal abstract class BaseViewModel : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged

		protected Boolean SetProperty<T>( ref T field, T value, [CallerMemberName] String propertyName = null )
		{
			if ( EqualityComparer<T>.Default.Equals( field, value ) )
			{
				return false;
			}

			field = value;
			OnPropertyChanged( propertyName );
			return true;
		}

		protected void OnPropertyChanged( String propertyName )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
