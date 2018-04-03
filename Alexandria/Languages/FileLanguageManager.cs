// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Alexandria.Net;
using Newtonsoft.Json;

namespace Alexandria.Languages
{
    /// <summary>
    /// An implementation of <see cref="ILanguageManager"/> that will parse from a specified
    /// file on the local filesystem, additionally monitoring that file for changes and
    /// updating the internal store as appropriate.
    /// </summary>
    public sealed class FileLanguageManager : ILanguageManager
    {
        readonly string _filename;
        readonly IFileSystem _filesystem;
        readonly object _databaseLock = new object();
        LanguageDatabase _database;
        bool _hasPerformedInitialFetch;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLanguageManager"/> class.
        ///  </summary>
        /// <param name="filename">The name of the JSON file on the local filesystem.</param>
        /// <param name="filesystem">The filesystem abstraction that can be used to perform the
        /// filesystem IO operations.</param>
        public FileLanguageManager( string filename, IFileSystem filesystem )
        {
            _filename = filename?.Trim() ?? throw new ArgumentNullException( nameof( filename ) );
            _filesystem = filesystem ?? throw new ArgumentNullException( nameof( filesystem ) );

            if ( _filename.Equals( string.Empty ) )
            {
                throw new ArgumentException("Filename cannot be an empty string or composed only of whitespace.", nameof(filename));
            }

            if ( !_filesystem.File.Exists( _filename ) )
            {
                throw new FileNotFoundException( "The provided file does not exist on the filesystem provided." );
            }
        }

        /// <inheritdoc />
        public IReadOnlyList<Language> AllLanguages
        {
            get
            {
                if (!_hasPerformedInitialFetch)
                {
                    Refetch();
                }

                lock (_databaseLock)
                {
                    return _database.AllLanguages;
                }
            }
        }

        /// <inheritdoc />
        public Language GetLanguage( string input )
        {
            input = input?.Trim() ?? throw new ArgumentNullException(nameof(input));
            if (input.Equals(string.Empty))
            {
                throw new ArgumentException("Input cannot be an empty string or composed only of whitespace.", nameof(input));
            }

            bool retryIfNotFound = true;
            if (!_hasPerformedInitialFetch)
            {
                Refetch();
                retryIfNotFound = false;
            }

            return GetLanguage(input, retryIfNotFound);
        }

        Language GetLanguage( string input, bool retryIfNotFound )
        {
            LanguageDatabaseEntry entry;
            lock (_databaseLock)
            {
                entry = _database.Entries.FirstOrDefault(e => e.IsEntryFor(input));
            }

            if (entry != null)
            {
                return entry.Language;
            }

            if (retryIfNotFound)
            {
                Refetch();
                return GetLanguage(input, false);
            }

            return null;
        }

        void Refetch()
        {
            using (Stream fileContents = _filesystem.File.OpenRead(_filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader reader = new StreamReader(fileContents))
                {
                    using (JsonReader jsonReader = new JsonTextReader(reader))
                    {
                        lock (_databaseLock)
                        {
                            _database = serializer.Deserialize<LanguageDatabase>(jsonReader);
                        }
                    }
                }
            }

            _hasPerformedInitialFetch = true;
        }
    }
}
