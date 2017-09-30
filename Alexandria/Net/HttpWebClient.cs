// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Net;

namespace Alexandria.Net
{
    public sealed class HttpWebClient : IWebClient
    {
        public WebResult Get( string uri )
        {
            if ( string.IsNullOrWhiteSpace( uri ) )
            {
                throw new ArgumentNullException( nameof( uri ) );
            }

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create( uri );
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            if ( response.StatusCode != HttpStatusCode.OK )
            {
                throw CreateExceptionFromStatusCode( response.StatusCode, uri );
            }

            return new WebResult( response.ResponseUri, response.GetResponseStream() );
        }

        static WebException CreateExceptionFromStatusCode( HttpStatusCode code, string uri )
        {
            switch ( code )
            {
                case HttpStatusCode.Unauthorized: return new WebException( $"[401] You must be authorized in order to access {uri}." );
                case HttpStatusCode.Forbidden: return new WebException( $"[403] {uri} is forbidden." );
                case HttpStatusCode.NotFound: return new WebException( $"[404] {uri} could not be found." );
                default: return new WebException( $"[{(int) code}] Response code from {uri} was not OK." );
            }
        }
    }
}
