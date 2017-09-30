// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Alexandria.Model;

namespace Alexandria.Utils
{
    public static class LanguageUtils
    {
        class LanguageInfo : ILanguageInfo
        {
            public LanguageInfo( Language language )
            {
                Language = language;

                MemberInfo memberInfo = typeof( Language ).GetMember( language.ToString() )[0];

                RenderLanguageNameAttribute displayName = memberInfo.GetCustomAttribute<RenderLanguageNameAttribute>();
                DisplayName = displayName?.Name ?? language.ToString();

                NativeLanguageAttribute nativeLanguage = memberInfo.GetCustomAttribute<NativeLanguageAttribute>();
                NativeName = nativeLanguage.Native;
            }

            public Language Language { get; }

            public string DisplayName { get; }

            public string NativeName { get; }
        }

        static LanguageUtils()
        {
            List<ILanguageInfo> infos = new List<ILanguageInfo>();
            foreach ( string value in Enum.GetNames( typeof( Language ) ) )
            {
                Language enumValue = (Language) Enum.Parse( typeof( Language ), value );
                LanguageInfo info = new LanguageInfo( enumValue );
                infos.Add( info );

                _languageStrings.Add( value, enumValue );
                if ( !_languageStrings.ContainsKey( info.NativeName ) )
                {
                    _languageStrings.Add( info.NativeName, enumValue );
                }
            }

            _info = infos;
        }

        public static ILanguageInfo GetInfo( Language language )
        {
            return _info[(int) language];
        }

        public static Language Parse( string str )
        {
            if ( string.IsNullOrWhiteSpace( str ) )
            {
                throw new ArgumentNullException( nameof( str ) );
            }

            str = str.Trim();

            if ( _languageStrings.TryGetValue( str, out Language language ) )
            {
                return language;
            }

            throw new ArgumentException( $"Could not parse to a {nameof( Language )} value." );
        }

        static readonly IReadOnlyList<ILanguageInfo> _info;
        static readonly IDictionary<string, Language> _languageStrings = new Dictionary<string, Language>( StringComparer.InvariantCultureIgnoreCase );
    }
}
