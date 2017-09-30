// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.AO3.Utils
{
    internal static class AO3RequestUtils
    {
        internal static string GetRequestUriForTag( string tag )
        {
            tag = tag.Replace( "/", "*s*" );
            return $"http://archiveofourown.org/tags/{tag}";
        }
    }
}
