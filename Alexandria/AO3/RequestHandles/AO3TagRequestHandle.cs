// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.RequestHandles;

namespace Alexandria.AO3.RequestHandles
{
    internal sealed class AO3TagRequestHandle : ITagRequestHandle
    {
        public AO3TagRequestHandle( string tagName )
        {
            Text = tagName;
        }

        public string Text { get; }

        public override string ToString()
        {
            return Text;
        }
    }
}
