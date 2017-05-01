using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace HeadLibrarian.TriggerActions
{
	public sealed class SetSelectedItemToEventArgsAction : TriggerAction<ListBox>
	{
		/// <inheritdoc />
		protected override void Invoke( Object parameter )
		{
			throw new NotImplementedException();
		}

		public ListBox ListBox { get; set; }
	}
}
