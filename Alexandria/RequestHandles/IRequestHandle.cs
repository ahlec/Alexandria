// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Model;

namespace Alexandria.RequestHandles
{
    /// <summary>
    /// A handle that contains the data and capability of requesting data from the website
    /// about a particular thing. Request handles are the primary way of collecting data
    /// from the web services.
    /// <para />
    /// Initially, a request handle can be created through the functions on <seealso cref="LibrarySource"/>
    /// classes, such as <seealso cref="LibrarySource.MakeFanficRequest"/>. Making a request handle doesn't
    /// actually communicate with the server. Rather, it creates a shell class that itself has the capability
    /// of requesting data from the server through the <seealso cref="Request"/> function. After
    /// the initial request handle has been made, all the data that comes back in the form of whatever
    /// model is will be additional request handles (for example, <seealso cref="IFanfic.Author"/>. This allows
    /// for the enduser to explore the data somewhat seemlessly by using these request handles and having them
    /// request the data only if necessary (because accessing this data will always result in another endpoint
    /// being called).
    /// <para />
    /// All of the request handles will work with the configuration that the original <seealso cref="LibrarySource"/>
    /// was set up with in regards to dependencies, caching, and other such configurations.
    /// </summary>
    /// <typeparam name="TModel">The type of the data that is returned by requesting data from this request handle.</typeparam>
    public interface IRequestHandle<out TModel>
        where TModel : IRequestable
    {
        /// <summary>
        /// Gets the website that this request handle will be retrieving data from.
        /// </summary>
        Website Website { get; }

        /// <summary>
        /// Requests the data from the website (or retrieves it from the cache if caching has been set up in the
        /// <seealso cref="LibrarySource"/>).
        /// <para />
        /// Requesting data multiple times from the same request handle will result in multiple calls being dispatched
        /// to the website (that is, there is no caching local to a request handle outside of the cache that is set up
        /// in the <seealso cref="LibrarySource"/>) so it is adviseable to hold onto the result of this function if
        /// this will be used multiple times.
        /// </summary>
        /// <returns>Returns the data that was retrieved from the website for this request as configured.</returns>
        TModel Request();
    }
}
