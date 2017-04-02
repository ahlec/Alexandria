using System;

namespace Alexandria.Utils
{
	public static class FlagUtils
	{
		public static Boolean HasMultipleFlagsSet( this Enum value )
		{
			Int64 intValue = Convert.ToInt64( value );

			// http://www.graphics.stanford.edu/~seander/bithacks.html#DetermineIfPowerOf2
			// If the value isn't a power of two, then that means that there are multiple flags set.
			return ( ( intValue & ( intValue - 1 ) ) != 0 );
		}
	}
}
