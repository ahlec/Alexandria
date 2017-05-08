using System;
using Alexandria.Searching;

namespace HeadLibrarian.ViewModels
{
	public enum SearchCriteriaViewModelType
	{
		None,
		ExactMatch,
		LessThan,
		LessThanOrEqual,
		GreaterThan,
		GreaterThanOrEqual,
		Range
	}

	public static class SearchCriteriaViewModelTypeUtils
	{
		public static SearchCriteriaType ToAlexandriaType( SearchCriteriaViewModelType type )
		{
			switch ( type )
			{
				case SearchCriteriaViewModelType.None:
					throw new ArgumentException();
				case SearchCriteriaViewModelType.ExactMatch:
					return SearchCriteriaType.ExactMatch;
				case SearchCriteriaViewModelType.LessThan:
					return SearchCriteriaType.LessThan;
				case SearchCriteriaViewModelType.LessThanOrEqual:
					return SearchCriteriaType.LessThanOrEqual;
				case SearchCriteriaViewModelType.GreaterThan:
					return SearchCriteriaType.GreaterThan;
				case SearchCriteriaViewModelType.GreaterThanOrEqual:
					return SearchCriteriaType.GreaterThanOrEqual;
				case SearchCriteriaViewModelType.Range:
					return SearchCriteriaType.Range;
				default:
					throw new NotImplementedException();
			}
		}

		public static SearchCriteriaViewModelType FromAlexandriaType( SearchCriteriaType type )
		{
			switch ( type )
			{
				case SearchCriteriaType.ExactMatch:
					return SearchCriteriaViewModelType.ExactMatch;
				case SearchCriteriaType.LessThan:
					return SearchCriteriaViewModelType.LessThan;
				case SearchCriteriaType.LessThanOrEqual:
					return SearchCriteriaViewModelType.LessThanOrEqual;
				case SearchCriteriaType.GreaterThan:
					return SearchCriteriaViewModelType.GreaterThan;
				case SearchCriteriaType.GreaterThanOrEqual:
					return SearchCriteriaViewModelType.GreaterThanOrEqual;
				case SearchCriteriaType.Range:
					return SearchCriteriaViewModelType.Range;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
