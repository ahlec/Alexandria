using System;

namespace Alexandria.Searching
{
	public sealed class ExactNumberSearchCriteria : NumberSearchCriteria
	{
		public ExactNumberSearchCriteria( Int32 number )
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
			return new ExactNumberSearchCriteria( Number );
		}

		/// <inheritdoc />
		public override String ToString()
		{
			return Number.ToString();
		}

		Int32 _number;
	}
}
