// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.RequestHandles;

namespace Alexandria
{
    /// <summary>
    /// Any item which is requestable from a website by using an appropriately
    /// configured and typed <seealso cref="IRequestHandle{TModel}"/>.
    /// </summary>
    public interface IRequestable
    {
        /// <summary>
        /// Gets the website that this request handle will be retrieving data from.
        /// </summary>
        Website Website { get; }

        /// <summary>
        /// Gets the URL of the webpage that is being requested.
        /// </summary>
        Uri Url { get; }
    }
}
