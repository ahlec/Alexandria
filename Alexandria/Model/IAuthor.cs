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
    /// <summary>
    /// An author of a fanfic.
    /// <para />
    /// Fanfics and authors have a many:1 relationship.
    /// </summary>
    public interface IAuthor : IQueryable
    {
        /// <summary>
        /// Gets the name of the author. This is their official name and the one that is their
        /// account name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the list of other names that this author goes by, if that feature is supported
        /// for this website.
        /// </summary>
        IReadOnlyList<string> Nicknames { get; }

        /// <summary>
        /// Gets the date that the author joined this website/created their account there.
        /// </summary>
        DateTime DateJoined { get; }

        /// <summary>
        /// Gets the location that the author has specified in the "Location" field of their profile.
        /// This doesn't have to correspond to any actual location in the real world, as many websites
        /// will allow for freeform entry for this field.
        /// </summary>
        string Location { get; }

        /// <summary>
        /// Gets the author's birthday if they have supplied it.
        /// </summary>
        DateTime? Birthday { get; }

        /// <summary>
        /// Gets the description/introduction message that the user has written about themselves for
        /// their profile.
        /// </summary>
        string Biography { get; }

        /// <summary>
        /// Gets the total number of fanfics that this author has published.
        /// </summary>
        int NumberFanfics { get; }
    }
}
