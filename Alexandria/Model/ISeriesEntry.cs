using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Model
{
	public interface ISeriesEntry
	{
		ISeries Series { get; }

		Int32 EntryNumber { get; }

		String PreviousEntryHandle { get; }

		String NextEntryHandle { get; }
	}
}
