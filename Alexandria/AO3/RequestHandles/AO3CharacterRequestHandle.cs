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
    internal sealed class AO3CharacterRequestHandle : AO3TagRequestHandleBase<ICharacter>, ICharacterRequestHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AO3CharacterRequestHandle"/> class.
        /// </summary>
        /// <param name="source">The source that the HTML page came from, which is then stored for
        /// querying fanfics and also passed along to any nested request handles for them to parse
        /// data with as well.</param>
        /// <param name="name">The name of the character that can be requested from the website.</param>
        public AO3CharacterRequestHandle( AO3Source source, string name )
            : base( source, name )
        {
        }

        /// <inheritdoc />
        public string FullName => TagName;

        /// <inheritdoc />
        protected override string RequestCacheHandle => $"ao3-character-{FullName}";

        /// <summary>
        /// Gets the string representation of this request handle. The functionality of this is
        /// here for the purposes of ease of debugging and the result of this function should not
        /// be relied on to be in any particular format as it is liable to change as necessary.
        /// </summary>
        /// <returns>A human-readible string that represents the data contained in this request handle.</returns>
        public override string ToString()
        {
            return FullName;
        }

        /// <inheritdoc />
        protected override ICharacter ParseRequest( HtmlCacheableDocument requestDocument )
        {
            return AO3Character.Parse( Source, requestDocument );
        }
    }
}
