using System;

namespace Alexandria.Searching
{
	public sealed class ExactDateSearchCriteria : DateSearchCriteria
	{
		public ExactDateSearchCriteria( DateField unit, Int32 amount ) : base( unit )
		{
			Amount = amount;
		}

		public Int32 Amount
		{
			get => _amount;
			set => _amount = ValidateNumber( value );
		}

		/// <inheritdoc />
		public override DateSearchCriteria Clone()
		{
			return new ExactDateSearchCriteria( DateUnit, Amount );
		}

		/// <inheritdoc />
		public override String ToString()
		{
			return String.Concat( Amount, " ", DateUnit, "s ago" );
		}

		Int32 _amount;
	}
}
