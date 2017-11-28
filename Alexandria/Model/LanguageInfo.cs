// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Alexandria.Model
{
    /// <summary>
    /// A class which stores information about a language and how it can be represented to the user.
    /// <para />
    /// There is no public constructor for this class, and all data therein is immutable. Two languages
    /// will have the same reference to instances of their info, and instances of this data can be
    /// retrieved through the use of <see cref="Languages"/>.
    /// </summary>
    public sealed class LanguageInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageInfo"/> class.
        /// </summary>
        /// <param name="language">The enum value of the language.</param>
        /// <param name="ao3Id">The unique ID of the language as defined on AO3.</param>
        /// <param name="displayName">The display name of the language, in English.</param>
        /// <param name="nativeName">The name of the language, written in the language itself.</param>
        internal LanguageInfo( Language language, int ao3Id, string displayName, string nativeName )
        {
            Language = language;
            AO3Id = ao3Id.ToString();
            DisplayName = displayName;
            NativeName = nativeName;
        }

        /// <summary>
        /// Gets the enum value for the language. This can be used for things like equality, comparison,
        /// or filtering.
        /// </summary>
        public Language Language { get; }

        /// <summary>
        /// Gets the display name for the language. This will be the name of the language value, in English,
        /// as can be displayed to the reader. This will pretty much always match the enum value, just with
        /// spaces and/or punctuation that cannot exist within an enum value.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Gets the name of this language as written in the language itself (using the language's
        /// writing system and own word for the language).
        /// <para />
        /// Example: English - English, Japanese: 日本語
        /// </summary>
        public string NativeName { get; }

        /// <summary>
        /// Gets the unique ID number for this language when used on AO3.
        /// </summary>
        internal string AO3Id { get; }

        /// <summary>
        /// Adds all unique string representations/names of this language to the provided hash set. This
        /// is done in this way to prevent a lot of unnecessary allocation of new hash sets on startup.
        /// <para />
        /// NOTE: If you aren't the static constructor for <seealso cref="Languages"/>, you probably
        /// don't want this function.
        /// </summary>
        /// <param name="uniqueNames">The hash set which should be mutated in place.</param>
        internal void AddUniqueNames( HashSet<string> uniqueNames )
        {
            uniqueNames.Add( Language.ToString() );
            uniqueNames.Add( DisplayName );
            uniqueNames.Add( NativeName );
        }
    }
}
