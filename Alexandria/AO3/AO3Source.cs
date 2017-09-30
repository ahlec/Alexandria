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

        /// <inheritdoc />
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
        /// <returns>A new request handle that can be requested to retrieve information about the specific author on AO3.</returns>
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

        /// <inheritdoc />
        public override ICharacterRequestHandle MakeCharacterRequest( string fullName )
        {
            if ( string.IsNullOrEmpty( fullName ) )
            {
                throw new ArgumentNullException( nameof( fullName ) );
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

        /// <inheritdoc />
        public override IShipRequestHandle MakeShipRequest( string tag )
        {
            if ( string.IsNullOrEmpty( tag ) )
            {
                throw new ArgumentNullException( nameof( tag ) );
            }

            return new AO3ShipRequestHandle( this, tag );
        }

        /// <inheritdoc />
        public override ITagRequestHandle MakeTagRequest( string tag )
        {
            if ( string.IsNullOrEmpty( tag ) )
            {
                throw new ArgumentNullException( nameof( tag ) );
            }

            return new AO3TagRequestHandle( this, tag );
        }
    }
}
