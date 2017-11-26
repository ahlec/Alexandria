// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Model
{
    /// <summary>
    /// The content rating of a fanfic. This will usually indicate who the target
    /// audience of the fanfic is, as well as the level of content that might be
    /// deemed inappropriate is that the reader will encounter within the fanfic.
    /// </summary>
    public enum MaturityRating
    {
        /// <summary>
        /// This fanfic was not given a maturity rating by the author.
        /// <para />
        /// MPAA Rating: NR (Not Rated) / UR (Unrated)
        /// </summary>
        NotRated,

        /// <summary>
        /// This fanfic is suitable for a reader of any age and will most likely not
        /// contain any references to sex, violence, drugs, or other such topics.
        /// <para />
        /// MPAA Rating: G (General Audiences) / PG (Parent Guidance Suggested)
        /// </summary>
        General,

        /// <summary>
        /// This fanfic might contain some slight mentions or allusions to sex, violence,
        /// drugs, or other such topics, but likely won't expand on them beyond a cursory
        /// mention.
        /// <para />
        /// MPAA Rating: PG-13 (Parents Strongly Cautioned)
        /// </summary>
        Teen,

        /// <summary>
        /// This fanfic will contain some mentions of sex, violence, drugs, and/or other
        /// such topics, but these topics might still be eschewed from delving into them
        /// in great detail.
        /// <para />
        /// MPAA Rating: R (Restricted)
        /// </summary>
        Adult,

        /// <summary>
        /// This fanfic will deal explicitly with sex, violence, drugs, and/or other such
        /// topics, usually focusing exclusively on one or more of them for a prolonged
        /// period of time.
        /// <para />
        /// MPAA Rating: NC-17 (Adults Only)
        /// </summary>
        Explicit
    }
}
