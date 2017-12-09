// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Net;
using Alexandria.Exceptions.Net;

namespace Alexandria.Net
{
    /// <summary>
    /// A standard implementation of <seealso cref="IWebClient"/> that performs the HTTP requests
    /// on the public internet using <seealso cref="HttpWebRequest"/>.
    /// </summary>
    public sealed class HttpWebClient : IWebClient
    {
        /// <inheritdoc />
        public WebResult Get( string uri )
        {
            if ( uri == null )
            {
                throw new ArgumentNullException( nameof( uri ) );
            }

            if ( string.IsNullOrWhiteSpace( uri ) )
            {
                throw new ArgumentException( "The parameter must not be empty or whitespace.", nameof( uri ) );
            }

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create( uri );
            request.Method = "GET";

            HttpWebResponse response = PerformHttpWebRequest( uri, request );
            return new WebResult( response.ResponseUri, response.GetResponseStream() );
        }

        static HttpWebResponse PerformHttpWebRequest( string uri, HttpWebRequest request )
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                if ( response.StatusCode != HttpStatusCode.OK )
                {
                    throw CreateExceptionFromStatusCode( response.StatusCode, uri );
                }

                return response;
            }
            catch ( WebException e )
            {
                if ( e.Status == WebExceptionStatus.Timeout )
                {
                    throw new WebRequestTimedOutException( uri );
                }

                throw;
            }
        }

        static NetException CreateExceptionFromStatusCode( HttpStatusCode code, string uri )
        {
            switch ( code )
            {
                case HttpStatusCode.NotFound: return new WebsiteNotFoundNetException( uri );
                default: return new HttpStatusNetException( uri, code );
            }
        }
    }
}
