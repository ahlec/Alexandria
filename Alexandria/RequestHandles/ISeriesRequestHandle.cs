// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Model;

namespace Alexandria.RequestHandles
{
    /// <summary>
    /// A request handle that is configured to retrieve data about a series of fanfics from
    /// the website.
    /// </summary>
    public interface ISeriesRequestHandle : IRequestHandle<ISeries>
    {
        /// <summary>
        /// Gets the unique handle of the series that is being requetsed. The format of this
        /// can change between different websites, but it is a guarantee that, within the website
        /// itself, a series will be uniquely identified by a particular handle (there is only one
        /// handle per series and only one series per handle).
        /// </summary>
        string Handle { get; }
    }
}
