// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.AO3.RequestHandles;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    internal sealed class AO3Series : AO3ModelBase<AO3Series>, ISeries
    {
        static readonly IReadOnlyDictionary<string, TableFieldMutator> _seriesMetaGroupMutators = new Dictionary<string, TableFieldMutator>
        {
            { "Creator", ( series, value ) => series.Authors = ParseAuthorsList( series.Source, value ) },
            { "Series Begun", ( series, value ) => series.DateStarted = DateTime.Parse( value.InnerText ) },
            { "Series Updated", ( series, value ) => series._dateLastUpdated = DateTime.Parse( value.InnerText ) },
            { "Stats", ParseStatsTable }
        };

        static readonly IReadOnlyDictionary<string, TableFieldMutator> _statsMutators = new Dictionary<string, TableFieldMutator>
        {
            { "Complete", ( series, value ) => series.IsCompleted = ParseCompletedDd( value ) }
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

        public static AO3Series Parse( AO3Source source, HtmlCacheableDocument document )
        {
            HtmlNode mainDiv = document.Html.SelectSingleNode( "//div[@id='main']" );

            AO3Series parsed = new AO3Series( source, document.Url )
            {
                Fanfics = ParseSeriesFanfics( source, mainDiv )
            };

            HtmlNode seriesMetaGroupDl = mainDiv.SelectSingleNode( ".//dl[@class='series meta group']" );
            ParseDlTable( parsed, seriesMetaGroupDl, _seriesMetaGroupMutators, DlFieldSource.DtText );

            return parsed;
        }

        static void ParseStatsTable( AO3Series series, HtmlNode statsTable )
        {
            HtmlNode statsDl = statsTable.Element( "dl" );
            ParseDlTable( series, statsDl, _statsMutators, DlFieldSource.DtText );
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
