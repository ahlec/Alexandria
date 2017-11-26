// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

namespace Alexandria.Model
{
    /// <summary>
    /// An enumeration of all of the languages that Alexandria is capable of supporting. This enum is
    /// determined by the websites themselves and this enum is capable of changing somewhat frequently
    /// as the websites add support or deprecate support for certain languages.
    /// </summary>
    public enum Language
    {
        /// <summary>
        /// The English language.
        /// https://en.wikipedia.org/wiki/English_language
        /// </summary>
        English,

        /// <summary>
        /// The Persian language.
        /// https://en.wikipedia.org/wiki/Persian_language
        /// </summary>
        Persian,

        /// <summary>
        /// The Punjabi language.
        /// https://en.wikipedia.org/wiki/Punjabi_language
        /// </summary>
        Punjabi,

        /// <summary>
        /// The Catalan language.
        /// https://en.wikipedia.org/wiki/Catalan_language
        /// </summary>
        Catalan,

        /// <summary>
        /// The Czech language.
        /// https://en.wikipedia.org/wiki/Czech_language
        /// </summary>
        Czech,

        /// <summary>
        /// The Danish language.
        /// https://en.wikipedia.org/wiki/Danish_language
        /// </summary>
        Danish,

        /// <summary>
        /// The German language.
        /// https://en.wikipedia.org/wiki/German_language
        /// </summary>
        German,

        /// <summary>
        /// The French language.
        /// https://en.wikipedia.org/wiki/French_language
        /// </summary>
        French,

        /// <summary>
        /// The Spanish language.
        /// https://en.wikipedia.org/wiki/Spanish_language
        /// </summary>
        Spanish,

        /// <summary>
        /// The Irish language.
        /// https://en.wikipedia.org/wiki/Irish_language
        /// </summary>
        Irish,

        /// <summary>
        /// The Croatian language.
        /// https://en.wikipedia.org/wiki/Croatian_language
        /// </summary>
        Croatian,

        /// <summary>
        /// The Icelandic language.
        /// https://en.wikipedia.org/wiki/Icelandic_language
        /// </summary>
        Icelandic,

        /// <summary>
        /// The Italian language.
        /// https://en.wikipedia.org/wiki/Italian_language
        /// </summary>
        Italian,

        /// <summary>
        /// The Latvian language.
        /// https://en.wikipedia.org/wiki/Latvian_language
        /// </summary>
        Latvian,

        /// <summary>
        /// The Lithuanian language.
        /// https://en.wikipedia.org/wiki/Lithuanian_language
        /// </summary>
        Lithuanian,

        /// <summary>
        /// The Hungarian language.
        /// https://en.wikipedia.org/wiki/Hungarian_language
        /// </summary>
        Hungarian,

        /// <summary>
        /// The Dutch language.
        /// https://en.wikipedia.org/wiki/Dutch_language
        /// </summary>
        Dutch,

        /// <summary>
        /// The Norwegian language.
        /// https://en.wikipedia.org/wiki/Norwegian_language
        /// </summary>
        Norwegian,

        /// <summary>
        /// The Polish language.
        /// https://en.wikipedia.org/wiki/Polish_language
        /// </summary>
        Polish,

        /// <summary>
        /// The Brazilian dialect of the Portuguese language.
        /// https://en.wikipedia.org/wiki/Brazilian_Portuguese
        /// </summary>
        BrazilianPortuguese,

        /// <summary>
        /// The Romanian language.
        /// https://en.wikipedia.org/wiki/Romanian_language
        /// </summary>
        Romanian,

        /// <summary>
        /// The Albanian language.
        /// https://en.wikipedia.org/wiki/Albanian_language
        /// </summary>
        Albanian,

        /// <summary>
        /// The Finnish language.
        /// https://en.wikipedia.org/wiki/Finnish_language
        /// </summary>
        Finnish,

        /// <summary>
        /// The Turkish language.
        /// https://en.wikipedia.org/wiki/Turkish_language
        /// </summary>
        Turkish,

        /// <summary>
        /// The Swedish language.
        /// https://en.wikipedia.org/wiki/Swedish_language
        /// </summary>
        Swedish,

        /// <summary>
        /// The Tagalog language.
        /// https://en.wikipedia.org/wiki/Tagalog_language
        /// </summary>
        Tagalog,

        /// <summary>
        /// The Greek language.
        /// https://en.wikipedia.org/wiki/Greek_language
        /// </summary>
        Greek,

        /// <summary>
        /// The Belarusian language.
        /// https://en.wikipedia.org/wiki/Belarusian_language
        /// </summary>
        Belarusian,

        /// <summary>
        /// The Bulgarian language.
        /// https://en.wikipedia.org/wiki/Bulgarian_language
        /// </summary>
        Bulgarian,

        /// <summary>
        /// The Russian language.
        /// https://en.wikipedia.org/wiki/Russian_language
        /// </summary>
        Russian,

        /// <summary>
        /// The Serbian language.
        /// https://en.wikipedia.org/wiki/Serbian_language
        /// </summary>
        Serbian,

        /// <summary>
        /// The Ukranian language.
        /// https://en.wikipedia.org/wiki/Ukrainian_language
        /// </summary>
        Ukranian,

        /// <summary>
        /// The Hebrew language.
        /// https://en.wikipedia.org/wiki/Hebrew_language
        /// </summary>
        Hebrew,

        /// <summary>
        /// The Arabic language.
        /// https://en.wikipedia.org/wiki/Arabic
        /// </summary>
        Arabic,

        /// <summary>
        /// The Korean language.
        /// https://en.wikipedia.org/wiki/Korean_language
        /// </summary>
        Korean,

        /// <summary>
        /// The Japanese language.
        /// https://en.wikipedia.org/wiki/Japanese_language
        /// </summary>
        Japanese,

        /// <summary>
        /// The Hindi language.
        /// https://en.wikipedia.org/wiki/Hindi
        /// </summary>
        Hindi,

        /// <summary>
        /// The Indonesian language.
        /// https://en.wikipedia.org/wiki/Indonesian_language
        /// </summary>
        Indonesian,

        /// <summary>
        /// The Chinese language (all dialects).
        /// https://en.wikipedia.org/wiki/Chinese_language
        /// </summary>
        Chinese,

        /// <summary>
        /// Latin.
        /// https://en.wikipedia.org/wiki/Latin
        /// </summary>
        Latin,

        /// <summary>
        /// The Gaelic language.
        /// https://en.wikipedia.org/wiki/Scottish_Gaelic
        /// </summary>
        ScottishGaelic,

        /// <summary>
        /// The Welsh language.
        /// https://en.wikipedia.org/wiki/Welsh_language
        /// </summary>
        Welsh,

        /// <summary>
        /// The Vietnamese language.
        /// https://en.wikipedia.org/wiki/Vietnamese_language
        /// </summary>
        Vietnamese,

        /// <summary>
        /// Esperanto.
        /// https://en.wikipedia.org/wiki/Esperanto
        /// </summary>
        Esperanto,

        /// <summary>
        /// The Marathi language.
        /// https://en.wikipedia.org/wiki/Marathi_language
        /// </summary>
        Marathi,

        /// <summary>
        /// The Thai language.
        /// https://en.wikipedia.org/wiki/Thai_language
        /// </summary>
        Thai,

        /// <summary>
        /// The constructed language "Thermian" from the sci-fi comedy movie Galaxy Quest.
        /// https://en.wikipedia.org/wiki/Galaxy_Quest
        /// </summary>
        Thermian,

        /// <summary>
        /// The constructed language "Klingon" from the sci-fi universe Star Trek.
        /// https://en.wikipedia.org/wiki/Klingon_language
        /// </summary>
        Klingon,

        /// <summary>
        /// The constructed language "Quenya" from the fantasy universe of J. R. R. Tolkien.
        /// https://en.wikipedia.org/wiki/Quenya
        /// </summary>
        Quenya,

        /// <summary>
        /// The Estonian language.
        /// https://en.wikipedia.org/wiki/Estonian_language
        /// </summary>
        Estonian,

        /// <summary>
        /// The Slovak language.
        /// https://en.wikipedia.org/wiki/Slovak_language
        /// </summary>
        Slovak,

        /// <summary>
        /// The Malaysian language.
        /// https://en.wikipedia.org/wiki/Malaysian_language
        /// </summary>
        Malaysian,

        /// <summary>
        /// The Swahili language.
        /// https://en.wikipedia.org/wiki/Swahili_language
        /// </summary>
        Swahili,

        /// <summary>
        /// The Somali language.
        /// https://en.wikipedia.org/wiki/Somali_language
        /// </summary>
        Somali,

        /// <summary>
        /// The European dialect of the Portuguese language.
        /// https://en.wikipedia.org/wiki/European_Portuguese
        /// </summary>
        EuropeanPortuguese,

        /// <summary>
        /// The reconstructed proto-language of the Germanic language family.
        /// https://en.wikipedia.org/wiki/Proto-Germanic_language
        /// </summary>
        ProtoGermanic,

        /// <summary>
        /// The Low German language.
        /// https://en.wikipedia.org/wiki/Low_German
        /// </summary>
        LowGerman,

        /// <summary>
        /// The Slovene language.
        /// https://en.wikipedia.org/wiki/Slovene_language
        /// </summary>
        Slovene,

        /// <summary>
        /// The Afrikaans language.
        /// https://en.wikipedia.org/wiki/Afrikaans
        /// </summary>
        Afrikaans,

        /// <summary>
        /// The constructed language "Sindarin" from the fantasy universe of J. R. R. Tolkien.
        /// https://en.wikipedia.org/wiki/Sindarin
        /// </summary>
        Sindarin,

        /// <summary>
        /// The constructed language "Khuzdul" from the fantasy universe of J. R. R. Tolkien.
        /// https://en.wikipedia.org/wiki/Khuzdul
        /// </summary>
        Khuzdul,

        /// <summary>
        /// The Bengali language.
        /// https://en.wikipedia.org/wiki/Bengali_language
        /// </summary>
        Bengali,

        /// <summary>
        /// The Bosnian language.
        /// https://en.wikipedia.org/wiki/Bosnian_language
        /// </summary>
        Bosnian,

        /// <summary>
        /// The constructed language "Interlingua"
        /// https://en.wikipedia.org/wiki/Interlingua
        /// </summary>
        Interlingua,

        /// <summary>
        /// The Luxembourgish language.
        /// https://en.wikipedia.org/wiki/Luxembourgish
        /// </summary>
        Luxembourgish,

        /// <summary>
        /// The Tamil language.
        /// https://en.wikipedia.org/wiki/Tamil_language
        /// </summary>
        Tamil
    }
}
