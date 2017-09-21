// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

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

        public int Number1
        {
            get => _number1;
            set => _number1 = ValidateNumber( value );
        }

        public int Number2
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

        public static DateSearchCriteria Parse( string text )
        {
            if ( string.IsNullOrEmpty( text ) )
            {
                return null;
            }

            DateSearchCriteria parsed = new DateSearchCriteria();

            int dateUnitStartIndex;
            if ( text.StartsWith( "<", StringComparison.InvariantCultureIgnoreCase ) )
            {
                parsed.Type = DateSearchCriteriaType.Before;
                const int PrefixLength = 2; // Length of string "< "
                dateUnitStartIndex = text.IndexOf( ' ', PrefixLength );
                parsed.Number1 = parsed.ValidateNumber( int.Parse( text.Substring( PrefixLength, dateUnitStartIndex - PrefixLength ) ) );
            }
            else if ( text.StartsWith( ">", StringComparison.InvariantCultureIgnoreCase ) )
            {
                parsed.Type = DateSearchCriteriaType.After;
                const int PrefixLength = 2; // Length of string "> "
                dateUnitStartIndex = text.IndexOf( ' ', PrefixLength );
                parsed.Number1 = parsed.ValidateNumber( int.Parse( text.Substring( PrefixLength, dateUnitStartIndex - PrefixLength ) ) );
            }
            else if ( text.Contains( "-" ) )
            {
                parsed.Type = DateSearchCriteriaType.Between;
                int hyphenIndex = text.IndexOf( '-' );
                dateUnitStartIndex = text.IndexOf( ' ', hyphenIndex + 2 ); // Move past "- "
                parsed.Number1 = parsed.ValidateNumber( int.Parse( text.Substring( 0, hyphenIndex ) ) );
                parsed.Number2 = parsed.ValidateNumber( int.Parse( text.Substring( hyphenIndex + 1, dateUnitStartIndex - hyphenIndex - 1 ) ) );
            }
            else
            {
                parsed.Type = DateSearchCriteriaType.Exactly;
                dateUnitStartIndex = text.IndexOf( ' ' );
                parsed.Number1 = parsed.ValidateNumber( int.Parse( text.Substring( 0, dateUnitStartIndex ) ) );
            }

            const int SuffixLength = 5; // Length of string "s ago"
            parsed.DateUnit = (DateField) Enum.Parse( typeof( DateField ), text.Substring( dateUnitStartIndex, text.Length - dateUnitStartIndex - SuffixLength ) );
            return parsed;
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

        public bool Equals( DateSearchCriteria other )
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
        public override string ToString()
        {
            switch ( Type )
            {
                case DateSearchCriteriaType.Exactly:
                    return string.Concat( Number1, " ", DateUnit, "s ago" );
                case DateSearchCriteriaType.Before:
                    return string.Concat( "< ", Number1, " ", DateUnit, "s ago" );
                case DateSearchCriteriaType.After:
                    return string.Concat( "> ", Number1, " ", DateUnit, "s ago" );
                case DateSearchCriteriaType.Between:
                    return string.Concat( Number1, " - ", Number2, " ", DateUnit, "s ago" );
                default:
                    throw new NotImplementedException();
            }
        }

        public override bool Equals( object obj )
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
        public override int GetHashCode()
        {
            return _internalId.GetHashCode();
        }

        int ValidateNumber( int number )
        {
            if ( number < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( number ), "The number must be zero or positive." );
            }

            return number;
        }

        static uint _nextInternalId = 1;
        readonly uint _internalId;
        int _number1;
        int _number2;
    }
}
