// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.AO3.RequestHandles;
using Alexandria.Caching;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.Utils;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    internal sealed class AO3Tag : ITag
    {
        AO3Tag( AO3Source source, Uri url )
        {
            _source = source;
            Url = url;
        }

        public Uri Url { get; }

        public TagType Type { get; private set; }

        public string Text { get; private set; }

        public IReadOnlyList<ITagRequestHandle> ParentTags { get; private set; }

        public IReadOnlyList<ITagRequestHandle> SynonymousTags { get; private set; }

        public IQueryResultsPage<IFanfic, IFanficRequestHandle> QueryFanfics()
        {
            string endpointTag = Text.Replace( "/", "*s*" );
            return AO3QueryResults.Retrieve( _source, CacheableObjects.TagFanficsHtml, "tags", endpointTag, 1 );
        }

        internal static AO3Tag Parse( AO3Source source, Uri url, HtmlDocument document )
        {
            AO3Tag parsed = new AO3Tag( source, url );

            HtmlNode mainDiv = document.DocumentNode.SelectSingleNode( "//div[@class='tags-show region']" );

            string mainContentPText = mainDiv.SelectSingleNode( "div[@class='tag home profile']/p" ).InnerText;
            string mainContentPFirstSentence = mainContentPText.Substring( 0, mainContentPText.IndexOf( '.' ) );
            int mainContentSentenceStartLength = "This tag belongs to the ".Length;
            string textCategory = mainContentPFirstSentence.Substring( mainContentSentenceStartLength, mainContentPText.LastIndexOf( " Category", StringComparison.InvariantCultureIgnoreCase ) - mainContentSentenceStartLength );
            switch ( textCategory )
            {
                case "Character":
                    {
                        parsed.Type = TagType.Character;
                        break;
                    }

                case "Relationship":
                    {
                        parsed.Type = TagType.Relationship;
                        break;
                    }

                case "Additional Tags":
                    {
                        parsed.Type = TagType.Miscellaneous;
                        break;
                    }

                default:
                    {
                        throw new NotImplementedException();
                    }
            }

            parsed.Text = mainDiv.SelectSingleNode( ".//div[@class='primary header module']/h2" ).ReadableInnerText().Trim();

            List<ITagRequestHandle> parentTags = new List<ITagRequestHandle>();
            HtmlNode parentUl = mainDiv.SelectSingleNode( ".//div[@class='parent listbox group']/ul" );
            if ( parentUl != null )
            {
                foreach ( HtmlNode li in parentUl.Elements( "li" ) )
                {
                    parentTags.Add( new AO3TagRequestHandle( li.ReadableInnerText().Trim() ) );
                }
            }

            parsed.ParentTags = parentTags;

            List<ITagRequestHandle> synonymousTags = new List<ITagRequestHandle>();
            HtmlNode synonymUl = mainDiv.SelectSingleNode( ".//div[@class='synonym listbox group']/ul" );
            if ( synonymUl != null )
            {
                foreach ( HtmlNode li in synonymUl.Elements( "li" ) )
                {
                    synonymousTags.Add( new AO3TagRequestHandle( li.ReadableInnerText().Trim() ) );
                }
            }

            parsed.SynonymousTags = synonymousTags;

            return parsed;
        }

        readonly AO3Source _source;
    }
}
