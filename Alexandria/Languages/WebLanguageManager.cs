// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alexandria.Net;
using Newtonsoft.Json;

namespace Alexandria.Languages
{
    /// <summary>
    /// An implementation of <see cref="ILanguageManager"/> that will download a Language.json file
    /// from a URL using HTTP requests. This Language.json file has all of the data needed to populate
    /// the full list of recognized <see cref="Language"/> instances.
    /// </summary>
    public sealed class WebLanguageManager : ILanguageManager
    {
        /// <summary>
        /// The master repository version of Languages.json. Using this endpoint ensures that your application can be updated
        /// to reflect the latest data that the websites are using without being forced to update your code to the latest
        /// versions or without your applications breaking.
        /// </summary>
        public const string AlexandriaRepositoryEndpoint = "https://raw.githubusercontent.com/ahlec/Alexandria/master/Languages.json";

        readonly IWebClient _webClient;
        readonly string _endpoint;
        readonly object _databaseLock = new object();
        LanguageDatabase _database;
        bool _hasPerformedInitialFetch;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebLanguageManager"/> class.
        ///  </summary>
        /// <param name="webClient">The web client that is used to perform HTTP requests.</param>
        /// <param name="endpoint">The endpoint where the language database exists that can be fetched via HTTP requests. It's
        /// highly recommended that you use <seealso cref="AlexandriaRepositoryEndpoint"/> in order to keep up to date with the
        /// latest data versions without needing to sync your code version with each data change.</param>
        public WebLanguageManager( IWebClient webClient, string endpoint = AlexandriaRepositoryEndpoint )
        {
            _webClient = webClient ?? throw new ArgumentNullException( nameof( webClient ) );
            _endpoint = endpoint?.Trim() ?? throw new ArgumentNullException( nameof( endpoint ) );

            if ( _endpoint.Equals( string.Empty ) )
            {
                throw new ArgumentException( "Endpoint cannot be an empty string or composed only of whitespace.", nameof( endpoint ) );
            }
        }

        /// <inheritdoc />
        public IReadOnlyList<Language> AllLanguages
        {
            get
            {
                if ( !_hasPerformedInitialFetch )
                {
                    Refetch();
                }

                lock ( _databaseLock )
                {
                    return _database.AllLanguages;
                }
            }
        }

        /// <inheritdoc />
        public Language GetLanguage( string input )
        {
            input = input?.Trim() ?? throw new ArgumentNullException( nameof( input ) );
            if ( input.Equals( string.Empty ) )
            {
                throw new ArgumentException( "Input cannot be an empty string or composed only of whitespace.", nameof( input ) );
            }

            bool retryIfNotFound = true;
            if ( !_hasPerformedInitialFetch )
            {
                Refetch();
                retryIfNotFound = false;
            }

            return GetLanguage( input, retryIfNotFound );
        }

        Language GetLanguage( string input, bool retryIfNotFound )
        {
            LanguageDatabaseEntry entry;
            lock ( _databaseLock )
            {
                entry = _database.Entries.FirstOrDefault( e => e.IsEntryFor( input ) );
            }

            if ( entry != null )
            {
                return entry.Language;
            }

            if ( retryIfNotFound )
            {
                Refetch();
                return GetLanguage( input, false );
            }

            return null;
        }

        void Refetch()
        {
            WebResult result = _webClient.Get( _endpoint );

            JsonSerializer serializer = new JsonSerializer();
            using ( StringReader reader = new StringReader( result.ResponseText ) )
            {
                using ( JsonReader jsonReader = new JsonTextReader( reader ) )
                {
                    lock ( _databaseLock )
                    {
                        _database = serializer.Deserialize<LanguageDatabase>( jsonReader );
                    }
                }
            }

            _hasPerformedInitialFetch = true;
        }
    }
}
