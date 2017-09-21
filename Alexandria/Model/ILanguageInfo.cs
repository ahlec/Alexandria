// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Model
{
    public interface ILanguageInfo
    {
        Language Language { get; }

        string DisplayName { get; }

        string NativeName { get; }
    }
}
