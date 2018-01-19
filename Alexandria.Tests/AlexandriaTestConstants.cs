// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Tests
{
    internal static class AlexandriaTestConstants
    {
        /// <summary>
        /// A category for tests that should be run every 24 hours. These are usually
        /// tests that ensure that data that is liable to change without notice is kept
        /// up to date.
        /// </summary>
        public const string NightlyTestsCategory = "Nightly Tests";
    }
}
