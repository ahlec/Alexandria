using System;

namespace Alexandria.Searching
{
	public sealed class LessThanNumberSearchCriteria : NumberSearchCriteria
	{
		public LessThanNumberSearchCriteria( Int32 number )
		{
			Number = number;
		}

		public Int32 Number
		{
			get => _number;
			set => _number = ValidateNumber( value );
		}

		/// <inheritdoc />
		public override NumberSearchCriteria Clone()
		{
			return new LessThanNumberSearchCriteria( Number );
		}

		/// <inheritdoc />
		public override String ToString()
		{
			return String.Concat( "<", Number );
		}

		Int32 _number;
	}
}
