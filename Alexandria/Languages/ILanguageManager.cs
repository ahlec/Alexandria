// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Alexandria.Languages
{
    /// <summary>
    /// An interface for a dependency that handles the recognization and translation of languages
    /// from their forms on various websites into <seealso cref="Language"/> elements.
    /// <para />
    /// The standard implementation that would be recommended for this is <seealso cref="WebLanguageManager"/>.
    /// You are capable of writing or using your own implementation, but this can come with its own set of
    /// risks given that the various websites are prone to change their language set at any given time without
    /// notice.
    /// </summary>
    public interface ILanguageManager
    {
        /// <summary>
        /// Gets a list of all of the languages managed by this manager.
        /// </summary>
        IReadOnlyList<Language> AllLanguages { get; }

            /// <summary>
        /// Retrieves the appropriate language entry given the provided input.
        /// </summary>
        /// <param name="input">A string representation of a recognized language from one of the
        /// supported <seealso cref="Website"/>s.</param>
        /// <returns>Returns the language represented by the input parameter, if there is one. If the
        /// input parameter does not represent any known language, this will return null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="input"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="input"/> is empty or only composed of whitespace.</exception>
        Language GetLanguage( string input );
    }
}
