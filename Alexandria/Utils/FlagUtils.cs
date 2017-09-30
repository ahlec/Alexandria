// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Utils
{
    public static class FlagUtils
    {
        public static bool HasMultipleFlagsSet( this Enum value )
        {
            long intValue = Convert.ToInt64( value );

            // http://www.graphics.stanford.edu/~seander/bithacks.html#DetermineIfPowerOf2
            // If the value isn't a power of two, then that means that there are multiple flags set.
            return ( ( intValue & ( intValue - 1 ) ) != 0 );
        }
    }
}
