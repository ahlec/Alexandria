// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.AO3.Querying;
using Alexandria.Model;
using Alexandria.Querying;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    /// <summary>
    /// A concrete class for parsing an author from AO3.
    /// </summary>
    internal sealed class AO3Author : AO3ModelBase<AO3Author>, IAuthor
    {
        static readonly IReadOnlyDictionary<string, TableFieldMutator> _metaMutators = new Dictionary<string, TableFieldMutator>
        {
            { "My pseuds", ( author, value ) => author.Nicknames = CollectPseuds( value ) },
            { "I joined on", ( author, value ) => author.DateJoined = DateTime.Parse( value.InnerText ) },
            { "I live in", ( author, value ) => author.Location = GetReadableInnerText( value ) },
            { "My birthday", ( author, value ) => author.Birthday = DateTime.Parse( value.InnerText ) }
        };

        AO3Author( AO3Source source, Uri url )
            : base( source, url )
        {
        }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<string> Nicknames { get; private set; }

        /// <inheritdoc />
        public DateTime DateJoined { get; private set; }

        /// <inheritdoc />
        public string Location { get; private set; }

        /// <inheritdoc />
        public DateTime? Birthday { get; private set; }

        /// <inheritdoc />
        public string Biography { get; private set; }

        /// <inheritdoc />
        public int NumberFanfics { get; private set; }

        /// <summary>
        /// Parses an HTML page into an instance of an <seealso cref="AO3Author"/>.
        /// </summary>
        /// <param name="source">The source that the HTML page came from, which is then stored for
        /// querying fanfics and also passed along to any nested request handles for them to parse
        /// data with as well.</param>
        /// <param name="document">The document that came from the website itself.</param>
        /// <returns>An instance of <seealso cref="AO3Author"/> that was parsed and configured using
        /// the information provided.</returns>
        public static AO3Author Parse( AO3Source source, Document document )
        {
            HtmlNode userHomeProfile = document.Html.SelectSingleNode( "//div[@class='user home profile']" );

            AO3Author parsed = new AO3Author( source, document.Url )
            {
                Name = GetReadableInnerText( userHomeProfile.SelectSingleNode( "div[@class='primary header module']/h2[@class='heading']/a" ) ),
                Biography = GetReadableInnerText( userHomeProfile.SelectSingleNode( "div[@class='bio module']/blockquote" ) ),
                NumberFanfics = ParseNumberAuthorWorks( document.Html )
            };

            parsed.ParseProfileMetaData( userHomeProfile );

            return parsed;
        }

        /// <inheritdoc />
        public IQueryResultsPage<IFanfic, IFanficRequestHandle> QueryFanfics()
        {
            return AO3QueryResults.Retrieve( Source, "users", Name, 1 );
        }

        static IReadOnlyList<string> CollectPseuds( HtmlNode pseudsDd )
        {
            return pseudsDd.Elements( "a" ).Select( GetReadableInnerText ).ToList();
        }

        static int ParseNumberAuthorWorks( HtmlNode html )
        {
            HtmlNode dashboardDiv = html.SelectSingleNode( "//div[@id='dashboard']" );
            foreach ( HtmlNode dashboardA in dashboardDiv.SelectNodes( ".//a" ) )
            {
                if ( !dashboardA.InnerText.StartsWith( "Work" ) )
                {
                    continue;
                }

                int startIndex = dashboardA.InnerText.IndexOf( '(' );
                int endIndex = dashboardA.InnerText.IndexOf( ')' );

                return int.Parse( dashboardA.InnerText.Substring( startIndex + 1, endIndex - startIndex - 1 ) );
            }

            return 0;
        }

        void ParseProfileMetaData( HtmlNode userHomeProfile )
        {
            HtmlNode metaDl = userHomeProfile.SelectSingleNode( ".//dl[@class='meta']" );
            ParseDlTable( this, metaDl, _metaMutators, DlFieldSource.DtText );
        }
    }
}
