// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Exceptions.Input
{
    public sealed class InvalidAuthorAlexandriaException : AlexandriaException
    {
        internal InvalidAuthorAlexandriaException( string value, string paramName )
        {
            InvalidValue = value;
            ParamName = paramName;
        }

        public string InvalidValue { get; }

        public string ParamName { get; }
    }
}
