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

			public String DisplayName { get; }

			public String NativeName { get; }
		}

		static LanguageUtils()
		{
			List<ILanguageInfo> infos = new List<ILanguageInfo>();
			foreach ( String value in Enum.GetNames( typeof( Language ) ) )
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
			return _info[(Int32) language];
		}

		public static Language Parse( String str )
		{
			if ( String.IsNullOrWhiteSpace( str ) )
			{
				throw new ArgumentNullException( nameof( str ) );
			}
			str = str.Trim();

			if ( _languageStrings.TryGetValue( str, out Language language ) )
			{
				return language;
			}

			throw new ArgumentException( $"Could not parse to a {nameof( Language ) } value." );
		}

		static readonly IReadOnlyList<ILanguageInfo> _info;
		static readonly IDictionary<String, Language> _languageStrings = new Dictionary<String, Language>();
	}
}
