// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.AO3.Data
{
    /// <summary>
    /// A concrete class for storing information and different representations of an enum on AO3.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum as defined within Alexandria.</typeparam>
    internal sealed class AO3EnumDefinition<TEnum>
        where TEnum : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AO3EnumDefinition{TEnum}"/> class.
        /// </summary>
        /// <param name="enumValue">The value of the enum that is defined here.</param>
        /// <param name="id">The ID number used to represent this enum value on AO3's website.</param>
        /// <param name="websiteText">The text that AO3 will use on their website to represent this text</param>
        public AO3EnumDefinition( TEnum enumValue, int id, string websiteText )
        {
            EnumValue = enumValue;
            Id = id.ToString();
            WebsiteText = websiteText;
        }

        /// <summary>
        /// Gets the value of the enum that is defined here.
        /// </summary>
        public TEnum EnumValue { get; }

        /// <summary>
        /// Gets the ID number used to represent this enum value on AO3's website.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the text that AO3 will use on their website to represent this text and which
        /// can, case-insensitively, be used to parse an enum value from a string.
        /// </summary>
        public string WebsiteText { get; }

        /// <summary>
        /// Determines if the provided string matches the text representation of this enum definition.
        /// This is case-insensitive.
        /// </summary>
        /// <param name="str">The provided string to match against.</param>
        /// <returns>Returns true if this enum matches the provided string, or false otherwise.</returns>
        public bool Matches( string str )
        {
            return WebsiteText.Equals( str, StringComparison.InvariantCultureIgnoreCase );
        }
    }
}
