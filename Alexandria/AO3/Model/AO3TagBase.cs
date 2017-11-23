// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.AO3.Querying;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Utils;
using Alexandria.Caching;
using Alexandria.Documents;
using Alexandria.Exceptions.Parsing;
using Alexandria.Model;
using Alexandria.Querying;
using Alexandria.RequestHandles;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    /// <summary>
    /// A base class for anything that AO3 uses as a tag. This can be a character, a ship, or
    /// an actual, legitimate tag (freeform or otherwise).
    /// </summary>
    internal abstract class AO3TagBase : RequestableBase<AO3Source>, IQueryable
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

        delegate TRequestHandle RequestHandleCreatorFunc<TModel, out TRequestHandle>( string text )
            where TModel : IRequestable
            where TRequestHandle : IRequestHandle<TModel>;

        public string Text { get; }

        /// <inheritdoc />
        public IQueryResultsPage<IFanfic, IFanficRequestHandle> QueryFanfics()
        {
            string endpointTag = AO3TagUtils.EscapeTagForUrl( Text );
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

        protected static TagType ParseTagType( HtmlNode mainDiv, Website website, Uri url )
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
                    throw new UnrecognizedTagTypeAlexandriaException( textCategory, website, url );
            }
        }

        protected IReadOnlyList<ITagRequestHandle> ParseParentTags( HtmlNode mainDiv )
        {
            return ParseTagsListboxGroup<ITag, ITagRequestHandle>( mainDiv, "parent", ( tag => new AO3TagRequestHandle( Source, tag ) ) );
        }

        protected IReadOnlyList<ITagRequestHandle> ParseSynonymousTags( HtmlNode mainDiv )
        {
            return ParseTagsListboxGroup<ITag, ITagRequestHandle>( mainDiv, "synonym", ( tag => new AO3TagRequestHandle( Source, tag ) ) );
        }

        protected IReadOnlyList<IShipRequestHandle> ParseChildRelationshipTags( HtmlNode mainDiv )
        {
            return ParseTagsListboxGroup<IShip, IShipRequestHandle>( mainDiv, "relationships", ( tag => new AO3ShipRequestHandle( Source, tag ) ) );
        }

        static IReadOnlyList<TRequestHandle> ParseTagsListboxGroup<TModel, TRequestHandle>( HtmlNode mainDiv, string listboxName, RequestHandleCreatorFunc<TModel, TRequestHandle> requestHandleCreator )
            where TModel : IRequestable
            where TRequestHandle : IRequestHandle<TModel>
        {
            string xpath = $".//div[@class='{listboxName} listbox group']/ul";
            HtmlNode tagsGroupUl = mainDiv.SelectSingleNode( xpath );
            if ( tagsGroupUl == null )
            {
                return new List<TRequestHandle>( 0 );
            }

            List<TRequestHandle> results = new List<TRequestHandle>();

            foreach ( HtmlNode li in tagsGroupUl.Elements( "li" ) )
            {
                HtmlNode tagA = li.Element( "a" );
                string aClass = tagA.GetAttributeValue( "class", string.Empty ) ?? string.Empty;
                if ( !aClass.Equals( "tag" ) )
                {
                    continue;
                }

                string tag = tagA.ReadableInnerText().Trim();
                TRequestHandle requestHandle = requestHandleCreator( tag );
                results.Add( requestHandle );
            }

            return results;
        }
    }
}
