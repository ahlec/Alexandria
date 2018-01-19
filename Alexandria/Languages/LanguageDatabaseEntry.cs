// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alexandria.Languages
{
    [JsonObject]
    internal sealed class LanguageDatabaseEntry
    {
        [JsonIgnore]
        public Language Language { get; private set; }

        [JsonProperty( Required = Required.Always )]
        internal string EnglishName { get; set; }

        [JsonProperty( Required = Required.Always )]
        internal string NativeName { get; set; }

        [JsonProperty]
        internal string TwoLetterISOCode { get; set; }

        [JsonProperty]
        internal string ThreeLetterISOCode { get; set; }

        [JsonProperty( Required = Required.Always )]
        internal string AO3Name { get; set; }

        [JsonProperty( Required = Required.Always )]
        internal int AO3Id { get; set; }

        /// <summary>
        /// Determines whether this database entry is the entry that represents the
        /// provided language input.
        /// </summary>
        /// <param name="input">The prospective language to test if this entry
        /// represents or not.</param>
        /// <returns>Returns true if this database entry represents the langauge provided,
        /// or false otherwise.</returns>
        public bool IsEntryFor( string input )
        {
            return AO3Name.Equals( input, StringComparison.InvariantCultureIgnoreCase );
        }

        [OnDeserialized]
        void OnDeserialized( StreamingContext streamingContext )
        {
            Language = new Language( EnglishName, NativeName, TwoLetterISOCode, ThreeLetterISOCode, AO3Id );
        }
    }
}
