// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3.Model;
using Alexandria.AO3.Utils;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.RequestHandles
{
    internal sealed class AO3CharacterRequestHandle : AO3TagRequestHandleBase<ICharacter>, ICharacterRequestHandle
    {
        public AO3CharacterRequestHandle( AO3Source source, string name )
            : base( source, name )
        {
        }

        public string FullName => TagName;

        /// <inheritdoc />
        protected override string RequestCacheHandle => $"ao3-character-{FullName}";

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
