// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;

namespace Alexandria.Net
{
    /// <summary>
    /// A class that is returned as the result of performing an HTTP GET request using
    /// <seealso cref="IWebClient.Get"/>.
    /// </summary>
    public sealed class WebResult : IDisposable
    {
        readonly Stream _responseStream;
        bool _hasGottenResponseText;
        string _responseText;
        bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebResult"/> class.
        /// </summary>
        /// <param name="responseUri">The URL of the HTTP response after any redirects are processed.</param>
        /// <param name="responseStream">The stream returned from the web result.</param>
        public WebResult( Uri responseUri, Stream responseStream )
        {
            ResponseUri = responseUri ?? throw new ArgumentNullException( nameof( responseUri ) );
            _responseStream = responseStream ?? throw new ArgumentNullException( nameof( responseStream ) );
        }

        /// <summary>
        /// Gets the URL of the HTTP response.
        /// <para />
        /// Note that because of HTTP redirects, it's possible that this won't be the URL that was originally
        /// requested. If the URL that was requested originally indicated that HTTP traffic to it should be
        /// redirected to a separate URL, then that redirect will be followed automatically. This property is
        /// the final URL where the data was downloaded from once any and all redirects were followed.
        /// </summary>
        public Uri ResponseUri { get; }

        /// <summary>
        /// Gets the content downloaded from the HTTP request as a string. This is a cached call so this property
        /// can be accessed multiple times without causing additional network operations.
        /// </summary>
        public string ResponseText
        {
            get
            {
                if ( !_hasGottenResponseText )
                {
                    using ( StreamReader reader = new StreamReader( _responseStream ) )
                    {
                        _responseText = reader.ReadToEnd();
                        _hasGottenResponseText = true;
                    }
                }

                return _responseText;
            }
        }

        /// <summary>
        /// Disposes of the <seealso cref="WebResult"/>.
        /// </summary>
        public void Dispose()
        {
            if ( _isDisposed )
            {
                return;
            }

            _isDisposed = true;
            _responseStream.Dispose();
        }
    }
}
