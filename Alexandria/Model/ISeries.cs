using System;
using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
	public interface ISeries : IRequestable
	{
		IAuthorRequestHandle Author { get; }

		DateTime DateStarted { get; }

		DateTime DateLastUpdated { get; }

		Boolean IsCompleted { get; }

		IReadOnlyList<IFanficRequestHandle> Fanfics { get; }
	}
}
