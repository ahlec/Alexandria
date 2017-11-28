// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.Exceptions.Input;

namespace Alexandria.Model
{
    /// <summary>
    /// A set of utility functions for interacting with <see cref="Language"/> and <see cref="LanguageInfo"/>.
    /// </summary>
    public static class LanguageUtils
    {
        static readonly IDictionary<string, LanguageInfo> _languageStrings = new Dictionary<string, LanguageInfo>( StringComparer.InvariantCultureIgnoreCase );

        static LanguageUtils()
        {
            HashSet<string> uniqueNames = new HashSet<string>( StringComparer.InvariantCultureIgnoreCase );
            foreach ( LanguageInfo info in AllLanguages )
            {
                uniqueNames.Clear();
                info.AddUniqueNames( uniqueNames );

                foreach ( string name in uniqueNames )
                {
                    _languageStrings.Add( name, info );
                }
            }
        }

        /// <summary>
        /// Gets a list of all defined languages in Alexandria to iterate over, along with information about
        /// each.
        /// </summary>
        public static IReadOnlyList<LanguageInfo> AllLanguages { get; } = new List<LanguageInfo>
        {
            new LanguageInfo( Language.English,              1, "English",              "English" ),
            new LanguageInfo( Language.Persian,              2, "Persian",              "فارسی" ),
            new LanguageInfo( Language.Punjabi,              3, "Punjabi",              "ਪੰਜਾਬੀ" ),
            new LanguageInfo( Language.Catalan,              4, "Catalan",              "Català" ),
            new LanguageInfo( Language.Czech,                5, "Czech",                "Čeština" ),
            new LanguageInfo( Language.Danish,               6, "Danish",               "Dansk" ),
            new LanguageInfo( Language.German,               7, "German",               "Deutsch" ),
            new LanguageInfo( Language.French,               8, "French",               "Français" ),
            new LanguageInfo( Language.Spanish,              9, "Spanish",              "Español" ),
            new LanguageInfo( Language.Irish,               10, "Irish",                "Gaeilge" ),
            new LanguageInfo( Language.Croatian,            11, "Croatian",             "Hrvatski" ),
            new LanguageInfo( Language.Icelandic,           12, "Icelandic",            "Íslenska" ),
            new LanguageInfo( Language.Italian,             13, "Italian",              "Italiano" ),
            new LanguageInfo( Language.Latvian,             14, "Latvian",              "Latviešu valoda" ),
            new LanguageInfo( Language.Lithuanian,          15, "Lithuanian",           "Lietuvių" ),
            new LanguageInfo( Language.Hungarian,           16, "Hungarian",            "Magyar" ),
            new LanguageInfo( Language.Dutch,               17, "Dutch",                "Nederlands" ),
            new LanguageInfo( Language.Norwegian,           18, "Norwegian",            "Norsk" ),
            new LanguageInfo( Language.Polish,              19, "Polish",               "Polski" ),
            new LanguageInfo( Language.BrazilianPortuguese, 20, "Brazilian Portuguese", "Português brasileiro" ),
            new LanguageInfo( Language.Romanian,            21, "Romanian",             "Română" ),
            new LanguageInfo( Language.Albanian,            22, "Albanian",             "Shqip" ),
            new LanguageInfo( Language.Finnish,             23, "Finnish",              "Suomi" ),
            new LanguageInfo( Language.Turkish,             24, "Turkish",              "Türkçe" ),
            new LanguageInfo( Language.Swedish,             25, "Swedish",              "Svenska" ),
            new LanguageInfo( Language.Tagalog,             26, "Tagalog",              "Wikang Filipino" ),
            new LanguageInfo( Language.Greek,               27, "Greek",                "Ελληνικά" ),
            new LanguageInfo( Language.Belarusian,          28, "Belarusian",           "беларуская" ),
            new LanguageInfo( Language.Bulgarian,           29, "Bulgarian",            "Български език" ),
            new LanguageInfo( Language.Russian,             30, "Russian",              "Русский" ),
            new LanguageInfo( Language.Serbian,             31, "Serbian",              "srpski" ),
            new LanguageInfo( Language.Ukranian,            32, "Ukranian",             "українська" ),
            new LanguageInfo( Language.Hebrew,              33, "Hebrew",               "עברית" ),
            new LanguageInfo( Language.Arabic,              34, "Arabic",               "العربية" ),
            new LanguageInfo( Language.Korean,              35, "Korean",               "한국어" ),
            new LanguageInfo( Language.Japanese,            36, "Japanese",             "日本語" ),
            new LanguageInfo( Language.Hindi,               37, "Hindi",                "हिन्दी" ),
            new LanguageInfo( Language.Indonesian,          38, "Indonesian",           "Bahasa Indonesia" ),
            new LanguageInfo( Language.Chinese,             40, "Chinese",              "中文" ),
            new LanguageInfo( Language.Latin,               41, "Latin",                "Lingua latina" ),
            new LanguageInfo( Language.ScottishGaelic,      42, "Scottish Gaelic",      "Gàidhlig" ),
            new LanguageInfo( Language.Welsh,               43, "Welsh",                "Cymraeg" ),
            new LanguageInfo( Language.Vietnamese,          44, "Vietnamese",           "Tiếng Việt" ),
            new LanguageInfo( Language.Esperanto,           45, "Esperanto",            "Esperanto" ),
            new LanguageInfo( Language.Marathi,             46, "Marathi",              "मराठी" ),
            new LanguageInfo( Language.Thai,                47, "Thai",                 "ไทย" ),
            new LanguageInfo( Language.Thermian,            48, "Thermian",             "Thermian" ),
            new LanguageInfo( Language.Klingon,             49, "Klingon",              "tlhIngan-Hol" ),
            new LanguageInfo( Language.Quenya,              50, "Quenya",               "Quenya" ),
            new LanguageInfo( Language.Estonian,            51, "Estonian",             "eesti keel" ),
            new LanguageInfo( Language.Slovak,              53, "Slovak",               "Slovenčina" ),
            new LanguageInfo( Language.Malaysian,           56, "Malaysian",            "Bahasa Malaysia" ),
            new LanguageInfo( Language.Swahili,             58, "Swahili",              "Kiswahili" ),
            new LanguageInfo( Language.Somali,              64, "Somali",               "af Soomaali" ),
            new LanguageInfo( Language.EuropeanPortuguese,  65, "European Portuguese",  "Português europeu" ),
            new LanguageInfo( Language.ProtoGermanic,       69, "Proto-Germanic",       "Sprēkō Þiudiskō" ),
            new LanguageInfo( Language.LowGerman,           70, "Low German",           "Plattdüütsch" ),
            new LanguageInfo( Language.Slovene,             72, "Slovene",              "Slovenščina" ),
            new LanguageInfo( Language.Afrikaans,           73, "Afrikaans",            "Afrikaans" ),
            new LanguageInfo( Language.Sindarin,            75, "Sindarin",             "Sindarin" ),
            new LanguageInfo( Language.Khuzdul,             76, "Khuzdul",              "Khuzdul" ),
            new LanguageInfo( Language.Bengali,             79, "Bengali",              "বাংলা" ),
            new LanguageInfo( Language.Bosnian,             82, "Bosnian",              "Bosanski" ),
            new LanguageInfo( Language.Interlingua,         84, "Interlingua",          "Interlingua" ),
            new LanguageInfo( Language.Luxembourgish,       87, "Luxembourgish",        "Lëtzebuergesch" ),
            new LanguageInfo( Language.Tamil,               90, "Tamil",                "தமிழ்" )
        };

        /// <summary>
        /// Gets the instance of the <see cref="LanguageInfo"/> for the provided language.
        /// </summary>
        /// <param name="language">The language to retrieve the information about.</param>
        /// <returns>The reference to the instance of the language information. There is only one instance
        /// of this data in use, so duplicate calls to this function will return the same reference.</returns>
        public static LanguageInfo GetInfo( Language language )
        {
            return AllLanguages.First( info => info.Language == language );
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
        public static LanguageInfo Parse( string str )
        {
            str = str?.Trim() ?? throw new ArgumentNullException( nameof( str ) );

            if ( _languageStrings.TryGetValue( str, out LanguageInfo info ) )
            {
                return info;
            }

            throw new NoSuchLanguageAlexandriaException( str );
        }
    }
}
