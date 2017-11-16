// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Searching
{
    public sealed class NumberSearchCriteria : IEquatable<NumberSearchCriteria>
    {
        public const int MaximumNumberValue = 9999999;

        static uint _nextInternalId = 1;
        readonly uint _internalId;
        int _number1;
        int _number2;

        public NumberSearchCriteria()
        {
            _internalId = _nextInternalId++;
        }

        public NumberSearchCriteriaType Type { get; set; }

        public int Number1
        {
            get => _number1;
            set => _number1 = ValidateNumber( value );
        }

        public int Number2
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

        public static NumberSearchCriteria Parse( string text )
        {
            if ( string.IsNullOrEmpty( text ) )
            {
                return null;
            }

            NumberSearchCriteria parsed = new NumberSearchCriteria();

            if ( text.StartsWith( "<", StringComparison.InvariantCultureIgnoreCase ) )
            {
                parsed.Type = NumberSearchCriteriaType.LessThan;
                parsed.Number1 = parsed.ValidateNumber( int.Parse( text.Substring( 1 ) ) );
            }
            else if ( text.StartsWith( ">", StringComparison.InvariantCultureIgnoreCase ) )
            {
                parsed.Type = NumberSearchCriteriaType.GreaterThan;
                parsed.Number1 = parsed.ValidateNumber( int.Parse( text.Substring( 1 ) ) );
            }
            else if ( text.Contains( "-" ) )
            {
                parsed.Type = NumberSearchCriteriaType.Range;
                string[] pieces = text.Split( '-' );
                if ( pieces.Length != 2 )
                {
                    throw new ArgumentException( "There must be exactly two numbers divided by a hyphen for a range of numbers.", nameof( text ) );
                }

                parsed.Number1 = parsed.ValidateNumber( int.Parse( pieces[0] ) );
                parsed.Number2 = parsed.ValidateNumber( int.Parse( pieces[1] ) );
            }
            else
            {
                parsed.Type = NumberSearchCriteriaType.ExactMatch;
                parsed.Number1 = parsed.ValidateNumber( int.Parse( text ) );
            }

            return parsed;
        }

        public bool Equals( NumberSearchCriteria other )
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
        public override string ToString()
        {
            switch ( Type )
            {
                case NumberSearchCriteriaType.ExactMatch:
                    return Number1.ToString();
                case NumberSearchCriteriaType.LessThan:
                    return string.Concat( "<", Number1 );
                case NumberSearchCriteriaType.GreaterThan:
                    return string.Concat( ">", Number1 );
                case NumberSearchCriteriaType.Range:
                    return string.Concat( Number1, "-", Number2 );
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

            NumberSearchCriteria other = obj as NumberSearchCriteria;
            return Equals( other );
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _internalId.GetHashCode();
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

        int ValidateNumber( int number )
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
    }
}
