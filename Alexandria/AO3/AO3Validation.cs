// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Utils;

namespace Alexandria.AO3
{
    public static class AO3Validation
    {
        public static bool IsValidAuthorName( string name )
        {
            return IsValidAuthorName( name, out string _ );
        }

        public static bool IsValidAuthorName( string name, out string reason )
        {
            if ( name == null )
            {
                reason = "No author name was provided (name is null)";
                return false;
            }

            if ( name.Length < MinNameLength || name.Length > MaxNameLength )
            {
                reason = $"Name was {name.Length} character{( name.Length == 1 ? string.Empty : "s" )} long but must be between {MinNameLength} and {MaxNameLength} characters long.";
                return false;
            }

            bool hasNumberOrLetter = false;
            foreach ( char character in name )
            {
                if ( !IsValidCharacterForAuthorName( character ) )
                {
                    reason = $"Name contains '{character}', which is an invalid character for an author name";
                    return false;
                }

                if ( char.IsLetterOrDigit( character ) )
                {
                    hasNumberOrLetter = true;
                }
            }

            if ( !hasNumberOrLetter )
            {
                reason = "Name must contain at least one letter or digit.";
                return false;
            }

            reason = null;
            return true;
        }

        static bool IsValidCharacterForAuthorName( char character )
        {
            if ( char.IsLetterOrDigit( character ) )
            {
                return true;
            }

            if ( CharUtils.IsSpace( character ) )
            {
                return true;
            }

            if ( character == '_' )
            {
                return true;
            }

            if ( CharUtils.IsDash( character ) )
            {
                return true;
            }

            return false;
        }

        const int MinNameLength = 1;
        const int MaxNameLength = 40;
    }
}
