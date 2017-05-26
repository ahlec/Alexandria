using System;

namespace Alexandria.Searching
{
	public sealed class DateSearchCriteria : IEquatable<DateSearchCriteria>
	{
		public DateSearchCriteria()
		{
			_internalId = _nextInternalId++;
		}

		public DateSearchCriteriaType Type { get; set; }

		public DateField DateUnit { get; set; }

		public Int32 Number1
		{
			get => _number1;
			set => _number1 = ValidateNumber( value );
		}

		public Int32 Number2
		{
			get
			{
				if ( Type != DateSearchCriteriaType.Between )
				{
					throw new InvalidOperationException( $"Only a type of {nameof( DateSearchCriteriaType.Between )} may have two numbers." );
				}
				return _number2;
			}
			set
			{
				if ( Type != DateSearchCriteriaType.Between )
				{
					throw new InvalidOperationException( $"Only a type of {nameof( DateSearchCriteriaType.Between )} may have two numbers." );
				}
				_number2 = ValidateNumber( value );
			}
		}

		public DateSearchCriteria Clone()
		{
			return new DateSearchCriteria
			{
				Type = Type,
				DateUnit = DateUnit,
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

		public static DateSearchCriteria Parse( String text )
		{
			if ( String.IsNullOrEmpty( text ) )
			{
				return null;
			}

			DateSearchCriteria parsed = new DateSearchCriteria();

			Int32 dateUnitStartIndex;
			if ( text.StartsWith( "<", StringComparison.InvariantCultureIgnoreCase ) )
			{
				parsed.Type = DateSearchCriteriaType.Before;
				const Int32 PrefixLength = 2; // Length of string "< "
				dateUnitStartIndex = text.IndexOf( ' ', PrefixLength );
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( text.Substring( PrefixLength, dateUnitStartIndex - PrefixLength ) ) );
			}
			else if ( text.StartsWith( ">", StringComparison.InvariantCultureIgnoreCase ) )
			{
				parsed.Type = DateSearchCriteriaType.After;
				const Int32 PrefixLength = 2; // Length of string "> "
				dateUnitStartIndex = text.IndexOf( ' ', PrefixLength );
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( text.Substring( PrefixLength, dateUnitStartIndex - PrefixLength ) ) );
			}
			else if ( text.Contains( "-" ) )
			{
				parsed.Type = DateSearchCriteriaType.Between;
				Int32 hyphenIndex = text.IndexOf( '-' );
				dateUnitStartIndex = text.IndexOf( ' ', hyphenIndex + 2 ); // Move past "- "
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( text.Substring( 0, hyphenIndex ) ) );
				parsed.Number2 = parsed.ValidateNumber( Int32.Parse( text.Substring( hyphenIndex + 1, dateUnitStartIndex - hyphenIndex - 1 ) ) );
			}
			else
			{
				parsed.Type = DateSearchCriteriaType.Exactly;
				dateUnitStartIndex = text.IndexOf( ' ' );
				parsed.Number1 = parsed.ValidateNumber( Int32.Parse( text.Substring( 0, dateUnitStartIndex ) ) );
			}

			const Int32 SuffixLength = 5; // Length of string "s ago"
			parsed.DateUnit = (DateField) Enum.Parse( typeof( DateField ), text.Substring( dateUnitStartIndex, text.Length - dateUnitStartIndex - SuffixLength ) );
			return parsed;
		}

		public Boolean Equals( DateSearchCriteria other )
		{
			if ( Type != other?.Type || DateUnit != other.DateUnit || Number1 != other.Number1 )
			{
				return false;
			}

			if ( Type == DateSearchCriteriaType.Between && Number2 != other.Number2 )
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
				case DateSearchCriteriaType.Exactly:
					return String.Concat( Number1, " ", DateUnit, "s ago" );
				case DateSearchCriteriaType.Before:
					return String.Concat( "< ", Number1, " ", DateUnit, "s ago" );
				case DateSearchCriteriaType.After:
					return String.Concat( "> ", Number1, " ", DateUnit, "s ago" );
				case DateSearchCriteriaType.Between:
					return String.Concat( Number1, " - ", Number2, " ", DateUnit, "s ago" );
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

			DateSearchCriteria other = obj as DateSearchCriteria;
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
