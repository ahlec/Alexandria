// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
    public interface ISeries : IRequestable
    {
        IAuthorRequestHandle Author { get; }

        DateTime DateStarted { get; }

        DateTime DateLastUpdated { get; }

        bool IsCompleted { get; }

        IReadOnlyList<IFanficRequestHandle> Fanfics { get; }
    }
}
