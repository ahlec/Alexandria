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
        /// A category for tests whose purpose is to ensure that all of our code still matches
        /// what the various websites have (for instance, making sure that our enums match the
        /// values that the websites have). These are sort of like integration tests, but instead
        /// of us integrating two of our own components, we're checking to make sure that our components
        /// and their components can integrate successfully.
        /// </summary>
        public const string ConformityTestsCategory = "Conformity Tests";
    }
}
