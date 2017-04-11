using System;
using System.Collections.Generic;
using Alexandria.Model;

namespace Alexandria.Utils
{
	public static class LanguageUtils
	{
		static LanguageUtils()
		{
			foreach ( String value in Enum.GetNames( typeof( Language ) ) )
			{
				Language enumValue = (Language) Enum.Parse( typeof( Language ), value );
				_languageStrings.Add( value, enumValue );

				String nativeLanguage = GetNativeLanguage( enumValue );
				if ( !_languageStrings.ContainsKey( nativeLanguage ) )
				{
					_languageStrings.Add( nativeLanguage, enumValue );
				}
			}
		}

		public static Language Parse( String str )
		{
			return _languageStrings[str.Trim()];
		}

		public static String GetNativeLanguage( Language language )
		{
			NativeLanguageAttribute nativeLanguage =
				typeof( Language ).GetMember( language.ToString() )[0].GetCustomAttributes( typeof( NativeLanguageAttribute ), false )[0] as
				NativeLanguageAttribute;
			return nativeLanguage.Native;
		}

		public static String GetDisplayName( Language language )
		{
			RenderLanguageNameAttribute renderName =
				typeof( Language ).GetMember( language.ToString() )[0].GetCustomAttributes( typeof( RenderLanguageNameAttribute ), false )[0] as
				RenderLanguageNameAttribute;
			return renderName.Name;
		}

		static readonly IDictionary<String, Language> _languageStrings = new Dictionary<String, Language>();
	}
}
