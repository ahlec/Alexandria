// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.Model;
using HtmlAgilityPack;

namespace Alexandria.AO3.Utils
{
    public static class AO3ContentWarningUtils
    {
        public static IEnumerable<string> GetIds( ContentWarnings warnings )
        {
            if ( warnings.HasFlag( ContentWarnings.Undetermined ) )
            {
                yield return "14";
            }

            if ( warnings.HasFlag( ContentWarnings.Violence ) )
            {
                yield return "17";
            }

            if ( warnings.HasFlag( ContentWarnings.MajorCharacterDeath ) )
            {
                yield return "18";
            }

            if ( warnings.HasFlag( ContentWarnings.None ) )
            {
                yield return "16";
            }

            if ( warnings.HasFlag( ContentWarnings.Rape ) )
            {
                yield return "19";
            }

            if ( warnings.HasFlag( ContentWarnings.Underage ) )
            {
                yield return "20";
            }
        }
    }
}
