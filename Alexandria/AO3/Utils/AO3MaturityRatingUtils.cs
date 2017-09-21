// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using Alexandria.Model;

namespace Alexandria.AO3.Utils
{
    public static class AO3MaturityRatingUtils
    {
        public static MaturityRating Parse( string name )
        {
            switch ( name.ToLowerInvariant() )
            {
                case "general audiences":
                    return MaturityRating.General;
                case "teen and up audiences":
                    return MaturityRating.Teen;
                case "mature":
                    return MaturityRating.Adult;
                case "explicit":
                    return MaturityRating.Explicit;
                case "not rated":
                    return MaturityRating.NotRated;
                default:
                    throw new ArgumentException( $"Unknown AO3 maturity rating: {name}", nameof( name ) );
            }
        }

        public static int GetId( MaturityRating rating )
        {
            switch ( rating )
            {
                case MaturityRating.NotRated:
                    return 9;
                case MaturityRating.General:
                    return 10;
                case MaturityRating.Teen:
                    return 11;
                case MaturityRating.Adult:
                    return 12;
                case MaturityRating.Explicit:
                    return 13;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
