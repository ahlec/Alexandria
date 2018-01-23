// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Searching
{
    /// <summary>
    /// A class that can be used to represent a numerical search field in a
    /// <seealso cref="LibrarySearch" />. This class can be configured to a number
    /// of variations of filtering with numbers, such as "&lt; 15" or "between 1-10".
    /// </summary>
    public sealed class NumberSearchCriteria : SearchCriteriaBase<NumberSearchCriteria, NumberSearchCriteriaType>
    {
        enum ParseResult
        {
            InputCannotBeNull,
            UnrecognizedCriteriaType,
            InvalidInputFormat,
            InvalidNumber1,
            InvalidNumber2,
            Success
        }

        /// <summary>
        /// Parses a string representation of a numeric filter into an instance of
        /// <see cref="NumberSearchCriteria"/>.
        /// </summary>
        /// <param name="text">The valid string representation of a numeric filter
        /// that should be parsed.</param>
        /// <returns>A valid instance of <see cref="NumberSearchCriteria"/> configured
        /// based on the contents of the provided string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/>
        /// is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided string does not
        /// indicate any valid <seealso cref="NumberSearchCriteriaType"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided string is in an
        /// invalid format for the <seealso cref="NumberSearchCriteriaType"/> it identifies
        /// itself as.</exception>
        /// <exception cref="ArgumentException">Thrown when the first number in the provided
        /// string could not be parsed correctly, either because it wasn't an integer or because
        /// it was outside the allowed bounds of numbers for <see cref="NumberSearchCriteria"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when the second number in the provided
        /// string could not be parsed correctly, either because it wasn't an integer or because
        /// it was outside the allowed bounds of numbers for <see cref="NumberSearchCriteria"/>.</exception>
        public static NumberSearchCriteria Parse( string text )
        {
            ParseResult result = TryParseInternal( text, out NumberSearchCriteria criteria );
            switch ( result )
            {
                case ParseResult.InputCannotBeNull:
                    throw new ArgumentNullException( nameof( text ) );
                case ParseResult.UnrecognizedCriteriaType:
                    throw new ArgumentException( $"The {nameof( NumberSearchCriteriaType )} could not be determined from the input string.", nameof( text ) );
                case ParseResult.InvalidInputFormat:
                    throw new ArgumentException( "The provided string was invalid for the criteria type it was detected as.", nameof( text ) );
                case ParseResult.InvalidNumber1:
                    throw new ArgumentException( "The first number could not be parsed from the provided string.", nameof( text ) );
                case ParseResult.InvalidNumber2:
                    throw new ArgumentException( "The second number could not be parsed from the provided string.", nameof( text ) );
                case ParseResult.Success:
                    return criteria;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Attempts to safely parse the provided string as a valid instance of
        /// <see cref="NumberSearchCriteria"/>.
        /// </summary>
        /// <param name="text">The valid string representation of a numeric filter
        /// that should be parsed.</param>
        /// <param name="criteria">An out parameter for the <see cref="NumberSearchCriteria"/>
        /// that was successfully parsed. This will be a valid instance if the return value
        /// is true, or null if the return value is false.</param>
        /// <returns>Returns true if <paramref name="text"/> could be parsed successfully
        /// as a valid instance, or false otherwise. This also indicates what the value of
        /// <paramref name="criteria"/> can be expected to be: a valid instance (true) or null
        /// (false).</returns>
        public static bool TryParse( string text, out NumberSearchCriteria criteria )
        {
            if ( TryParseInternal( text, out criteria ) == ParseResult.Success )
            {
                return true;
            }

            criteria = null;
            return false;
        }

        /// <inheritdoc />
        public override NumberSearchCriteria Clone()
        {
            return new NumberSearchCriteria
            {
                Type = Type,
                Number1 = Number1,
                Number2 = Number2
            };
        }

        /// <inheritdoc />
        public override bool Equals( NumberSearchCriteria other )
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

        static ParseResult TryParseInternal( string str, out NumberSearchCriteria criteria )
        {
            if ( str == null )
            {
                criteria = null;
                return ParseResult.InputCannotBeNull;
            }

            str = str.Trim();

            if ( str.StartsWith( "<", StringComparison.InvariantCultureIgnoreCase ) )
            {
                return TryParseLessThan( str, out criteria );
            }

            if ( str.StartsWith( ">", StringComparison.InvariantCultureIgnoreCase ) )
            {
                return TryParseGreaterThan( str, out criteria );
            }

            if ( str.Contains( "-" ) )
            {
                return TryParseRange( str, out criteria );
            }

            return TryParseExactMatch( str, out criteria );
        }

        static ParseResult TryParseLessThan( string str, out NumberSearchCriteria criteria )
        {
            criteria = new NumberSearchCriteria
            {
                Type = NumberSearchCriteriaType.LessThan
            };

            if ( TryParseValidNumber( str.Substring( 1 ), out int number1 ) != TryParseNumberResult.Success )
            {
                return ParseResult.InvalidNumber1;
            }

            criteria.Number1 = number1;
            return ParseResult.Success;
        }

        static ParseResult TryParseGreaterThan( string str, out NumberSearchCriteria criteria )
        {
            criteria = new NumberSearchCriteria
            {
                Type = NumberSearchCriteriaType.GreaterThan
            };

            if ( TryParseValidNumber( str.Substring( 1 ), out int number1 ) != TryParseNumberResult.Success )
            {
                return ParseResult.InvalidNumber1;
            }

            criteria.Number1 = number1;
            return ParseResult.Success;
        }

        static ParseResult TryParseRange( string str, out NumberSearchCriteria criteria )
        {
            criteria = new NumberSearchCriteria
            {
                Type = NumberSearchCriteriaType.Range
            };

            string[] pieces = str.Split( '-' );
            if ( pieces.Length != 2 )
            {
                return ParseResult.InvalidInputFormat;
            }

            if ( TryParseValidNumber( pieces[0], out int number1 ) != TryParseNumberResult.Success )
            {
                return ParseResult.InvalidNumber1;
            }

            if ( TryParseValidNumber( pieces[1], out int number2 ) != TryParseNumberResult.Success )
            {
                return ParseResult.InvalidNumber2;
            }

            criteria.Number1 = number1;
            criteria.Number2 = number2;
            return ParseResult.Success;
        }

        static ParseResult TryParseExactMatch( string str, out NumberSearchCriteria criteria )
        {
            criteria = new NumberSearchCriteria
            {
                Type = NumberSearchCriteriaType.ExactMatch
            };

            TryParseNumberResult result = TryParseValidNumber( str, out int number1 );
            switch ( result )
            {
                case TryParseNumberResult.NotAnInteger:
                    return ParseResult.UnrecognizedCriteriaType;

                case TryParseNumberResult.NumberNotValid:
                    return ParseResult.InvalidNumber1;

                case TryParseNumberResult.Success:
                    {
                        criteria.Number1 = number1;
                        return ParseResult.Success;
                    }

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
