// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace Alexandria.Caching
{
    // TODO: Implement fully (and keep in mind that we're using `using` elsewhere which means we can't store data in MemoryStream because we'll Dispose() it after first use
    /*
    public sealed class MemoryCache : Cache
    {
        /// <inheritdoc />
        internal override bool Contains<TDocument>( string handle )
        {
            EntryKey key = new EntryKey( typeof( TDocument ), handle );
            return _entries.ContainsKey( key );
        }

        /// <inheritdoc />
        internal override void WriteToCache<TDocument>(TDocument document)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override Stream GetCachedDocumentStream<TDocument>(string handle)
        {
            throw new NotImplementedException();
        }

        readonly Dictionary<EntryKey, MemoryStream> _entries = new Dictionary<EntryKey, MemoryStream>();

        sealed class EntryKey : IEquatable<EntryKey>
        {
            public EntryKey( Type documentType, string handle )
            {
                _documentType = documentType;
                _handle = handle;
            }

            readonly Type _documentType;
            readonly string _handle;

            public bool Equals( EntryKey other )
            {
                if ( other == null )
                {
                    return false;
                }

                return ( _documentType == other._documentType && _handle.Equals( other._handle ) );
            }

            /// <inheritdoc />
            public override bool Equals( object obj )
            {
                return Equals( obj as EntryKey );
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                int hashCode = 17;
                hashCode = ( hashCode * 23 ) + _documentType.GetHashCode();
                hashCode = ( hashCode * 23 ) + _handle.GetHashCode();
                return hashCode;
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return $"[{_documentType.Name}] {_handle}";
            }
        }
    }
    */
}
