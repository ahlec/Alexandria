using System;

namespace Alexandria.Searching
{
	public sealed class NumberSearchCriteria
	{
		public const Int32 MaximumNumberValue = 9999999;

		public SearchCriteriaType Type { get; set; }

		public Int32 Number1
		{
			get => _number1;
			set => _number1 = ValidateNumber( value );
		}

		public Int32 Number2
		{
			get
			{
				if ( Type != SearchCriteriaType.Range )
				{
					throw new InvalidOperationException( $"Only a type of {nameof( SearchCriteriaType.Range )} may have two numbers." );
				}
				return _number2;
			}
			set
			{
				if ( Type != SearchCriteriaType.Range )
				{
					throw new InvalidOperationException( $"Only a type of {nameof( SearchCriteriaType.Range )} may have two numbers." );
				}
				_number2 = ValidateNumber( value );
			}
		}

		public NumberSearchCriteria Clone()
		{
			return new NumberSearchCriteria
			{
				Type = Type,
				_number1 = _number1,
				_number2 = _number2
			};
		}

		Int32 ValidateNumber( Int32 number )
		{
			if ( number < 0 )
			{
				throw new ArgumentOutOfRangeException( nameof( number ), "The number must be zero or positive." );
			}

			return number;
		}

		public static NumberSearchCriteria Parse( String text )
		{
			if ( String.IsNullOrEmpty( text ) )
			{
				return null;
			}

			NumberSearchCriteria parsed = new NumberSearchCriteria();

			if ( text.StartsWith( "<=", StringComparison.InvariantCultureIgnoreCase ) )
			{
				parsed.Type = SearchCriteriaType.LessThanOrEqual;
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( text.Substring( 2 ) ) );
			}
			else if ( text.StartsWith( "<", StringComparison.InvariantCultureIgnoreCase ) )
			{
				parsed.Type = SearchCriteriaType.LessThan;
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( text.Substring( 1 ) ) );
			}
			else if ( text.StartsWith( ">=", StringComparison.InvariantCultureIgnoreCase ) )
			{
				parsed.Type = SearchCriteriaType.GreaterThanOrEqual;
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( text.Substring( 2 ) ) );
			}
			else if ( text.StartsWith( ">", StringComparison.InvariantCultureIgnoreCase ) )
			{
				parsed.Type = SearchCriteriaType.GreaterThan;
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( text.Substring( 1 ) ) );
			}
			else if ( text.Contains( "-" ) )
			{
				parsed.Type = SearchCriteriaType.Range;
				String[] pieces = text.Split( '-' );
				if ( pieces.Length != 2 )
				{
					throw new ArgumentException( "There must be exactly two numbers divided by a hyphen for a range of numbers.", nameof( text ) );
				}
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( pieces[0] ) );
				parsed.Number2 = parsed.ValidateNumber( Int32.Parse( pieces[1] ) );
			}
			else
			{
				parsed.Type = SearchCriteriaType.ExactMatch;
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( text ) );
			}

			return parsed;
		}

		/// <inheritdoc />
		public override String ToString()
		{
			switch ( Type )
			{
				case SearchCriteriaType.ExactMatch:
					return Number1.ToString();
				case SearchCriteriaType.LessThan:
					return String.Concat( "<", Number1 );
				case SearchCriteriaType.LessThanOrEqual:
					return String.Concat( "<=", Number1 );
				case SearchCriteriaType.GreaterThan:
					return String.Concat( ">", Number1 );
				case SearchCriteriaType.GreaterThanOrEqual:
					return String.Concat( ">=", Number1 );
				case SearchCriteriaType.Range:
					return String.Concat( Number1, "-", Number2 );
				default:
					throw new NotImplementedException();
			}
		}

		Int32 _number1;
		Int32 _number2;
	}
}
