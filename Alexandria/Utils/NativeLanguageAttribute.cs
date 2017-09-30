// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Utils
{
    [AttributeUsage( AttributeTargets.Field )]
    internal class NativeLanguageAttribute : Attribute
    {
        public NativeLanguageAttribute( string native )
        {
            Native = native;
        }

        public string Native { get; }
    }
}
