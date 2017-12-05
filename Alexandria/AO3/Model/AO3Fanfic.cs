// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.AO3.Data;
using Alexandria.Model;
using Alexandria.RequestHandles;
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
            { "rating tags", ( fanfic, value ) => fanfic.Rating = ParseMaturityRating( value ) },
            { "warning tags", ( fanfic, value ) => fanfic.ContentWarnings = ParseContentWarning( value ) },
            { "relationship tags", ( fanfic, value ) => fanfic.Ships = ParseTagsFromDlTable<IShip, IShipRequestHandle>( fanfic, value, ShipRequestHandleCreator ) },
            { "character tags", ( fanfic, value ) => fanfic.Characters = ParseTagsFromDlTable<ICharacter, ICharacterRequestHandle>( fanfic, value, CharacterRequestHandleCreator ) },
            { "freeform tags", ( fanfic, value ) => fanfic.Tags = ParseTagsFromDlTable<ITag, ITagRequestHandle>( fanfic, value, TagRequestHandleCreator ) },
            { "language", ( fanfic, value ) => fanfic.Language = Languages.Parse( value.InnerText.Trim() ) },
            { "series", ( fanfic, value ) => fanfic.SeriesInfo = ParseSeriesEntries( fanfic.Source, value ) },
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
        public IReadOnlyList<ISeriesEntry> SeriesInfo { get; private set; }

        /// <inheritdoc />
        public IChapterInfo ChapterInfo { get; private set; }

        /// <inheritdoc />
        public LanguageInfo Language { get; private set; }

        /// <inheritdoc />
        public string Summary { get; private set; }

        /// <inheritdoc />
        public string AuthorsNote { get; private set; }

        /// <inheritdoc />
        public string Footnote { get; private set; }

        /// <inheritdoc />
        public string Text { get; private set; }

        /// <summary>
        /// Parses an HTML page into an instance of an <seealso cref="AO3Fanfic"/>.
        /// </summary>
        /// <param name="source">The source that the HTML page came from, which is then passed
        /// along to any nested request handles for them to parse data with as well.</param>
        /// <param name="document">The document that came from the website itself.</param>
        /// <returns>An instance of <seealso cref="AO3Fanfic"/> that was parsed and configured using
        /// the information provided.</returns>
        public static AO3Fanfic Parse( AO3Source source, Document document )
        {
            HtmlNode workMetaGroup = document.Html.SelectSingleNode( "//dl[@class='work meta group']" );

            AO3Fanfic parsed = new AO3Fanfic( source, document.Url )
            {
                ChapterInfo = AO3ChapterInfo.Parse( source, document, workMetaGroup ),
                Footnote = ParseFootnote( document.Html ),
                Text = ParseFanficText( document.Html )
            };

            ParseWorkMetaGroup( parsed, workMetaGroup );
            ParsePreface( parsed, document.Html );

            return parsed;
        }

        static void ParseWorkMetaGroup( AO3Fanfic parsed, HtmlNode workMetaGroup )
        {
            ParseDlTable( parsed, workMetaGroup, _workMetaGroupMutators, DlFieldSource.DtClass );
            if ( parsed.SeriesInfo == null )
            {
                parsed.SeriesInfo = new List<ISeriesEntry>( 0 );
            }
        }

        static MaturityRating ParseMaturityRating( HtmlNode value )
        {
            string ratingStr = value?.InnerText?.Trim();
            if ( string.IsNullOrEmpty( ratingStr ) )
            {
                return MaturityRating.NotRated;
            }

            return AO3Enums.MaturityRatings.First( def => def.Matches( ratingStr ) ).EnumValue;
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

                ContentWarnings flag = AO3Enums.ContentWarnings.First( def => def.Matches( tag ) ).EnumValue;
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

        static IReadOnlyList<ISeriesEntry> ParseSeriesEntries( AO3Source source, HtmlNode value )
        {
            if ( value == null )
            {
                return new List<ISeriesEntry>( 0 );
            }

            List<ISeriesEntry> series = new List<ISeriesEntry>();
            foreach ( HtmlNode seriesSpan in value.Elements( "span" ) )
            {
                string classAttribute = seriesSpan?.GetAttributeValue( "class", string.Empty );
                if ( !string.Equals( "series", classAttribute, StringComparison.InvariantCultureIgnoreCase ) )
                {
                    continue;
                }

                series.Add( AO3SeriesEntry.Parse( source, seriesSpan ) );
            }

            return series;
        }

        static void ParseStatsTable( AO3Fanfic fanfic, HtmlNode statsTable )
        {
            HtmlNode statsDl = statsTable.Element( "dl" );
            ParseDlTable( fanfic, statsDl, _statsMutators, DlFieldSource.DtClass );
        }

        static void ParsePreface( AO3Fanfic fanfic, HtmlNode html )
        {
            HtmlNode prefaceGroup = html.SelectSingleNode( "//div[@class='preface group']" );

            fanfic.Title = GetReadableInnerText( prefaceGroup.SelectSingleNode( "h2[@class='title heading']" ) );
            fanfic.Authors = ParseAuthorsList( fanfic.Source, prefaceGroup.SelectSingleNode( "h3[@class = 'byline heading']" ) );

            HtmlNode summaryBlockquote = prefaceGroup.SelectSingleNode( ".//div[@class='summary module']/blockquote" );
            if ( summaryBlockquote != null )
            {
                fanfic.Summary = GetReadableInnerText( summaryBlockquote );
            }

            HtmlNode notesBlockquote = prefaceGroup.SelectSingleNode( ".//div[@class='notes module']/blockquote" );
            if ( notesBlockquote != null )
            {
                fanfic.AuthorsNote = GetReadableInnerText( notesBlockquote );
            }
        }

        static string ParseFootnote( HtmlNode html )
        {
            HtmlNode workEndnotesBlockquote = html.SelectSingleNode( "//div[@id='work_endnotes']/blockquote" );
            return GetReadableInnerText( workEndnotesBlockquote );
        }

        static string ParseFanficText( HtmlNode html )
        {
            HtmlNode userstuffModuleDiv = html.SelectSingleNode( "//div[@class='userstuff module']" ) ??
                                          html.SelectSingleNode( "//div[@id='chapters']/div[contains( @class, 'userstuff' )]" );
            userstuffModuleDiv.Element( "h3" )?.Remove();
            return GetReadableInnerText( userstuffModuleDiv );
        }
    }
}
