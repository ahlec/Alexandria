// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Model;

namespace Alexandria.Utils
{
    public static class MaturityRatingUtils
    {
        public static string GetDisplayName( this MaturityRating rating )
        {
            switch ( rating )
            {
                case MaturityRating.NotRated:
                    return "Not Rated";
                default:
                    return rating.ToString();
            }
        }
    }
}
