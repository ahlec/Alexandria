// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.AO3.Model;
using Alexandria.Model;

namespace Alexandria.Exceptions
{
    /// <summary>
    /// An exception to be thrown when a tag is being parsed by a class and the page being retrieved indicates
    /// that the type of the tag is not valid for this class (for instance, a tag which was a character was attempted
    /// to be parsed as a <seealso cref="AO3Ship"/>).
    /// </summary>
    public sealed class InvalidTagTypeAlexandriaException : AlexandriaException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTagTypeAlexandriaException"/> class.
        /// </summary>
        /// <param name="expected">The expected tag type for this class.</param>
        /// <param name="actual">The actual tag type encountered.</param>
        /// <param name="tagName">The name of the tag.</param>
        internal InvalidTagTypeAlexandriaException( TagType expected, TagType actual, string tagName )
            : base( $"Encountered a {actual} tag that was supposed to be a {expected} tag in order to be used here." )
        {
            Expected = expected;
            Actual = actual;
            TagName = tagName;
        }

        /// <summary>
        /// Gets the expected tag type.
        /// </summary>
        public TagType Expected { get; }

        /// <summary>
        /// Gets the actual tag type encountered.
        /// </summary>
        public TagType Actual { get; }

        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        public string TagName { get; }
    }
}
