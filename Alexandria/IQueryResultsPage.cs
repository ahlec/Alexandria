using System;
using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria
{
	public interface IQueryResultsPage<TModel, TRequestHandle> where TModel : IRequestable where TRequestHandle : IRequestHandle<TModel>
	{
		IReadOnlyList<TRequestHandle> Results { get; }

		Boolean HasMoreResults { get; }

		IQueryResultsPage<TModel, TRequestHandle> RetrieveNextPage();
	}
}
