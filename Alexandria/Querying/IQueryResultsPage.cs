// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Querying
{
    public interface IQueryResultsPage<TModel, out TRequestHandle>
        where TModel : IRequestable
        where TRequestHandle : IRequestHandle<TModel>
    {
        IReadOnlyList<TRequestHandle> Results { get; }

        bool HasMoreResults { get; }

        IQueryResultsPage<TModel, TRequestHandle> RetrieveNextPage();
    }
}
