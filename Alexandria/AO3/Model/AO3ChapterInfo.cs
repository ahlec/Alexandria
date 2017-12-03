// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Alexandria.AO3.RequestHandles;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Model
{
    internal sealed class AO3ChapterInfo : HtmlParserBase, IChapterInfo
    {
        AO3ChapterInfo()
        {
        }

        public string ChapterTitle { get; private set; }

        public int ChapterNumber { get; private set; }

        public int? TotalNumberChapters { get; private set; }

        public IReadOnlyList<IFanficRequestHandle> Chapters { get; private set; }

        internal static AO3ChapterInfo Parse( AO3Source source, Document document, HtmlNode workMetaGroup )
        {
            HtmlNode chapterDropdownSelect = document.Html.SelectSingleNode( "//ul[@id='chapter_index']//select" );
            if ( chapterDropdownSelect == null )
            {
                return null;
            }

            AO3ChapterInfo parsed = new AO3ChapterInfo();
            List<IFanficRequestHandle> chapters = new List<IFanficRequestHandle>( chapterDropdownSelect.ChildNodes.Count );
            int chapterNumber = 1;
            foreach ( HtmlNode chapterOption in chapterDropdownSelect.Elements( "option" ) )
            {
                string fanficHandle = chapterOption.GetAttributeValue( "value", null );
                chapters.Add( new AO3FanficRequestHandle( source, fanficHandle ) );

                if ( chapterOption.GetAttributeValue( "selected", null ) != null )
                {
                    parsed.ChapterNumber = chapterNumber;
                }

                ++chapterNumber;
            }

            parsed.Chapters = chapters;

            HtmlNode chaptersDd = workMetaGroup.SelectSingleNode( ".//dd[@class='chapters']" );
            string[] chaptersInfo = chaptersDd.InnerText.Trim().Split( '/' );
            if ( chaptersInfo[1] == "?" )
            {
                parsed.TotalNumberChapters = null;
            }
            else
            {
                parsed.TotalNumberChapters = int.Parse( chaptersInfo[1] );
            }

            HtmlNode chapterPrefaceGroup = document.Html.SelectSingleNode( "//div[@class='chapter preface group']" );
            HtmlNode chapterTitleTextNode = chapterPrefaceGroup.LastChild;
            if ( chapterTitleTextNode.Name != "a" && chapterTitleTextNode.Name != "A" )
            {
                string tentativeTitle = GetReadableInnerText( chapterTitleTextNode );
                if ( !string.IsNullOrWhiteSpace( tentativeTitle ) )
                {
                    parsed.ChapterTitle = tentativeTitle.Trim();
                }
            }

            return parsed;
        }
    }
}
