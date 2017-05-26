using System;

namespace Alexandria.Searching
{
	public sealed class NumberSearchCriteria : IEquatable<NumberSearchCriteria>
	{
		public const Int32 MaximumNumberValue = 9999999;

		public NumberSearchCriteria()
		{
			_internalId = _nextInternalId++;
		}

		public NumberSearchCriteriaType Type { get; set; }

		public Int32 Number1
		{
			get => _number1;
			set => _number1 = ValidateNumber( value );
		}

		public Int32 Number2
		{
			get
			{
				if ( Type != NumberSearchCriteriaType.Range )
				{
					throw new InvalidOperationException( $"Only a type of {nameof( NumberSearchCriteriaType.Range )} may have two numbers." );
				}
				return _number2;
			}
			set
			{
				if ( Type != NumberSearchCriteriaType.Range )
				{
					throw new InvalidOperationException( $"Only a type of {nameof( NumberSearchCriteriaType.Range )} may have two numbers." );
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

			if ( number > MaximumNumberValue )
			{
				throw new ArgumentOutOfRangeException( nameof( number ), $"The number must be less than or equal to {MaximumNumberValue}." );
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

			if ( text.StartsWith( "<", StringComparison.InvariantCultureIgnoreCase ) )
			{
				parsed.Type = NumberSearchCriteriaType.LessThan;
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( text.Substring( 1 ) ) );
			}
			else if ( text.StartsWith( ">", StringComparison.InvariantCultureIgnoreCase ) )
			{
				parsed.Type = NumberSearchCriteriaType.GreaterThan;
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( text.Substring( 1 ) ) );
			}
			else if ( text.Contains( "-" ) )
			{
				parsed.Type = NumberSearchCriteriaType.Range;
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
				parsed.Type = NumberSearchCriteriaType.ExactMatch;
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( text ) );
			}

			return parsed;
		}

		public Boolean Equals( NumberSearchCriteria other )
		{
			if ( Type != other?.Type || Number1 != other.Number1 )
			{
				return false;
			}

			if ( Type == NumberSearchCriteriaType.Range && Number2 != other.Number2 )
			{
				return false;
			}

			return true;
		}

		/// <inheritdoc />
		public override String ToString()
		{
			switch ( Type )
			{
				case NumberSearchCriteriaType.ExactMatch:
					return Number1.ToString();
				case NumberSearchCriteriaType.LessThan:
					return String.Concat( "<", Number1 );
				case NumberSearchCriteriaType.GreaterThan:
					return String.Concat( ">", Number1 );
				case NumberSearchCriteriaType.Range:
					return String.Concat( Number1, "-", Number2 );
				default:
					throw new NotImplementedException();
			}
		}

		public override Boolean Equals( Object obj )
		{
			if ( ReferenceEquals( obj, null ) )
			{
				return false;
			}

			if ( ReferenceEquals( obj, this ) )
			{
				return true;
			}

			NumberSearchCriteria other = obj as NumberSearchCriteria;
			return Equals( other );
		}

		/// <inheritdoc />
		public override Int32 GetHashCode()
		{
			return _internalId.GetHashCode();
		}

		static UInt32 _nextInternalId = 1;
		readonly UInt32 _internalId;
		Int32 _number1;
		Int32 _number2;
	}
}
