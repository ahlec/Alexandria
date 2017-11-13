// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Searching
{
    /// <summary>
    /// A field that can be sorted upon when retrieving results of a query. See also: <seealso cref="SortDirection"/>,
    /// which will indicate in which order the results should be returned based upon the selected field here.
    /// </summary>
    public enum SearchField
    {
        BestMatch,

        /// <summary>
        /// The author of the fanfiction. When sorting ascendingly, fanfics will be returned in the alphabetical (computer alphabetical)
        /// order based on the author's name (whichever name -- pseud or username -- was used to post the fanfic). This sorting is case
        /// insensitive.
        /// </summary>
        Author,

        /// <summary>
        /// The title of the fanfiction. When sorting ascendingly, fanfics will be returned in the alphabetical (computer alphabetical)
        /// order based on their titles. This sorting is case insensitive and will not include stop words (ie, 'An' or 'The' at the
        /// start of the title).
        /// </summary>
        Title,

        /// <summary>
        /// The date when this fanfic was first posted. When sorting ascendingly, fanfics that have been posted for longer (aka
        /// fanfics which were posted further in the past) will be returned before more recently posted fanfics.
        /// </summary>
        DatedPosted,

        /// <summary>
        /// The last date when this fanfiction was updated (either modified or when a new chapter was posted). When sorting
        /// ascendingly, fanfics that have not been updated in a long while will be returned before fanfics which were updated
        /// more recently.
        /// </summary>
        DateLastUpdated,

        /// <summary>
        /// The number of words in the fanfiction (the entire fanfiction, and not just a specific chapter). When sorting
        /// ascendingly, fanfics with fewer words will be returned before fanfics with more words.
        /// </summary>
        WordCount,

        /// <summary>
        /// The number of likes that a fanfiction has received. When sorting ascendingly, fanfics with fewer likes
        /// will be returned before fanfics with more likes.
        /// </summary>
        NumberLikes,

        /// <summary>
        /// The number of comments that a fanfiction has received. When sorting ascendingly, fanfics with fewer comments
        /// will be returned before fanfics with more comments.
        /// </summary>
        NumberComments
    }
}
