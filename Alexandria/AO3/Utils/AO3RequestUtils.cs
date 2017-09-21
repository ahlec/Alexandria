// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Alexandria.AO3.RequestHandles;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.Utils
{
    public static class AO3RequestUtils
    {
        public static IFanficRequestHandle MakeFanficRequest( string handle )
        {
            if ( string.IsNullOrEmpty( handle ) )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            if ( handle.Any( character => !char.IsDigit( character ) ) )
            {
                throw new ArgumentException( "Handles to fanfics on AO3 may only consist of numbers.", nameof( handle ) );
            }

            return new AO3FanficRequestHandle( handle );
        }

        public static IAuthorRequestHandle MakeAuthorRequest( string username, string pseud = null )
        {
            if ( string.IsNullOrEmpty( username ) )
            {
                throw new ArgumentNullException( nameof( username ) );
            }

            if ( string.IsNullOrWhiteSpace( pseud ) )
            {
                pseud = null;
            }

            return new AO3AuthorRequestHandle( username, pseud );
        }

        public static ITagRequestHandle MakeTagRequest( string tag )
        {
            if ( string.IsNullOrEmpty( tag ) )
            {
                throw new ArgumentNullException( nameof( tag ) );
            }

            return new AO3TagRequestHandle( tag );
        }

        public static IShipRequestHandle MakeShipRequest( string tag )
        {
            if ( string.IsNullOrEmpty( tag ) )
            {
                throw new ArgumentNullException( nameof( tag ) );
            }

            return new AO3ShipRequestHandle( tag );
        }

        public static ISeriesRequestHandle MakeSeriesRequest( string handle )
        {
            if ( string.IsNullOrEmpty( handle ) )
            {
                throw new ArgumentNullException( nameof( handle ) );
            }

            if ( handle.Any( character => !char.IsDigit( character ) ) )
            {
                throw new ArgumentException( "Handles for series on AO3 may only consist of numbers.", nameof( handle ) );
            }

            return new AO3SeriesRequestHandle( handle );
        }
    }
}
