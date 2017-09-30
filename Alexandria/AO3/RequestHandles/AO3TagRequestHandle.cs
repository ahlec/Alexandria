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
    internal sealed class AO3TagRequestHandle : RequestHandleBase<ITag, AO3Source>, ITagRequestHandle
    {
        public AO3TagRequestHandle( AO3Source source, string tagName )
            : base( source )
        {
            Text = tagName;
        }

        public string Text { get; }

        /// <inheritdoc />
        protected override string RequestUri => AO3RequestUtils.GetRequestUriForTag( Text );

        /// <inheritdoc />
        protected override string RequestCacheHandle => $"ao3-tag-{Text}";

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
