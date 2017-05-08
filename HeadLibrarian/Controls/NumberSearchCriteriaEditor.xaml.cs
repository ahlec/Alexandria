using System;
using System.Collections.Generic;
using System.Windows.Input;
using Alexandria.Searching;
using HeadLibrarian.ViewModels;

namespace HeadLibrarian.Controls
{
	public partial class NumberSearchCriteriaEditor
	{
		public NumberSearchCriteriaEditor()
		{
			List<SearchCriteriaViewModelType> allTypes = new List<SearchCriteriaViewModelType>();
			foreach ( SearchCriteriaViewModelType type in Enum.GetValues( typeof( SearchCriteriaViewModelType ) ) )
			{
				allTypes.Add( type );
			}
			AllTypes = allTypes;

			InitializeComponent();
		}

		public IEnumerable<SearchCriteriaViewModelType> AllTypes { get; }
	}
}
