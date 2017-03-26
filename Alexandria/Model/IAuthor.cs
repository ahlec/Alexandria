using System;
using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
	public interface IAuthor : IRequestable
	{
		String Name { get; }

		IReadOnlyList<String> Nicknames { get; }

		DateTime DateJoined { get; }

		String Location { get; }

		DateTime? Birthday { get; }

		String Biography { get; }

		Int32 NumberFanfics { get; }

		IQueryResultsPage<IFanfic, IFanficRequestHandle> QueryFanfics();
	}
}
