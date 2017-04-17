using System;

namespace Alexandria.Searching
{
	public sealed class RangeNumberSearchCriteria : NumberSearchCriteria
	{
		public RangeNumberSearchCriteria( Int32 minimum, Int32 maximum )
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		public Int32 Minimum
		{
			get => _minimum;
			set => _minimum = ValidateNumber( value );
		}

		public Int32 Maximum
		{
			get => _maximum;
			set => _maximum = ValidateNumber( value );
		}

		/// <inheritdoc />
		public override String ToString()
		{
			return String.Concat( Minimum, "-", Maximum );
		}

		Int32 _minimum;
		Int32 _maximum;
	}
}
