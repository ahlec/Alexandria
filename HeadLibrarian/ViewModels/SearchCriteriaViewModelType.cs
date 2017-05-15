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
		public static NumberSearchCriteriaType ToAlexandriaNumberType( SearchCriteriaViewModelType type )
		{
			switch ( type )
			{
				case SearchCriteriaViewModelType.None:
					throw new ArgumentException();
				case SearchCriteriaViewModelType.ExactMatch:
					return NumberSearchCriteriaType.ExactMatch;
				case SearchCriteriaViewModelType.LessThan:
					return NumberSearchCriteriaType.LessThan;
				case SearchCriteriaViewModelType.LessThanOrEqual:
					return NumberSearchCriteriaType.LessThanOrEqual;
				case SearchCriteriaViewModelType.GreaterThan:
					return NumberSearchCriteriaType.GreaterThan;
				case SearchCriteriaViewModelType.GreaterThanOrEqual:
					return NumberSearchCriteriaType.GreaterThanOrEqual;
				case SearchCriteriaViewModelType.Range:
					return NumberSearchCriteriaType.Range;
				default:
					throw new NotImplementedException();
			}
		}

		public static DateSearchCriteriaType ToAlexandriaDateType( SearchCriteriaViewModelType type )
		{
			switch ( type )
			{
				case SearchCriteriaViewModelType.None:
					throw new ArgumentException();
				case SearchCriteriaViewModelType.ExactMatch:
					return DateSearchCriteriaType.Exactly;
				case SearchCriteriaViewModelType.LessThan:
					return DateSearchCriteriaType.Before;
				case SearchCriteriaViewModelType.LessThanOrEqual:
					throw new ArgumentException();
				case SearchCriteriaViewModelType.GreaterThan:
					return DateSearchCriteriaType.After;
				case SearchCriteriaViewModelType.GreaterThanOrEqual:
					throw new ArgumentException();
				case SearchCriteriaViewModelType.Range:
					return DateSearchCriteriaType.Between;
				default:
					throw new NotImplementedException();
			}
		}

		public static SearchCriteriaViewModelType FromAlexandriaNumberType( NumberSearchCriteriaType type )
		{
			switch ( type )
			{
				case NumberSearchCriteriaType.ExactMatch:
					return SearchCriteriaViewModelType.ExactMatch;
				case NumberSearchCriteriaType.LessThan:
					return SearchCriteriaViewModelType.LessThan;
				case NumberSearchCriteriaType.LessThanOrEqual:
					return SearchCriteriaViewModelType.LessThanOrEqual;
				case NumberSearchCriteriaType.GreaterThan:
					return SearchCriteriaViewModelType.GreaterThan;
				case NumberSearchCriteriaType.GreaterThanOrEqual:
					return SearchCriteriaViewModelType.GreaterThanOrEqual;
				case NumberSearchCriteriaType.Range:
					return SearchCriteriaViewModelType.Range;
				default:
					throw new NotImplementedException();
			}
		}

		public static SearchCriteriaViewModelType FromAlexandriaDateType( DateSearchCriteriaType type )
		{
			switch ( type )
			{
				case DateSearchCriteriaType.Exactly:
					return SearchCriteriaViewModelType.ExactMatch;
				case DateSearchCriteriaType.Before:
					return SearchCriteriaViewModelType.LessThanOrEqual;
				case DateSearchCriteriaType.After:
					return SearchCriteriaViewModelType.GreaterThan;
				case DateSearchCriteriaType.Between:
					return SearchCriteriaViewModelType.Range;
				default:
					throw new NotImplementedException();
			}
		}

		public static readonly SearchCriteriaViewModelType[] AllValidNumberTypes =
		{
			SearchCriteriaViewModelType.None,
			SearchCriteriaViewModelType.ExactMatch,
			SearchCriteriaViewModelType.LessThan,
			SearchCriteriaViewModelType.LessThanOrEqual,
			SearchCriteriaViewModelType.GreaterThan,
			SearchCriteriaViewModelType.GreaterThanOrEqual,
			SearchCriteriaViewModelType.Range
		};
		public static readonly SearchCriteriaViewModelType[] AllValidDateTypes =
		{
			SearchCriteriaViewModelType.None,
			SearchCriteriaViewModelType.ExactMatch,
			SearchCriteriaViewModelType.LessThan,
			SearchCriteriaViewModelType.GreaterThan,
			SearchCriteriaViewModelType.Range
		};
	}
}
