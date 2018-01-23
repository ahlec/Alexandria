// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Searching
{
    /// <summary>
    /// A class that can be used to represent a chronological/date search field in a
    /// <seealso cref="LibrarySearch"/>. This class can be configured to a number of
    /// variations for filtering with dates, such as "less than 10 days ago" or "before
    /// 3 months ago".
    /// </summary>
    public sealed class DateSearchCriteria : SearchCriteriaBase<DateSearchCriteria, DateSearchCriteriaType>
    {
        enum ParseResult
        {
            InputCannotBeNull,
            UnrecognizedCriteriaType,
            InvalidInputFormat,
            InvalidNumber1,
            InvalidNumber2,
            InvalidDateUnit,
            Success
        }

        /// <summary>
        /// Gets or sets the unit of time measurement that the numbers in this criteria
        /// are measured in.
        /// </summary>
        public DateField DateUnit { get; set; }

        /// <summary>
        /// Parses a string representation of a date filter into an instance of
        /// <see cref="DateSearchCriteria"/>.
        /// </summary>
        /// <param name="text">The valid string representation of a date filter
        /// that should be parsed.</param>
        /// <returns>A valid instance of <see cref="DateSearchCriteria"/> configured
        /// based on the contents of the provided string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/>
        /// is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided string does not
        /// indicate any valid <seealso cref="DateSearchCriteriaType"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided string is in an
        /// invalid format for the <seealso cref="DateSearchCriteriaType"/> it identifies
        /// itself as.</exception>
        /// <exception cref="ArgumentException">Thrown when the first number in the provided
        /// string could not be parsed correctly, either because it wasn't an integer or because
        /// it was outside the allowed bounds of numbers for <see cref="DateSearchCriteria"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when the second number in the provided
        /// string could not be parsed correctly, either because it wasn't an integer or because
        /// it was outside the allowed bounds of numbers for <see cref="DateSearchCriteria"/>.</exception>
        /// <exception cref="ArgumentException">Thrown when the date unit could not be parsed correctly,
        /// either because it wasn't present in the string or because it was misspelled or with improper
        /// formatting.</exception>
        public static DateSearchCriteria Parse( string text )
        {
            ParseResult result = TryParseInternal( text, out DateSearchCriteria criteria );
            switch ( result )
            {
                case ParseResult.InputCannotBeNull:
                    throw new ArgumentNullException( nameof( text ) );
                case ParseResult.UnrecognizedCriteriaType:
                    throw new ArgumentException( $"The {nameof( DateSearchCriteriaType )} could not be determined from the input string.", nameof( text ) );
                case ParseResult.InvalidInputFormat:
                    throw new ArgumentException( "The provided string was invalid for the criteria type it was detected as.", nameof( text ) );
                case ParseResult.InvalidNumber1:
                    throw new ArgumentException( "The first number could not be parsed from the provided string.", nameof( text ) );
                case ParseResult.InvalidNumber2:
                    throw new ArgumentException( "The second number could not be parsed from the provided string.", nameof( text ) );
                case ParseResult.InvalidDateUnit:
                    throw new ArgumentException( "The date unit could not be parsed from the provided string.", nameof( text ) );
                case ParseResult.Success:
                    return criteria;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Attempts to safely parse the provided string as a valid instance of
        /// <see cref="DateSearchCriteria"/>.
        /// </summary>
        /// <param name="text">The valid string representation of a date filter
        /// that should be parsed.</param>
        /// <param name="criteria">An out parameter for the <see cref="DateSearchCriteria"/>
        /// that was successfully parsed. This will be a valid instance if the return value
        /// is true, or null if the return value is false.</param>
        /// <returns>Returns true if <paramref name="text"/> could be parsed successfully
        /// as a valid instance, or false otherwise. This also indicates what the value of
        /// <paramref name="criteria"/> can be expected to be: a valid instance (true) or null
        /// (false).</returns>
        public static bool TryParse( string text, out DateSearchCriteria criteria )
        {
            if ( TryParseInternal( text, out criteria ) == ParseResult.Success )
            {
                return true;
            }

            criteria = null;
            return false;
        }

        /// <inheritdoc />
        public override DateSearchCriteria Clone()
        {
            return new DateSearchCriteria
            {
                Type = Type,
                DateUnit = DateUnit,
                Number1 = Number1,
                Number2 = Number2
            };
        }

        /// <inheritdoc />
        public override bool Equals( DateSearchCriteria other )
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

        static ParseResult TryParseInternal( string str, out DateSearchCriteria criteria )
        {
            if ( str == null )
            {
                criteria = null;
                return ParseResult.InputCannotBeNull;
            }

            str = str.Trim();

            if ( str.StartsWith( "<", StringComparison.InvariantCultureIgnoreCase ) )
            {
                return TryParseBefore( str, out criteria );
            }

            if ( str.StartsWith( ">", StringComparison.InvariantCultureIgnoreCase ) )
            {
                return TryParseAfter( str, out criteria );
            }

            if ( str.Contains( "-" ) )
            {
                return TryParseBetween( str, out criteria );
            }

            return TryParseExactly( str, out criteria );
        }

        static ParseResult TryParseBefore( string str, out DateSearchCriteria criteria )
        {
            const int PrefixLength = 2;
            criteria = new DateSearchCriteria
            {
                Type = DateSearchCriteriaType.Before
            };

            int firstWhitespaceAfterNumber = str.IndexOf( ' ', PrefixLength );
            string number1Str = str.Substring( PrefixLength, firstWhitespaceAfterNumber - PrefixLength );
            if ( TryParseValidNumber( number1Str, out int number1 ) != TryParseNumberResult.Success )
            {
                return ParseResult.InvalidNumber1;
            }

            if ( !TryParseDateUnit( str, firstWhitespaceAfterNumber, out DateField dateField ) )
            {
                return ParseResult.InvalidDateUnit;
            }

            criteria.Number1 = number1;
            criteria.DateUnit = dateField;
            return ParseResult.Success;
        }

        static ParseResult TryParseAfter( string str, out DateSearchCriteria criteria )
        {
            const int PrefixLength = 2;
            criteria = new DateSearchCriteria
            {
                Type = DateSearchCriteriaType.After
            };

            int firstWhitespaceAfterNumber = str.IndexOf( ' ', PrefixLength );
            if ( firstWhitespaceAfterNumber < 0 )
            {
                return ParseResult.InvalidInputFormat;
            }

            string number1Str = str.Substring( PrefixLength, firstWhitespaceAfterNumber - PrefixLength );
            if ( TryParseValidNumber( number1Str, out int number1 ) != TryParseNumberResult.Success )
            {
                return ParseResult.InvalidNumber1;
            }

            if ( !TryParseDateUnit( str, firstWhitespaceAfterNumber, out DateField dateField ) )
            {
                return ParseResult.InvalidDateUnit;
            }

            criteria.Number1 = number1;
            criteria.DateUnit = dateField;
            return ParseResult.Success;
        }

        static ParseResult TryParseBetween( string str, out DateSearchCriteria criteria )
        {
            criteria = new DateSearchCriteria
            {
                Type = DateSearchCriteriaType.Between
            };

            int indexOfHyphen = str.IndexOf( '-' );
            if ( indexOfHyphen < 0 )
            {
                return ParseResult.InvalidInputFormat;
            }

            string number1Str = str.Substring( 0, indexOfHyphen );
            if ( TryParseValidNumber( number1Str, out int number1 ) != TryParseNumberResult.Success )
            {
                return ParseResult.InvalidNumber1;
            }

            int firstWhitespaceAfterNumbers = str.IndexOf( ' ', indexOfHyphen + 2 );
            if ( firstWhitespaceAfterNumbers < 0 )
            {
                return ParseResult.InvalidInputFormat;
            }

            string number2Str = str.Substring( indexOfHyphen + 1, firstWhitespaceAfterNumbers - indexOfHyphen - 1 );
            if ( TryParseValidNumber( number2Str, out int number2 ) != TryParseNumberResult.Success )
            {
                return ParseResult.InvalidNumber2;
            }

            if ( !TryParseDateUnit( str, firstWhitespaceAfterNumbers, out DateField dateField ) )
            {
                return ParseResult.InvalidDateUnit;
            }

            criteria.Number1 = number1;
            criteria.Number2 = number2;
            criteria.DateUnit = dateField;
            return ParseResult.Success;
        }

        static ParseResult TryParseExactly( string str, out DateSearchCriteria criteria )
        {
            criteria = new DateSearchCriteria
            {
                Type = DateSearchCriteriaType.Exactly
            };

            int firstWhitespaceAfterNumbers = str.IndexOf( ' ' );
            if ( firstWhitespaceAfterNumbers < 0 )
            {
                return ParseResult.UnrecognizedCriteriaType;
            }

            string number1Str = str.Substring( 0, firstWhitespaceAfterNumbers );
            TryParseNumberResult result = TryParseValidNumber( number1Str, out int number1 );
            switch ( result )
            {
                case TryParseNumberResult.NotAnInteger:
                    return ParseResult.UnrecognizedCriteriaType;

                case TryParseNumberResult.NumberNotValid:
                    return ParseResult.InvalidNumber1;

                case TryParseNumberResult.Success:
                    {
                        int firstWhitespaceAfterNumber = str.IndexOf( ' ' );
                        if ( !TryParseDateUnit( str, firstWhitespaceAfterNumber, out DateField dateField ) )
                        {
                            return ParseResult.InvalidDateUnit;
                        }

                        criteria.Number1 = number1;
                        criteria.DateUnit = dateField;
                        return ParseResult.Success;
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        static bool TryParseDateUnit( string str, int firstWhitespaceAfterNumbers, out DateField field )
        {
            const int SuffixLength = 5; // Length of string "s ago"

            if ( firstWhitespaceAfterNumbers < 0 )
            {
                field = default( DateField );
                return false;
            }

            string dateStr = str.Substring( firstWhitespaceAfterNumbers, str.Length - firstWhitespaceAfterNumbers - SuffixLength );
            return Enum.TryParse( dateStr, true, out field );
        }
    }
}
