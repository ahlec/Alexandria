using System;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
	public interface ISeriesEntry
	{
		ISeriesRequestHandle Series { get; }

		Int32 EntryNumber { get; }

		IFanficRequestHandle PreviousEntry { get; }

		IFanficRequestHandle NextEntry { get; }
	}
}
