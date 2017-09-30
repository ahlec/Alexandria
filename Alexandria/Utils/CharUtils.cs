// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Utils
{
    internal static class CharUtils
    {
        public static bool IsSpace( char character )
        {
            switch ( character )
            {
                case ' ':
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsDash( char character )
        {
            switch ( character )
            {
                case '-': // hyphen minus
                case 'ー': // CJK dash
                    return true;
                default:
                    return false;
            }
        }
    }
}
