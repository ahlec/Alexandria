// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3.Model;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.RequestHandles
{
    /// <summary>
    /// A concrete class for requesting a tag from AO3.
    /// </summary>
    internal sealed class AO3TagRequestHandle : AO3TagRequestHandleBase<ITag>, ITagRequestHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AO3TagRequestHandle"/> class.
        /// </summary>
        /// <param name="source">The source that the HTML page came from, which is then used to
        /// request the model from the website. It is also passed along to the model when parsed
        /// as well in order to make future chained requests or query fanfics.</param>
        /// <param name="tagName">The name of the tag that can be requested.</param>
        public AO3TagRequestHandle( AO3Source source, string tagName )
            : base( source, tagName )
        {
        }

        /// <inheritdoc />
        public string Text => TagName;

        /// <inheritdoc />
        protected override string RequestCacheHandle => $"ao3-tag-{Text}";

        /// <summary>
        /// Gets the string representation of this request handle. The functionality of this is
        /// here for the purposes of ease of debugging and the result of this function should not
        /// be relied on to be in any particular format as it is liable to change as necessary.
        /// </summary>
        /// <returns>A human-readible string that represents the data contained in this request handle.</returns>
        public override string ToString()
        {
            return Text;
        }

        /// <inheritdoc />
        protected override ITag ParseRequest( HtmlCacheableDocument requestDocument )
        {
            return AO3Tag.Parse( Source, requestDocument );
        }
    }
}
