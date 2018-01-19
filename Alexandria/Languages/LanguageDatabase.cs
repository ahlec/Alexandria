// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alexandria.Languages
{
    [JsonObject]
    internal sealed class LanguageDatabase
    {
        /// <summary>
        /// Gets the individual, defined language entries stored in this database.
        /// </summary>
        [JsonProperty( Required = Required.Always )]
        internal LanguageDatabaseEntry[] Entries { get; set; }

        [JsonIgnore]
        internal IReadOnlyList<Language> AllLanguages { get; private set; }

        [OnDeserialized]
        void OnDeserialized( StreamingContext streamingContext )
        {
            AllLanguages = Entries.Select( entry => entry.Language ).ToList();
        }
    }
}
