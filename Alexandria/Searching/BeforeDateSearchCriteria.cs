using System;

namespace Alexandria.Searching
{
	public sealed class BeforeDateSearchCriteria : DateSearchCriteria
	{
		public BeforeDateSearchCriteria( DateField unit, Int32 amount ) : base( unit )
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
			return new BeforeDateSearchCriteria( DateUnit, Amount );
		}

		/// <inheritdoc />
		public override String ToString()
		{
			return String.Concat( "> ", Amount, " ", DateUnit, "s ago" );
		}

		Int32 _amount;
	}
}
