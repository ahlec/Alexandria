using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HeadLibrarian.ViewModels
{
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		protected abstract class BaseUndoAction : IUndoRedoAction
		{
			public abstract void Undo();

			public abstract void Redo();

			protected void AssertModelSetFunction( Boolean value )
			{
				if ( !value )
				{
					throw new ApplicationException( "A model .SetXXX function did not return true, which it must inside of UndoActions!" );
				}
			}
		}

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

		protected virtual void OnPropertyChanged( String propertyName )
		{
			PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
