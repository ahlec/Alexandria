// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.AO3.RequestHandles;
using Alexandria.Exceptions.Parsing;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    /// <summary>
    /// A concrete class for containing information about a particular chapter of an <seealso cref="AO3Fanfic"/>.
    /// </summary>
    internal sealed class AO3ChapterInfo : HtmlParserBase, IChapterInfo
    {
        AO3ChapterInfo()
        {
        }

        /// <inheritdoc />
        public string ChapterTitle { get; private set; }

        /// <inheritdoc />
        public int ChapterNumber { get; private set; }

        /// <inheritdoc />
        public int? TotalNumberChapters { get; private set; }

        /// <inheritdoc />
        public IReadOnlyList<IFanficRequestHandle> Chapters { get; private set; }

        /// <summary>
        /// Parses an HTML page into an instance of an <seealso cref="AO3ChapterInfo"/>.
        /// </summary>
        /// <param name="source">The source that the HTML page came from, which is then stored for
        /// querying fanfics and also passed along to any nested request handles for them to parse
        /// data with as well.</param>
        /// <param name="document">The document that came from the website itself.</param>
        /// <param name="workMetaGroup">The result of running the XPath query "//dl[@class='work meta group']"
        /// on the HTML document. This is passed in as we're already computing it in the places that we're calling
        /// this function from, and it saves us from needing to run an extra XPath query unnecessarily.</param>
        /// <returns>If the appropriate HTML data that would indicate that the fanfic is a multi-chapter
        /// fanfic is not found, this will return null. Otherwise, this will return an instance of
        /// <seealso cref="AO3ChapterInfo"/> that was parsed and configured using the information provided.</returns>
        internal static AO3ChapterInfo Parse( AO3Source source, Document document, HtmlNode workMetaGroup )
        {
            HtmlNode chapterDropdownSelect = document.Html.SelectSingleNode( "//ul[@id='chapter_index']//select" );
            if ( chapterDropdownSelect == null )
            {
                return null;
            }

            IReadOnlyList<IFanficRequestHandle> allChapters = GetAllChapters( chapterDropdownSelect, source, document.Url, out int selectedChapterNumber );
            AO3ChapterInfo parsed = new AO3ChapterInfo
            {
                Chapters = allChapters,
                ChapterNumber = selectedChapterNumber,
                TotalNumberChapters = GetTotalNumberChapters(workMetaGroup),
                ChapterTitle = GetChapterTitle(document.Html)
            };
            return parsed;
        }

        static IReadOnlyList<IFanficRequestHandle> GetAllChapters( HtmlNode chapterDropdownSelect, AO3Source source, Uri url, out int selectedChapterNumber )
        {
            List<IFanficRequestHandle> chapters = new List<IFanficRequestHandle>( chapterDropdownSelect.ChildNodes.Count );

            int? selectedChapter = null;
            foreach ( HtmlNode chapterOption in chapterDropdownSelect.Elements( "option" ) )
            {
                string fanficHandle = chapterOption.GetAttributeValue( "value", null );
                chapters.Add( new AO3FanficRequestHandle( source, fanficHandle ) );

                if ( chapterOption.GetAttributeValue( "selected", null ) != null )
                {
                    selectedChapter = chapters.Count;
                }
            }

            if ( !selectedChapter.HasValue )
            {
                throw new UnrecognizedFormatAlexandriaException( Website.AO3, url, "There was no @selected tag in any of the chapter <options> nodes." );
            }

            selectedChapterNumber = selectedChapter.Value;
            return chapters;
        }

        static int? GetTotalNumberChapters( HtmlNode workMetaGroup )
        {
            HtmlNode chaptersDd = workMetaGroup.SelectSingleNode( ".//dd[@class='chapters']" );
            string[] chaptersInfo = chaptersDd.InnerText.Trim().Split( '/' );

            if ( chaptersInfo[1] == "?" )
            {
                return null;
            }

            return int.Parse( chaptersInfo[1] );
        }

        static string GetChapterTitle( HtmlNode documentNode )
        {
            HtmlNode chapterPrefaceGroup = documentNode.SelectSingleNode( "//div[@class='chapter preface group']" );
            HtmlNode chapterTitleTextNode = chapterPrefaceGroup?.LastChild;
            if ( chapterTitleTextNode == null )
            {
                return null;
            }

            if ( string.Equals( chapterTitleTextNode.Name, "a", StringComparison.InvariantCultureIgnoreCase ) )
            {
                return null;
            }

            string tentativeTitle = GetReadableInnerText( chapterTitleTextNode );
            return tentativeTitle;
        }
    }
}
