// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using Alexandria.Exceptions.Input;
using Alexandria.Model;

namespace Alexandria.Utils
{
    public static class LanguageUtils
    {
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

        /// <summary>
        /// Gets the <see cref="Language"/> enum value of the provided string, capable of handling
        /// either enum value names, or the native names of the languages (such as 日本語 being returned
        /// as <see cref="Language.Japanese"/>).
        /// </summary>
        /// <param name="str">A valid language name, being either the enum value name (German) or the native
        /// name of the language (Deutsch).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null.</exception>
        /// <exception cref="NoSuchLanguageAlexandriaException">Thrown if <paramref name="str"/> is not a language name.</exception>
        /// <returns>The enum value of the language represented by the string.</returns>
        public static Language Parse( string str )
        {
            str = str?.Trim() ?? throw new ArgumentNullException( nameof( str ) );

            if ( _languageStrings.TryGetValue( str, out Language language ) )
            {
                return language;
            }

            throw new NoSuchLanguageAlexandriaException( str );
        }

        static readonly IReadOnlyList<ILanguageInfo> _info;
        static readonly IDictionary<string, Language> _languageStrings = new Dictionary<string, Language>( StringComparer.InvariantCultureIgnoreCase );

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
    }
}
