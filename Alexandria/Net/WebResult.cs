// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.IO;

namespace Alexandria.Net
{
    public sealed class WebResult : IDisposable
    {
        public WebResult( Uri responseUri, Stream responseStream )
        {
            ResponseUri = responseUri ?? throw new ArgumentNullException( nameof( responseUri ) );
            _responseStream = responseStream ?? throw new ArgumentNullException( nameof( responseStream ) );
        }

        public Uri ResponseUri { get; }

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

        public void Dispose()
        {
            if ( _isDisposed )
            {
                return;
            }

            _isDisposed = true;
            _responseStream.Dispose();
        }

        readonly Stream _responseStream;
        bool _hasGottenResponseText;
        string _responseText;
        bool _isDisposed;
    }
}
