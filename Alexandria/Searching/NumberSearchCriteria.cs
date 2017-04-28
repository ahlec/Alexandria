using System;

namespace Alexandria.Searching
{
	public abstract class NumberSearchCriteria
	{
		protected virtual Int32 ValidateNumber( Int32 number )
		{
			if ( number < 0 )
			{
				throw new ArgumentOutOfRangeException( nameof( number ), "The number must be zero or positive." );
			}

			return number;
		}

		public abstract NumberSearchCriteria Clone();

		/// <inheritdoc />
		public override String ToString()
		{
			throw new NotImplementedException();
		}
	}
}
