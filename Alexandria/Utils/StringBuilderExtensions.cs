// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Text;

namespace Alexandria.Utils
{
    public static class StringBuilderExtensions
    {
        public static void TrimEnd( this StringBuilder builder )
        {
            if ( builder == null || builder.Length == 0 )
            {
                return;
            }

            int numberCharactersToRemove = 0;
            for ( int index = builder.Length - 1; index >= 0; --index )
            {
                if ( char.IsWhiteSpace( builder[index] ) )
                {
                    ++numberCharactersToRemove;
                }
                else
                {
                    break;
                }
            }

            if ( numberCharactersToRemove > 0 )
            {
                builder.Remove( builder.Length - numberCharactersToRemove, numberCharactersToRemove );
            }
        }

        public static void TrimStart( this StringBuilder builder )
        {
            if ( builder == null || builder.Length == 0 )
            {
                return;
            }

            int numberCharactersToRemove = 0;
            for ( int index = 0; index < builder.Length; ++index )
            {
                if ( char.IsWhiteSpace( builder[index] ) )
                {
                    ++numberCharactersToRemove;
                }
                else
                {
                    break;
                }
            }

            if ( numberCharactersToRemove > 0 )
            {
                builder.Remove( 0, numberCharactersToRemove );
            }
        }

        public static void Trim( this StringBuilder builder )
        {
            builder.TrimStart();
            builder.TrimEnd();
        }
    }
}
