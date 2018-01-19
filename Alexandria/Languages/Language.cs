// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Languages
{
    /// <summary>
    /// Representation about a particular language, along with information about that language.
    /// </summary>
    public sealed class Language : IEquatable<Language>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class.
        /// </summary>
        /// <param name="englishName">The display name of this language in readable English.</param>
        /// <param name="nativeName">The display name of this language as written in the language itself.</param>
        /// <param name="iso2Letter">The ISO 639-1 two-letter code for this language.</param>
        /// <param name="iso3Letter">The ISO 639-2 three-letter language code for this language.</param>
        /// <param name="ao3Id">The unique number that AO3 uses to represent this language.</param>
        public Language( string englishName, string nativeName, string iso2Letter, string iso3Letter, int ao3Id )
        {
            EnglishName = englishName?.Trim() ?? throw new ArgumentNullException( nameof( englishName ) );
            NativeName = nativeName?.Trim() ?? throw new ArgumentNullException( nameof( nativeName ) );
            TwoLetterISOCode = iso2Letter?.ToLowerInvariant();
            ThreeLetterISOCode = iso3Letter?.ToLowerInvariant();
            AO3Id = ao3Id;

            if ( EnglishName.Equals( string.Empty ) )
            {
                throw new ArgumentException( "The english name may not be empty or composed only of whitespace.", nameof( englishName ) );
            }

            if ( NativeName.Equals( string.Empty ) )
            {
                throw new ArgumentException( "The native name may not be empty or composed only of whitespace.", nameof( nativeName ) );
            }

            if ( TwoLetterISOCode != null && TwoLetterISOCode.Length != 2 )
            {
                throw new ArgumentException( "The ISO 639-1 two-letter code provided was not two characters in length.", nameof( iso2Letter ) );
            }

            if ( ThreeLetterISOCode != null && ThreeLetterISOCode.Length != 3 )
            {
                throw new ArgumentException( "The ISO 639-2 thre-letter code provided was not three characters in length.", nameof( iso3Letter ) );
            }

            if ( AO3Id < 1 )
            {
                throw new ArgumentOutOfRangeException( nameof( ao3Id ) );
            }
        }

        /// <summary>
        /// Gets the display name of this language in readable English.
        /// </summary>
        public string EnglishName { get; }

        /// <summary>
        /// Gets the display name of this language as written in the language itself.
        /// </summary>
        public string NativeName { get; }

        /// <summary>
        /// Gets the ISO 639-1 two-letter code for this language. Not all languages are guaranteed
        /// to have ISO codes, as not all languages are real, acknowledged languages. If this is not
        /// null, this is guaranteed to always be two characters in length and always lowercase.
        /// </summary>
        public string TwoLetterISOCode { get; }

        /// <summary>
        /// Gets the ISO 639-2 three-letter code for this language. Not all languages are guaranteed
        /// to have ISO codes, as not all languages are real, acknowledged languages. If this is not
        /// null, this is guaranteed to always be three characters in length and always lowercase.
        /// </summary>
        public string ThreeLetterISOCode { get; }

        /// <summary>
        /// Gets the unique number that AO3 uses to represent this language.
        /// </summary>
        public int AO3Id { get; }

        /// <summary>
        /// Determines if this language represents the same language as another instance.
        /// </summary>
        /// <param name="other">The other instance of this language to compare against.</param>
        /// <returns>Returns true if the provided instance represents the same data as this
        /// language, or false otherwise.</returns>
        public bool Equals( Language other )
        {
            return ( AO3Id == other?.AO3Id );
        }

        /// <inheritdoc />
        public override bool Equals( object obj )
        {
            return Equals( obj as Language );
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return AO3Id;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{EnglishName} [{ThreeLetterISOCode ?? "no code"}]";
        }
    }
}
