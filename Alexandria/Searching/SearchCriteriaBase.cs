// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Searching
{
    /// <summary>
    /// A base class for all comparison-based search fields in a <seealso cref="LibrarySearch"/>,
    /// such as fields that complexly compare numbers or fields that compare dates. This class
    /// provides shared base functionality for these classes.
    /// </summary>
    /// <typeparam name="TSelf">The child class itself. This allows the base class to have functions
    /// which return the specific child type, such as <see cref="Clone"/>, as well as implementing
    /// the <seealso cref="IEquatable{T}"/> interface.</typeparam>
    /// <typeparam name="TTypeEnum">The type of the enum that is used for the <see cref="Type"/>
    /// property. This enum should detail how <see cref="Number1"/> and <see cref="Number2"/> behave
    /// relative to each other when using the search criteria.</typeparam>
    public abstract class SearchCriteriaBase<TSelf, TTypeEnum> : IEquatable<TSelf>
        where TSelf : SearchCriteriaBase<TSelf, TTypeEnum>
        where TTypeEnum : struct
    {
        /// <summary>
        /// The maximum number value that <seealso cref="Number1"/> or
        /// <seealso cref="Number2"/> can be. Using a number larger than this will cause
        /// an <seealso cref="ArgumentOutOfRangeException" />.
        /// </summary>
        public const int MaximumNumberValue = 9999999;

        static uint _nextInternalId = 1;
        readonly uint _internalId;
        int _number1;
        int _number2;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCriteriaBase{TSelf, TTypeEnum}"/> class.
        /// </summary>
        internal SearchCriteriaBase()
        {
            _internalId = _nextInternalId++;
        }

        /// <summary>
        /// An enum of all of the possible return codes that
        /// <see cref="SearchCriteriaBase{TSelf,TTypeEnum}.TryParseValidNumber"/>
        /// can return.
        /// </summary>
        protected enum TryParseNumberResult
        {
            /// <summary>
            /// An ERROR code that indicates that the provided input parameter was not an integer.
            /// </summary>
            NotAnInteger,

            /// <summary>
            /// An ERROR code that indicates that the provided input parameter was outside the bounds
            /// that numbers are required to be within in order to be used in a
            /// <see cref="SearchCriteriaBase{TSelf,TTypeEnum}"/>.
            /// </summary>
            NumberNotValid,

            /// <summary>
            /// A SUCCESS code that indicates that everything went correctly and the out parameter is
            /// valid to use.
            /// </summary>
            Success
        }

        enum ValidateNumberResult
        {
            NumberTooSmall,
            NumberTooGreat,
            Success
        }

        /// <summary>
        /// Gets or sets the type of comparison combining <see cref="Number1"/>
        /// and <see cref="Number2"/>. Based on this value, the usage of these
        /// numbers will change meanings.
        /// </summary>
        public TTypeEnum Type { get; set; }

        /// <summary>
        /// Gets or sets the first number of the comparison. The usage of this number can
        /// take on various different (though intuitive) meanings, which can be figured out
        /// by looking at the documentation for the values of the <see cref="Type"/> enum.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when setting the number and
        /// the new value is either less than 0 or greater than <seealso cref="MaximumNumberValue"/>.</exception>
        public int Number1
        {
            get => _number1;
            set => _number1 = ValidateNumber( value );
        }

        /// <summary>
        /// Gets or sets the second number of the comparison. The usage of this number can
        /// take on various different (though intuitive) meanings, which can be figured out
        /// by looking at the documentation for for the values of the <see cref="Type"/> enum.
        /// <para />
        /// NOTE: Most types will not use a second number. However, you will still be able to access
        /// and mutate this property. If using a criteria type that doesn't include a second number (see
        /// type documentation) however, this value will be completely ignored.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when setting the number and
        /// the new value is either less than 0 or greater than <seealso cref="MaximumNumberValue"/>.</exception>
        public int Number2
        {
            get => _number2;
            set => _number2 = ValidateNumber( value );
        }

        /// <summary>
        /// Creates a clone of this instance with the same values and configuration.
        /// </summary>
        /// <returns>A new instance of this object with all of the same values and configurations.</returns>
        public abstract TSelf Clone();

        /// <summary>
        /// Determines if this instance is equal to the provided other instance.
        /// </summary>
        /// <param name="other">The other instance to compare this instance against.</param>
        /// <returns>Returns true if all of the values and configurations are shared between
        /// this instance and the provided instance, or false if there is some difference
        /// in values between the two instances or if the provided instance is null.</returns>
        public abstract bool Equals( TSelf other );

        /// <inheritdoc />
        public sealed override bool Equals( object obj )
        {
            if ( obj == null )
            {
                return false;
            }

            if ( ReferenceEquals( obj, this ) )
            {
                return true;
            }

            TSelf other = obj as TSelf;
            return Equals( other );
        }

        /// <inheritdoc />
        public sealed override int GetHashCode()
        {
            return _internalId.GetHashCode();
        }

        /// <summary>
        /// Attempts to parse the provided string as a valid number that can be used within a
        /// <see cref="SearchCriteriaBase{TSelf,TTypeEnum}"/>.
        /// </summary>
        /// <param name="str">The input string that should be in the form of a string representation
        /// of a valid integer within the range allowed by the class.</param>
        /// <param name="number">An OUT parameter that will contain the valid number in the case
        /// where it could be parsed into a valid number (the return value is
        /// <seealso cref="TryParseNumberResult.Success"/>), or undefined in the case where it couldn't be.</param>
        /// <returns>Returns the error/success result code with whether the operation was successful,
        /// or if it failed for a specific reason.</returns>
        protected static TryParseNumberResult TryParseValidNumber( string str, out int number )
        {
            if ( !int.TryParse( str, out number ) )
            {
                return TryParseNumberResult.NotAnInteger;
            }

            if ( TryValidateNumber( number ) != ValidateNumberResult.Success )
            {
                return TryParseNumberResult.NumberNotValid;
            }

            return TryParseNumberResult.Success;
        }

        static ValidateNumberResult TryValidateNumber( int number )
        {
            if ( number < 0 )
            {
                return ValidateNumberResult.NumberTooSmall;
            }

            if ( number > MaximumNumberValue )
            {
                return ValidateNumberResult.NumberTooGreat;
            }

            return ValidateNumberResult.Success;
        }

        static int ValidateNumber( int number )
        {
            switch ( TryValidateNumber( number ) )
            {
                case ValidateNumberResult.NumberTooSmall:
                    throw new ArgumentOutOfRangeException( nameof( number ), "The number must be zero or positive." );

                case ValidateNumberResult.NumberTooGreat:
                    throw new ArgumentOutOfRangeException( nameof( number ), $"The number must be less than or equal to {MaximumNumberValue}." );

                case ValidateNumberResult.Success:
                    return number;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
