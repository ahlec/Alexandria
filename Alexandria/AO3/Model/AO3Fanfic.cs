// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.AO3.Utils;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    /// <summary>
    /// A concrete class for fanfics from AO3.
    /// </summary>
    internal sealed class AO3Fanfic : AO3ModelBase<AO3Fanfic>, IFanfic
    {
        static readonly IReadOnlyDictionary<string, TableFieldMutator> _workMetaGroupMutators = new Dictionary<string, TableFieldMutator>
        {
            { "rating tags", ( fanfic, value ) => fanfic.Rating = AO3MaturityRatingUtils.Parse( value.InnerText.Trim() ) },
            { "warning tags", ( fanfic, value ) => fanfic.ContentWarnings = ParseContentWarning( value ) },
            { "relationship tags", ( fanfic, value ) => fanfic.Ships = ParseTagsFromDlTable<IShip, IShipRequestHandle>( fanfic, value, ShipRequestHandleCreator ) },
            { "character tags", ( fanfic, value ) => fanfic.Characters = ParseTagsFromDlTable<ICharacter, ICharacterRequestHandle>( fanfic, value, CharacterRequestHandleCreator ) },
            { "freeform tags", ( fanfic, value ) => fanfic.Tags = ParseTagsFromDlTable<ITag, ITagRequestHandle>( fanfic, value, TagRequestHandleCreator ) },
            { "language", ( fanfic, value ) => fanfic.Language = LanguageUtils.Parse( value.ReadableInnerText().Trim() ) },
            { "series", ( fanfic, value ) => fanfic.SeriesInfo = AO3SeriesEntry.Parse( fanfic.Source, value ) },
            { "stats", ParseStatsTable }
        };

        static readonly IReadOnlyDictionary<string, TableFieldMutator> _statsMutators = new Dictionary<string, TableFieldMutator>
        {
            { "published", ( fanfic, value ) => fanfic.DateStarted = DateTime.Parse( value.InnerText ) },
            { "status", ( fanfic, value ) => fanfic._dateLastUpdated = DateTime.Parse( value.InnerText ) },
            { "words", ( fanfic, value ) => fanfic.NumberWords = int.Parse( value.InnerText ) },
            { "comments", ( fanfic, value ) => fanfic.NumberComments = int.Parse( value.InnerText ) },
            { "kudos", ( fanfic, value ) => fanfic.NumberLikes = int.Parse( value.InnerText ) }
        };

        static readonly IReadOnlyDictionary<string, ContentWarnings> _ao3ContentWarningLookup = new Dictionary<string, ContentWarnings>( StringComparer.InvariantCultureIgnoreCase )
        {
            { "no archive warnings apply", ContentWarnings.None },
            { "creator chose not to use archive warnings", ContentWarnings.Undetermined },
            { "graphic depictions of violence", ContentWarnings.Violence },
            { "major character death", ContentWarnings.MajorCharacterDeath },
            { "rape/non-con", ContentWarnings.Rape },
            { "underage", ContentWarnings.Underage }
        };

        DateTime? _dateLastUpdated;

        AO3Fanfic( AO3Source source, Uri url )
            : base( source, url )
        {
        }

        /// <inheritdoc />
        public string Title { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<IAuthorRequestHandle> Authors { get; private set; }

        /// <inheritdoc />
        public MaturityRating Rating { get; private set; }

        /// <inheritdoc />
        public ContentWarnings ContentWarnings { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<IShipRequestHandle> Ships { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<ICharacterRequestHandle> Characters { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<ITagRequestHandle> Tags { get; private set; }

        /// <inheritdoc />
        public int NumberWords { get; private set; }

        /// <inheritdoc />
        public DateTime DateStarted { get; private set; }

        /// <inheritdoc />
        public DateTime DateLastUpdated => _dateLastUpdated.GetValueOrDefault( DateStarted );

        /// <inheritdoc />
        public int NumberLikes { get; private set; }

        /// <inheritdoc />
        public int NumberComments { get; private set; }

        /// <inheritdoc />
        public ISeriesEntry SeriesInfo { get; private set; }

        /// <inheritdoc />
        public IChapterInfo ChapterInfo { get; private set; }

        /// <inheritdoc />
        public Language Language { get; private set; }

        /// <inheritdoc />
        public string Summary { get; private set; }

        /// <inheritdoc />
        public string AuthorsNote { get; private set; }

        /// <inheritdoc />
        public string Footnote { get; private set; }

        /// <inheritdoc />
        public string Text { get; private set; }

        public static AO3Fanfic Parse( AO3Source source, HtmlCacheableDocument document )
        {
            AO3Fanfic parsed = new AO3Fanfic( source, document.Url );

            HtmlNode workMetaGroup = document.Html.SelectSingleNode( "//dl[@class='work meta group']" );
            ParseDlTable( parsed, workMetaGroup, _workMetaGroupMutators, DlFieldSource.DtClass );

            parsed.ChapterInfo = AO3ChapterInfo.Parse( source, document, workMetaGroup );

            ParsePreface( parsed, document.Html );

            HtmlNode workEndnotesBlockquote = document.Html.SelectSingleNode( "//div[@id='work_endnotes']/blockquote" );
            if ( workEndnotesBlockquote != null )
            {
                parsed.Footnote = workEndnotesBlockquote.ReadableInnerText().Trim();
            }

            HtmlNode userstuffModuleDiv = document.Html.SelectSingleNode( "//div[@class='userstuff module']" ) ??
                                          document.Html.SelectSingleNode( "//div[@id='chapters']/div[contains( @class, 'userstuff' )]" );
            userstuffModuleDiv.Element( "h3" )?.Remove();
            parsed.Text = userstuffModuleDiv.ReadableInnerText().Trim();

            return parsed;
        }

        static ContentWarnings ParseContentWarning( HtmlNode value )
        {
            HtmlNode ul = value?.Element( "ul" );
            if ( ul == null )
            {
                return ContentWarnings.None;
            }

            ContentWarnings parsed = ContentWarnings.None;

            foreach ( HtmlNode li in ul.Elements( "li" ) )
            {
                string tag = li.FirstChild.InnerText;

                if ( !_ao3ContentWarningLookup.TryGetValue( tag, out ContentWarnings flag ) )
                {
                    throw new ArgumentException( $"Unable to parse the built-in AO3 content warning tag for '{tag}'" );
                }

                parsed |= flag;
            }

            return parsed;
        }

        static IReadOnlyList<TRequestHandle> ParseTagsFromDlTable<TModel, TRequestHandle>( AO3Fanfic fanfic, HtmlNode value, RequestHandleCreatorFunc<TModel, TRequestHandle> requestHandleCreator )
            where TModel : IRequestable
            where TRequestHandle : IRequestHandle<TModel>
        {
            HtmlNode ul = value?.Element( "ul" );
            return ParseTagsUl( fanfic, ul, requestHandleCreator );
        }

        static void ParseStatsTable( AO3Fanfic fanfic, HtmlNode statsTable )
        {
            HtmlNode statsDl = statsTable.Element( "dl" );
            ParseDlTable( fanfic, statsDl, _statsMutators, DlFieldSource.DtClass );
        }

        static void ParsePreface( AO3Fanfic fanfic, HtmlNode html )
        {
            HtmlNode prefaceGroup = html.SelectSingleNode( "//div[@class='preface group']" );

            fanfic.Title = prefaceGroup.SelectSingleNode( "h2[@class='title heading']" ).ReadableInnerText().Trim();
            fanfic.Authors = ParseAuthorsList( fanfic.Source, prefaceGroup.SelectSingleNode( "h3[@class = 'byline heading']" ) );

            HtmlNode summaryBlockquote = prefaceGroup.SelectSingleNode( ".//div[@class='summary module']/blockquote" );
            if ( summaryBlockquote != null )
            {
                fanfic.Summary = summaryBlockquote.ReadableInnerText().Trim();
            }

            HtmlNode notesBlockquote = prefaceGroup.SelectSingleNode( ".//div[@class='notes module']/blockquote" );
            if ( notesBlockquote != null )
            {
                fanfic.AuthorsNote = notesBlockquote.ReadableInnerText().Trim();
            }
        }
    }
}
