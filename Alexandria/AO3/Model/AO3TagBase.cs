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
    internal abstract class AO3TagBase : RequestableBase<AO3Source>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AO3TagBase"/> class.
        /// </summary>
        /// <param name="source">The configured class for accessing AO3.</param>
        /// <param name="url">The URL for this tag page on AO3's website.</param>
        /// <param name="mainDiv">Can be retrieved by using <seealso cref="GetMainDiv"/>. This is
        /// the primary location in the HTML document where all of the tag-related data on AO3 is.</param>
        protected AO3TagBase( AO3Source source, Uri url, HtmlNode mainDiv )
            : base( source, url )
        {
            Text = ParseTagText( mainDiv );
        }

        public string Text { get; }

        public IQueryResultsPage<IFanfic, IFanficRequestHandle> QueryFanfics()
        {
            string endpointTag = Text.Replace( "/", "*s*" );
            return AO3QueryResults.Retrieve( Source, CacheableObjects.TagFanficsHtml, "tags", endpointTag, 1 );
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
                    parentTags.Add( new AO3TagRequestHandle( Source, li.ReadableInnerText().Trim() ) );
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
                    synonymousTags.Add( new AO3TagRequestHandle( Source, li.ReadableInnerText().Trim() ) );
                }
            }

            return synonymousTags;
        }
    }
}
