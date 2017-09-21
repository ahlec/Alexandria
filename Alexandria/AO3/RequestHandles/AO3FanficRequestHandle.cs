// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.RequestHandles
{
    internal sealed class AO3FanficRequestHandle : IFanficRequestHandle
    {
        public AO3FanficRequestHandle( string handle )
        {
            Handle = handle;
        }

        public string Handle { get; }

        public override string ToString()
        {
            return Handle;
        }

        internal static AO3FanficRequestHandle ParseFromWorkLi( HtmlNode li )
        {
            string fanficHandle = li.GetAttributeValue( "id", null ).Substring( "work_".Length );
            return new AO3FanficRequestHandle( fanficHandle );
        }
    }
}
