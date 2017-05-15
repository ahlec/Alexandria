using System;
using System.Collections.Generic;
using Alexandria.Searching;

namespace HeadLibrarian.Controls
{
	public partial class DateSearchCriteriaEditor
	{
		static DateSearchCriteriaEditor()
		{
			List<DateField> allDateFields = new List<DateField>();
			foreach ( DateField field in Enum.GetValues( typeof( DateField ) ) )
			{
				allDateFields.Add( field );
			}
			AllDateFields = allDateFields;
		}

		public DateSearchCriteriaEditor()
		{
			InitializeComponent();
		}

		public static IEnumerable<DateField> AllDateFields { get; }
	}
}
