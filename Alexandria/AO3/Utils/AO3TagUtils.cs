// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.AO3.Utils
{
    /// <summary>
    /// A utility class for functions related to tags on AO3 that cannot be stored into a base utility
    /// class because these functions are needed by different classes.
    /// </summary>
    internal static class AO3TagUtils
    {
        /// <summary>
        /// Escapes a tag in order to use it as part of a URL. Certain characters that are used in tags,
        /// such as # or /, have a separate meaning in URL strings and would be lost, so AO3 escapes them
        /// in order to transmit the full tag.
        /// </summary>
        /// <param name="tag">The tag that is going to be used in a URL string.</param>
        /// <returns>A tag that is escaped in order to not interfere with a regular URL and in order to be
        /// interpretable by AO3.</returns>
        //
        // Reference: owarchive/app/models/tag.rb as of c27d54cf14a3356e296bd1f0c4ca3796eb0076fa
        //            Function: to_param
        public static string EscapeTagForUrl( string tag )
        {
            tag = tag.Replace( "/", "*s*" );
            tag = tag.Replace( "&", "*a*" );
            tag = tag.Replace( ".", "*d*" );
            tag = tag.Replace( "?", "*q*" );
            tag = tag.Replace( "#", "*h*" );
            return tag;
        }
    }
}
