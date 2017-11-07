// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Alexandria.AO3.RequestHandles;
using Alexandria.Caching;
using Alexandria.Exceptions.Input;
using Alexandria.Net;
using Alexandria.RequestHandles;
using Alexandria.Searching;

namespace Alexandria.AO3
{
    public class AO3Source : LibrarySource
    {
        public AO3Source( IWebClient webClient, Cache cache )
            : base( webClient, cache )
        {
        }

        /// <inheritdoc />
        public override string SourceHandle => "ao3";

        /// <inheritdoc />
        public override LibrarySearch MakeSearch()
        {
            return new AO3Search( this );
        }

        /// <summary>
        /// Creates a new request handle for searching for a particular author on AO3. Use the overload of this function
        /// to provide the pseud of the author if attempting to retrieve the non-primary profile of an author.
        /// <para />
        /// Parameter can be validated ahead of time by using <see cref="AO3Validation.IsValidAuthorName"/>.
        /// </summary>
        /// <param name="username">The primary username of the author in question. This cannot be null
        /// and must adhere to the requirements of a valid AO3 author name.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="username"/> is null.</exception>
        /// <exception cref="InvalidAuthorAlexandriaException">Thrown when <paramref name="username"/> does not meet the
        /// criteria for valid author names for AO3.</exception>
        /// <returns>A new request handle that can be used to retrieve information about the specific author on AO3.</returns>
        public override IAuthorRequestHandle MakeAuthorRequest( string username )
        {
            return MakeAuthorRequest( username, null );
        }

        /// <summary>
        /// Creates a new request handle for searching for a particular author on AO3.
        /// <para />
        /// Parameters can be validated ahead of time by using <see cref="AO3Validation.IsValidAuthorName"/>.
        /// </summary>
        /// <param name="username">The primary username of the author in question. This cannot be null
        /// and must adhere to the requirements of a valid AO3 author name.</param>
        /// <param name="pseud">The specific pseud of the author in question. This may be null in order to
        /// retrieve information about the author's primary account. If this is not null, however, it must
        /// adhere to the requirements of a valid AO3 author name.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="username"/> is null.</exception>
        /// <exception cref="InvalidAuthorAlexandriaException">Thrown when <paramref name="username"/> or when
        /// <paramref name="pseud"/>, if provided, do not meet the criteria for valid author names for AO3. When
        /// thrown, which of the two fields can be determined by examining the properties of the exception.</exception>
        /// <returns>A new request handle that can be used to retrieve information about the specific author on AO3.</returns>
        public IAuthorRequestHandle MakeAuthorRequest( string username, string pseud )
        {
            if ( username == null )
            {
                throw new ArgumentNullException( nameof( username ) );
            }

            if ( !AO3Validation.IsValidAuthorName( username ) )
            {
                throw new InvalidAuthorAlexandriaException( username, nameof( username ) );
            }

            if ( pseud != null && !AO3Validation.IsValidAuthorName( pseud ) )
            {
                throw new InvalidAuthorAlexandriaException( pseud, nameof( pseud ) );
            }

            return new AO3AuthorRequestHandle( this, username, pseud );
        }

        /// <summary>
        /// Creates a new request handle for looking up information about a particular character on AO3.
        /// <para />
        /// Parameter can be validated ahead of time by using <see cref="AO3Validation.IsValidTag"/> (since characters are actually tags on AO3).
        /// </summary>
        /// <param name="fullName">The full name of the character to search for. Case should not matter.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="fullName"/> is null.</exception>
        /// <exception cref="InvalidTagAlexandriaException">Thrown when <paramref name="fullName"/> is not valid to be used as a tag.</exception>
        /// <returns>A new request handle that can be used to retrieve information about the specific character on AO3.</returns>
        public override ICharacterRequestHandle MakeCharacterRequest( string fullName )
        {
            if ( fullName == null )
            {
                throw new ArgumentNullException( nameof( fullName ) );
            }

            if ( !AO3Validation.IsValidTag( fullName ) )
            {
                throw new InvalidTagAlexandriaException( fullName, nameof( fullName ) );
            }

            return new AO3CharacterRequestHandle( this, fullName );
        }

        /// <inheritdoc />
        public override IFanficRequestHandle MakeFanficRequest( string handle )
        {
            if ( string.IsNullOrEmpty( handle ) )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            if ( handle.Any( character => !char.IsDigit( character ) ) )
            {
                throw new ArgumentException( "Handles to fanfics on AO3 may only consist of numbers.", nameof( handle ) );
            }

            return new AO3FanficRequestHandle( this, handle );
        }

        /// <inheritdoc />
        public override ISeriesRequestHandle MakeSeriesRequest( string handle )
        {
            if ( string.IsNullOrEmpty( handle ) )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            if ( handle.Any( character => !char.IsDigit( character ) ) )
            {
                throw new ArgumentException( "Handles for series on AO3 may only consist of numbers.", nameof( handle ) );
            }

            return new AO3SeriesRequestHandle( this, handle );
        }

        /// <summary>
        /// Creates a new request handle for looking up information about a particular relationship on AO3.
        /// <para />
        /// Parameter can be validated ahead of time by using <see cref="AO3Validation.IsValidTag"/> (since ships are actually tags on AO3).
        /// </summary>
        /// <param name="tag">The name of the ship that should be requested. Case should not matter.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="tag"/> is null.</exception>
        /// <exception cref="InvalidTagAlexandriaException">Thrown when <paramref name="tag"/> is not valid to be used as a tag (as ships are tags within AO3).</exception>
        /// <returns>A new request handle that can be used to retrieve information about the specific ship on AO3.</returns>
        public override IShipRequestHandle MakeShipRequest( string tag )
        {
            if ( tag == null )
            {
                throw new ArgumentNullException( nameof( tag ) );
            }

            if ( !AO3Validation.IsValidTag( tag ) )
            {
                throw new InvalidTagAlexandriaException( tag, nameof( tag ) );
            }

            return new AO3ShipRequestHandle( this, tag );
        }

        /// <summary>
        /// Creates a new request handle for looking up information about a specific tag on AO3.
        /// <para />
        /// Parameter can be validated ahead of time by using <see cref="AO3Validation.IsValidTag"/>.
        /// </summary>
        /// <param name="tag">The tag that should be requested. Case should not matter.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="tag"/> is null.</exception>
        /// <exception cref="InvalidTagAlexandriaException">Thrown when <paramref name="tag"/> is not valid to be used as a tag.</exception>
        /// <returns>A new request handle that can be used to retrieve information about the specific tag on AO3.</returns>
        public override ITagRequestHandle MakeTagRequest( string tag )
        {
            if ( tag == null )
            {
                throw new ArgumentNullException( nameof( tag ) );
            }

            if ( !AO3Validation.IsValidTag( tag ) )
            {
                throw new InvalidTagAlexandriaException( tag, nameof( tag ) );
            }

            return new AO3TagRequestHandle( this, tag );
        }
    }
}
