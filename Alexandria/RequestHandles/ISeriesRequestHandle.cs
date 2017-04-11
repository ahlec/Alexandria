using System;
using Alexandria.Model;

namespace Alexandria.RequestHandles
{
	public interface ISeriesRequestHandle : IRequestHandle<ISeries>
	{
		String Handle { get; }
	}
}
