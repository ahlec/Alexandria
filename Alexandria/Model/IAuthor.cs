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
    public interface IAuthor : IRequestable
    {
        string Name { get; }

        IReadOnlyList<string> Nicknames { get; }

        DateTime DateJoined { get; }

        string Location { get; }

        DateTime? Birthday { get; }

        string Biography { get; }

        int NumberFanfics { get; }

        IQueryResultsPage<IFanfic, IFanficRequestHandle> QueryFanfics();
    }
}
