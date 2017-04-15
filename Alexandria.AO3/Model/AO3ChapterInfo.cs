using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Utils;
using Alexandria.Utils;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3ChapterInfo : IChapterInfo
	{
		AO3ChapterInfo()
		{
		}

		public String ChapterTitle { get; private set; }

		public Int32 ChapterNumber { get; private set; }

		public Int32? TotalNumberChapters { get; private set; }

		public IReadOnlyList<IFanficRequestHandle> Chapters { get; private set; }

		internal static AO3ChapterInfo Parse( HtmlDocument document, HtmlNode workMetaGroup )
		{
			HtmlNode chapterDropdownSelect = document.DocumentNode.SelectSingleNode( "//ul[@id='chapter_index']//select" );
			if ( chapterDropdownSelect == null )
			{
				return null;
			}

			AO3ChapterInfo parsed = new AO3ChapterInfo();
			List<IFanficRequestHandle> chapters = new List<IFanficRequestHandle>( chapterDropdownSelect.ChildNodes.Count );
			Int32 chapterNumber = 1;
			foreach ( HtmlNode chapterOption in chapterDropdownSelect.Elements( HtmlUtils.OptionsHtmlTag ) )
			{
				String fanficHandle = chapterOption.GetAttributeValue( "value", null );
				chapters.Add( new AO3FanficRequestHandle( fanficHandle ) );

				if ( chapterOption.GetAttributeValue( "selected", null ) != null )
				{
					parsed.ChapterNumber = chapterNumber;
				}

				++chapterNumber;
			}
			parsed.Chapters = chapters;

			HtmlNode chaptersDd = workMetaGroup.SelectSingleNode( ".//dd[@class='chapters']" );
			String[] chaptersInfo = chaptersDd.InnerText.Trim().Split( '/' );
			if ( chaptersInfo[1] == "?" )
			{
				parsed.TotalNumberChapters = null;
			}
			else
			{
				parsed.TotalNumberChapters = Int32.Parse( chaptersInfo[1] );
			}

			HtmlNode chapterPrefaceGroup = document.DocumentNode.SelectSingleNode( "//div[@class='chapter preface group']" );
			HtmlNode chapterTitleTextNode = chapterPrefaceGroup.LastChild;
			if ( chapterTitleTextNode.Name != "a" && chapterTitleTextNode.Name != "A" )
			{
				String tentativeTitle = chapterTitleTextNode.ReadableInnerText();
				if ( !String.IsNullOrWhiteSpace( tentativeTitle ) )
				{
					parsed.ChapterTitle = tentativeTitle.Trim();
				}
			}

			return parsed;
		}
	}
}
