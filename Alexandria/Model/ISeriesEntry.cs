using System;

namespace Alexandria.Model
{
	public interface ISeriesEntry
	{
		IRequestHandle<ISeries> Series { get; }

		Int32 EntryNumber { get; }

		IRequestHandle<IFanfic> PreviousEntry { get; }

		IRequestHandle<IFanfic> NextEntry { get; }
	}
}
