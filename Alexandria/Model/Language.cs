// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Utils;

namespace Alexandria.Model
{
    public enum Language
    {
        [NativeLanguage( "English" )]
        English,

        [NativeLanguage( "فارسی" )]
        Persian,

        [NativeLanguage( "ਪੰਜਾਬੀ" )]
        Punjabi,

        [NativeLanguage( "Català" )]
        Catalan,

        [NativeLanguage( "Čeština" )]
        Czech,

        [NativeLanguage( "Dansk" )]
        Danish,

        [NativeLanguage( "Deutsch" )]
        German,

        [NativeLanguage( "Français" )]
        French,

        [NativeLanguage( "Español" )]
        Spanish,

        [NativeLanguage( "Gaeilge" )]
        Irish,

        [NativeLanguage( "Hrvatski" )]
        Croatian,

        [NativeLanguage( "Íslenska" )]
        Icelandic,

        [NativeLanguage( "Italiano" )]
        Italian,

        [NativeLanguage( "Latviešu valoda" )]
        Latvian,

        [NativeLanguage( "Lietuvių" )]
        Lithuanian,

        [NativeLanguage( "Magyar" )]
        Hungarian,

        [NativeLanguage( "Nederlands" )]
        Dutch,

        [NativeLanguage( "Norsk" )]
        Norwegian,

        [NativeLanguage( "Polski" )]
        Polish,

        [NativeLanguage( "Português brasileiro" )]
        [RenderLanguageName( "Brazilian Portuguese" )]
        BrazilianPortuguese,

        [NativeLanguage( "Română" )]
        Romanian,

        [NativeLanguage( "Shqip" )]
        Albanian,

        [NativeLanguage( "Suomi" )]
        Finnish,

        [NativeLanguage( "Türkçe" )]
        Turkish,

        [NativeLanguage( "Svenska" )]
        Swedish,

        [NativeLanguage( "Wikang Filipino" )]
        Tagalog,

        [NativeLanguage( "Ελληνικά" )]
        Greek,

        [NativeLanguage( "беларуская" )]
        Belarusian,

        [NativeLanguage( "Български език" )]
        Bulgarian,

        [NativeLanguage( "Русский" )]
        Russian,

        [NativeLanguage( "srpski" )]
        Serbian,

        [NativeLanguage( "українська" )]
        Ukranian,

        [NativeLanguage( "עברית" )]
        Hebrew,

        [NativeLanguage( "العربية" )]
        Arabic,

        [NativeLanguage( "한국어" )]
        Korean,

        [NativeLanguage( "日本語" )]
        Japanese,

        [NativeLanguage( "हिन्दी" )]
        Hindi,

        [NativeLanguage( "Bahasa Indonesia" )]
        Indonesian,

        [NativeLanguage( "中文" )]
        Chinese,

        [NativeLanguage( "Lingua latina" )]
        Latin,

        [NativeLanguage( "Gàidhlig" )]
        [RenderLanguageName( "Scottish Gaelic" )]
        ScottishGaelic,

        [NativeLanguage( "Cymraeg" )]
        Welsh,

        [NativeLanguage( "Tiếng Việt" )]
        Vietnamese,

        [NativeLanguage( "Esperanto" )]
        Esperanto,

        [NativeLanguage( "मराठी" )]
        Marathi,

        [NativeLanguage( "ไทย" )]
        Thai,

        [NativeLanguage( "Thermian" )]
        Thermian,

        [NativeLanguage( "tlhIngan-Hol" )]
        Klingon,

        [NativeLanguage( "Quenya" )]
        Elvish,

        [NativeLanguage( "eesti keel" )]
        Estonian,

        [NativeLanguage( "Slovenčina" )]
        Slovak,

        [NativeLanguage( "Bahasa Malaysia" )]
        Malaysian,

        [NativeLanguage( "Kiswahili" )]
        Swahili,

        [NativeLanguage( "af Soomaali" )]
        Somali,

        [NativeLanguage( "Português europeu" )]
        [RenderLanguageName( "European Portuguese" )]
        EuropeanPortuguese,

        [NativeLanguage( "Sprēkō Þiudiskō" )]
        [RenderLanguageName( "??? tbh idek" )]
        OldIcelandicMaybeIdk,

        [NativeLanguage( "Plattdüütsch" )]
        LowGerman,

        [NativeLanguage( "Slovenščina" )]
        Slovene,

        [NativeLanguage( "Afrikaans" )]
        Afrikaans,

        [NativeLanguage( "Sindarin" )]
        Sindarin,

        [NativeLanguage( "Khuzdul" )]
        Khuzdul,

        [NativeLanguage( "বাংলা" )]
        Bengali,

        [NativeLanguage( "Bosanski" )]
        Bosnian,

        [NativeLanguage( "Interlingua" )]
        Interlingua,

        [NativeLanguage( "Lëtzebuergesch" )]
        Luxembourgish,

        [NativeLanguage( "தமிழ்" )]
        Tamil
    }
}
