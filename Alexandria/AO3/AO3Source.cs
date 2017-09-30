// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Alexandria.AO3.RequestHandles;
using Alexandria.Caching;
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

        public IAuthorRequestHandle MakeAuthorRequest( string username, string pseud )
        {
            if ( string.IsNullOrEmpty( username ) )
            {
                throw new ArgumentNullException( nameof( username ) );
            }

            if ( string.IsNullOrWhiteSpace( pseud ) )
            {
                pseud = null;
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
