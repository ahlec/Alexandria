using System;

namespace Alexandria.Searching
{
	public abstract class DateSearchCriteria
	{
		protected DateSearchCriteria( DateField dateUnit )
		{
			DateUnit = dateUnit;
		}

		public DateField DateUnit { get; }

		protected virtual Int32 ValidateNumber( Int32 number )
		{
			if ( number < 0 )
			{
				throw new ArgumentOutOfRangeException( nameof( number ), "The number must be zero or positive." );
			}

			return number;
		}

		public abstract DateSearchCriteria Clone();

		/// <inheritdoc />
		public override String ToString()
		{
			throw new NotImplementedException( "Overrides must provide this!" );
		}
	}
}
