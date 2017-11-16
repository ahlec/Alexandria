// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.AO3.RequestHandles;
using Alexandria.Caching;
using Alexandria.Documents;
using Alexandria.Exceptions.Parsing;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    /// <summary>
    /// A base class for anything that AO3 uses as a tag. This can be a character, a ship, or
    /// an actual, legitimate tag (freeform or otherwise).
    /// </summary>
    internal abstract class AO3TagBase
    {
        AO3Source _source;

        protected AO3TagBase( AO3Source source, Uri url, HtmlNode mainDiv )
        {
            _source = source;
            Url = url;

            Text = ParseTagText( mainDiv );
        }

        /// <summary>
        /// Gets the URL of the webpage for this tag on AO3.
        /// </summary>
        public Uri Url { get; }

        public string Text { get; }

        public IQueryResultsPage<IFanfic, IFanficRequestHandle> QueryFanfics()
        {
            string endpointTag = Text.Replace( "/", "*s*" );
            return AO3QueryResults.Retrieve( _source, CacheableObjects.TagFanficsHtml, "tags", endpointTag, 1 );
        }

        protected static HtmlNode GetMainDiv( HtmlCacheableDocument document )
        {
            HtmlNode mainDiv = document.Html.SelectSingleNode( "//div[@class='tags-show region']" );
            return mainDiv;
        }

        protected static string ParseTagText( HtmlNode mainDiv )
        {
            return mainDiv.SelectSingleNode( ".//div[@class='primary header module']/h2" ).ReadableInnerText().Trim();
        }

        protected static TagType ParseTagType( HtmlNode mainDiv )
        {
            string mainContentPText = mainDiv.SelectSingleNode( "div[@class='tag home profile']/p" ).InnerText;
            string mainContentPFirstSentence = mainContentPText.Substring( 0, mainContentPText.IndexOf( '.' ) );
            int mainContentSentenceStartLength = "This tag belongs to the ".Length;
            string textCategory = mainContentPFirstSentence.Substring( mainContentSentenceStartLength, mainContentPText.LastIndexOf( " Category", StringComparison.InvariantCultureIgnoreCase ) - mainContentSentenceStartLength );

            switch ( textCategory )
            {
                case "Character":
                    return TagType.Character;
                case "Relationship":
                    return TagType.Relationship;
                case "Additional Tags":
                    return TagType.Miscellaneous;
                default:
                    throw new UnrecognizedTagTypeAlexandriaException( textCategory );
            }
        }

        protected IReadOnlyList<ITagRequestHandle> ParseParentTags( HtmlNode mainDiv )
        {
            List<ITagRequestHandle> parentTags = new List<ITagRequestHandle>();
            HtmlNode parentUl = mainDiv.SelectSingleNode( ".//div[@class='parent listbox group']/ul" );
            if ( parentUl != null )
            {
                foreach ( HtmlNode li in parentUl.Elements( "li" ) )
                {
                    parentTags.Add( new AO3TagRequestHandle( _source, li.ReadableInnerText().Trim() ) );
                }
            }

            return parentTags;
        }

        protected IReadOnlyList<ITagRequestHandle> ParseSynonymousTags( HtmlNode mainDiv )
        {
            List<ITagRequestHandle> synonymousTags = new List<ITagRequestHandle>();
            HtmlNode synonymUl = mainDiv.SelectSingleNode( ".//div[@class='synonym listbox group']/ul" );
            if ( synonymUl != null )
            {
                foreach ( HtmlNode li in synonymUl.Elements( "li" ) )
                {
                    synonymousTags.Add( new AO3TagRequestHandle( _source, li.ReadableInnerText().Trim() ) );
                }
            }

            return synonymousTags;
        }
    }
}
