// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
    /// <summary>
    /// A character from a fanfic.
    /// <para />
    /// Fanfics and characters have a 1:many relationship. Additionally, characters and ships
    /// have a many:many relationship.
    /// </summary>
    public interface ICharacter : IRequestable
    {
        /// <summary>
        /// Gets the fandoms that this character is from, if there are any.
        /// </summary>
        IReadOnlyList<ITagRequestHandle> Fandoms { get; }

        /// <summary>
        /// Gets a list of other names that this character might go by.
        /// <para />
        /// Note that this could be a disambiguation of names if this character was actually
        /// a generic name (ie, if the character were "Keith" this could be a list of all of
        /// the characters whose name is Keith).
        /// </summary>
        IReadOnlyList<ICharacterRequestHandle> AlternateNames { get; }

        /// <summary>
        /// Gets a list of ships the character is involved with.
        /// <para />
        /// Note that for a character with a lot of ships, this might not be a comprehensive list due
        /// to websites restricting how much data will be returned for the sake of bandwidth (and
        /// reasonability). Also, this list could might contain duplicate names for a ship as it isn't
        /// a distinct list of relationships this character is involved in.
        /// </summary>
        IReadOnlyList<IShipRequestHandle> Relationships { get; }

        /// <summary>
        /// Queries the original <see cref="LibrarySource"/> to retrieve a page of fanfics
        /// which contain this character.Subsequent pages can be called by using the result
        /// to ask for them.
        /// </summary>
        /// <returns>The first page of fanfics which contain this character.</returns>
        IQueryResultsPage<IFanfic, IFanficRequestHandle> QueryFanfics();
    }
}
