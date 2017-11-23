// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Querying;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
    /// <summary>
    /// A model which has the ability to query fanfics that are directly related to it.
    /// </summary>
    public interface IQueryable : IRequestable
    {
        /// <summary>
        /// Queries the original <see cref="LibrarySource"/> to retrieve a page of fanfics which
        /// match this requestable (for instance, are written by this author, or are tagged with
        /// this tag).
        /// </summary>
        /// <returns>The first page of results, which can be then be queried further to retrieve
        /// additional pages.</returns>
        IQueryResultsPage<IFanfic, IFanficRequestHandle> QueryFanfics();
    }
}
