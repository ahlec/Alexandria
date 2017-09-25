// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Utils;
using Alexandria.Documents;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    internal sealed class AO3Fanfic : IFanfic
    {
        AO3Fanfic( Uri url )
        {
            Url = url;
        }

        public Uri Url { get; }

        public string Title { get; private set; }

        public IAuthorRequestHandle Author { get; private set; }

        public MaturityRating Rating { get; private set; }

        public ContentWarnings ContentWarnings { get; private set; }

        public IReadOnlyList<IShipRequestHandle> Ships { get; private set; }

        public IReadOnlyList<ICharacterRequestHandle> Characters { get; private set; }

        public IReadOnlyList<ITagRequestHandle> Tags { get; private set; }

        public int NumberWords { get; private set; }

        public bool IsCompleted { get; private set; }

        public DateTime DateStarted { get; private set; }

        public DateTime DateLastUpdated { get; private set; }

        public int NumberLikes { get; private set; }

        public int NumberComments { get; private set; }

        public ISeriesEntry SeriesInfo { get; private set; }

        public IChapterInfo ChapterInfo { get; private set; }

        public Language Language { get; private set; }

        public string Summary { get; private set; }

        public string AuthorsNote { get; private set; }

        public string Footnote { get; private set; }

        public string Text { get; private set; }

        public static AO3Fanfic Parse( AO3Source source, HtmlCacheableDocument document )
        {
            AO3Fanfic parsed = new AO3Fanfic( document.Url );

            HtmlNode workMetaGroup = document.Html.SelectSingleNode( "//dl[@class='work meta group']" );
            parsed.Rating = AO3MaturityRatingUtils.Parse( workMetaGroup.SelectSingleNode( "dd[@class='rating tags']//a" ).InnerText );
            parsed.ContentWarnings = AO3ContentWarningUtils.Parse( workMetaGroup.SelectSingleNode( "dd[@class='warning tags']/ul" ) );

            HtmlNode relationshipsUl = workMetaGroup.SelectSingleNode( "dd[@class='relationship tags']/ul" );
            List<IShipRequestHandle> ships = new List<IShipRequestHandle>();
            if ( relationshipsUl != null )
            {
                foreach ( HtmlNode li in relationshipsUl.Elements( "li" ) )
                {
                    string shipTag = li.Element( "a" ).ReadableInnerText().Trim();
                    ships.Add( new AO3ShipRequestHandle( source, shipTag ) );
                }
            }

            parsed.Ships = ships;

            HtmlNode charactersUl = workMetaGroup.SelectSingleNode( "dd[@class='character tags']/ul" );
            List<ICharacterRequestHandle> characters = new List<ICharacterRequestHandle>();
            if ( charactersUl != null )
            {
                foreach ( HtmlNode li in charactersUl.Elements( "li" ) )
                {
                    string characterName = li.Element( "a" ).ReadableInnerText().Trim();
                    characters.Add( new AO3CharacterRequestHandle( source, characterName ) );
                }
            }

            parsed.Characters = characters;

            HtmlNode freeformTagsUl = workMetaGroup.SelectSingleNode( "dd[@class='freeform tags']/ul" );
            List<ITagRequestHandle> tags = new List<ITagRequestHandle>();
            if ( freeformTagsUl != null )
            {
                foreach ( HtmlNode li in freeformTagsUl.Elements( "li" ) )
                {
                    string tag = li.Element( "a" ).ReadableInnerText().Trim();
                    tags.Add( new AO3TagRequestHandle( source, tag ) );
                }
            }

            parsed.Tags = tags;
            parsed.Language = LanguageUtils.Parse( workMetaGroup.SelectSingleNode( "dd[@class='language']" ).ReadableInnerText().Trim() );

            // We wind up looking at every <dd> anyways, so this is more efficient than needing to make a lot of XPath calls over the same datasets
            bool hasDateLastUpdated = false;
            HtmlNode statsDl = workMetaGroup.SelectSingleNode( "dd[@class='stats']/dl" );
            foreach ( HtmlNode dd in statsDl.Elements( "dd" ) )
            {
                string ddClass = dd.GetAttributeValue( "class", null )?.ToLowerInvariant();
                string ddValue = dd.InnerText;
                switch ( ddClass )
                {
                    case "words":
                        {
                            parsed.NumberWords = int.Parse( ddValue );
                            break;
                        }

                    case "kudos":
                        {
                            parsed.NumberLikes = int.Parse( ddValue );
                            break;
                        }

                    case "comments":
                        {
                            parsed.NumberComments = int.Parse( ddValue );
                            break;
                        }

                    case "published":
                        {
                            parsed.DateStarted = DateTime.Parse( ddValue );
                            break;
                        }

                    case "status":
                        {
                            parsed.DateLastUpdated = DateTime.Parse( ddValue );
                            hasDateLastUpdated = true;
                            break;
                        }
                }
            }

            if ( !hasDateLastUpdated )
            {
                parsed.DateLastUpdated = parsed.DateStarted;
            }

            HtmlNode seriesSpan = workMetaGroup.SelectSingleNode( "dd[@class='series']/span" );
            if ( seriesSpan != null )
            {
                parsed.SeriesInfo = AO3SeriesEntry.Parse( source, seriesSpan );
            }

            parsed.ChapterInfo = AO3ChapterInfo.Parse( source, document, workMetaGroup );

            HtmlNode prefaceGroup = document.Html.SelectSingleNode( "//div[@class='preface group']" );
            parsed.Title = prefaceGroup.SelectSingleNode( "h2[@class='title heading']" ).ReadableInnerText().Trim();
            HtmlNode authorA = prefaceGroup.SelectSingleNode( ".//a[@rel='author']" );
            if ( authorA != null )
            {
                parsed.Author = AO3AuthorRequestHandle.Parse( source, authorA );
            }

            HtmlNode summaryBlockquote = prefaceGroup.SelectSingleNode( ".//div[@class='summary module']/blockquote" );
            if ( summaryBlockquote != null )
            {
                parsed.Summary = summaryBlockquote.ReadableInnerText().Trim();
            }

            HtmlNode notesBlockquote = prefaceGroup.SelectSingleNode( ".//div[@class='notes module']/blockquote" );
            if ( notesBlockquote != null )
            {
                parsed.AuthorsNote = notesBlockquote.ReadableInnerText().Trim();
            }

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
    }
}
