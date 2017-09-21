// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.RequestHandles;

namespace Alexandria.Model
{
    public interface ISeriesEntry
    {
        ISeriesRequestHandle Series { get; }

        int EntryNumber { get; }

        IFanficRequestHandle PreviousEntry { get; }

        IFanficRequestHandle NextEntry { get; }
    }
}
