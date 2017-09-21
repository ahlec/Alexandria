﻿// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria
{
    public sealed class LibrarySourceConfig
    {
        static LibrarySourceConfig()
        {
            Default = new LibrarySourceConfig
            {
                CacheLifetime = TimeSpan.FromDays( 1 ),
                CachedObjects = CacheableObjects.All,
                CacheBaseDirectory = "cache"
            };
            Default.Seal();
        }

        public static LibrarySourceConfig Default { get; }

        public CacheableObjects CachedObjects
        {
            get => _cachedObjects;
            set => SetInternalValue( ref _cachedObjects, value );
        }

        public string CacheBaseDirectory
        {
            get => _cacheBaseDirectory;
            set => SetInternalValue( ref _cacheBaseDirectory, value );
        }

        public TimeSpan CacheLifetime
        {
            get => _cacheLifetime;
            set => SetInternalValue( ref _cacheLifetime, value );
        }

        public bool IsSealed { get; private set; }

        public void Seal()
        {
            if ( IsSealed )
            {
                throw new InvalidOperationException( $"This {nameof( LibrarySourceConfig )} has already been sealed!" );
            }

            IsSealed = true;
        }

        void SetInternalValue<T>( ref T backingVariable, T newValue )
        {
            if ( IsSealed )
            {
                throw new InvalidOperationException( $"You cannot modify any properties on a {nameof( LibrarySourceConfig )} after it has been sealed!" );
            }

            backingVariable = newValue;
        }

        CacheableObjects _cachedObjects;
        string _cacheBaseDirectory;
        TimeSpan _cacheLifetime;
    }
}
