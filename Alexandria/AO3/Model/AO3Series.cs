// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.AO3.RequestHandles;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    /// <summary>
    /// A concrete class for series from AO3.
    /// </summary>
    internal sealed class AO3Series : AO3ModelBase<AO3Series>, ISeries
    {
        static readonly IReadOnlyDictionary<string, TableFieldMutator> _seriesMetaGroupMutators = new Dictionary<string, TableFieldMutator>
        {
            { "Creator", ( source, series, value ) => series.Authors = ParseAuthorsList( series.Source, value ) },
            { "Series Begun", ( source, series, value ) => series.DateStarted = DateTime.Parse( value.InnerText ) },
            { "Series Updated", ( source, series, value ) => series._dateLastUpdated = DateTime.Parse( value.InnerText ) },
            { "Stats", ParseStatsTable }
        };

        static readonly IReadOnlyDictionary<string, TableFieldMutator> _statsMutators = new Dictionary<string, TableFieldMutator>
        {
            { "Complete", ( source, series, value ) => series.IsCompleted = ParseCompletedDd( value ) }
        };

        DateTime? _dateLastUpdated;

        AO3Series( AO3Source source, Uri url )
            : base( source, url )
        {
        }

        /// <inheritdoc />
        public IReadOnlyList<IAuthorRequestHandle> Authors { get; private set; }

        /// <inheritdoc />
        public DateTime DateStarted { get; private set; }

        /// <inheritdoc />
        public DateTime DateLastUpdated => _dateLastUpdated.GetValueOrDefault( DateStarted );

        /// <inheritdoc />
        public bool IsCompleted { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<IFanficRequestHandle> Fanfics { get; private set; }

        /// <summary>
        /// Parses an HTML page into an instance of an <seealso cref="AO3Series"/>.
        /// </summary>
        /// <param name="source">The source that the HTML page came from, which is then passed
        /// along to any nested request handles for them to parse data with as well.</param>
        /// <param name="document">The document that came from the website itself.</param>
        /// <returns>An instance of <seealso cref="AO3Series"/> that was parsed and configured using
        /// the information provided.</returns>
        public static AO3Series Parse( AO3Source source, Document document )
        {
            HtmlNode mainDiv = document.Html.SelectSingleNode( "//div[@id='main']" );

            AO3Series parsed = new AO3Series( source, document.Url )
            {
                Fanfics = ParseSeriesFanfics( source, mainDiv )
            };

            HtmlNode seriesMetaGroupDl = mainDiv.SelectSingleNode( ".//dl[@class='series meta group']" );
            ParseDlTable( source, parsed, seriesMetaGroupDl, _seriesMetaGroupMutators, DlFieldSource.DtText );

            return parsed;
        }

        static void ParseStatsTable( AO3Source source, AO3Series series, HtmlNode statsTable )
        {
            HtmlNode statsDl = statsTable.Element( "dl" );
            ParseDlTable( source, series, statsDl, _statsMutators, DlFieldSource.DtText );
        }

        static IReadOnlyList<IFanficRequestHandle> ParseSeriesFanfics( AO3Source source, HtmlNode mainDiv )
        {
            HtmlNode seriesWorkUl = mainDiv.SelectSingleNode( ".//ul[contains(@class, 'series work' )]" );
            return AO3FanficRequestHandle.ParseFanficLiList( source, seriesWorkUl );
        }

        static bool ParseCompletedDd( HtmlNode completedDd )
        {
            return ( completedDd?.InnerText.Equals( "Yes", StringComparison.InvariantCultureIgnoreCase ) == true );
        }
    }
}
