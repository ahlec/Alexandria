// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.RequestHandles;

namespace Alexandria.AO3.RequestHandles
{
    /// <summary>
    /// A base class for request handles for tag-based data from AO3. This can be a character, a ship,
    /// or an actual, legitimate tag (freeform or otherwise).
    /// </summary>
    /// <typeparam name="TModel">The interface for the data that is returned from this request handle.</typeparam>
    internal abstract class AO3TagRequestHandleBase<TModel> : RequestHandleBase<TModel, AO3Source>
        where TModel : IRequestable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AO3TagRequestHandleBase{TModel}"/> class.
        /// </summary>
        /// <param name="source">The configured class for accessing AO3.</param>
        /// <param name="tagName">The name of the tag to be retrieved from AO3.</param>
        protected AO3TagRequestHandleBase( AO3Source source, string tagName )
            : base( source )
        {
            TagName = tagName;
            RequestUri = CreateRequestUri( tagName );
        }

        /// <summary>
        /// Gets the name of the tag to be retrieved from AO3.
        /// </summary>
        protected string TagName { get; }

        /// <inheritdoc />
        protected sealed override string RequestUri { get; }

        static string CreateRequestUri( string tagName )
        {
            tagName = tagName.Replace( "/", "*s*" );
            tagName = tagName.Replace( "#", "*h*" );
            return $"http://archiveofourown.org/tags/{tagName}";
        }
    }
}
