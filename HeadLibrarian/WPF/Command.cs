using System;
using System.Windows.Input;

namespace HeadLibrarian.WPF
{
	public class Command : ICommand
	{
		public Command( Func<Object, Boolean> canExecute, Action<Object> action )
		{
			_canExecute = canExecute;
			_action = action;
		}

		public Boolean CanExecute( Object parameter )
		{
			return ( _canExecute?.Invoke( parameter ) != false );
		}

		public void Execute( Object parameter )
		{
			_action?.Invoke( parameter );
		}

		public event EventHandler CanExecuteChanged;

		readonly Func<Object, Boolean> _canExecute;
		readonly Action<Object> _action;
	}
}
