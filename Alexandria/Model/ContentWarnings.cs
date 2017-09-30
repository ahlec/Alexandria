// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Model
{
    [Flags]
    public enum ContentWarnings
    {
        None = 0,
        Undetermined = 1,
        Violence = 2,
        MajorCharacterDeath = 4,
        Rape = 8,
        Underage = 16
    }
}
