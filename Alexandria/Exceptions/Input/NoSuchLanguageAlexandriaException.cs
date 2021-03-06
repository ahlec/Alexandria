﻿// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Exceptions.Input
{
    public sealed class NoSuchLanguageAlexandriaException : AlexandriaException
    {
        internal NoSuchLanguageAlexandriaException( string value )
        {
            InvalidValue = value;
        }

        public string InvalidValue { get; }
    }
}
