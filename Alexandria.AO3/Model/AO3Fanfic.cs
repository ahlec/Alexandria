using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.Utils;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3Fanfic : IFanfic
	{
		AO3Fanfic()
		{
		}

		#region IFanfic

		public String Title { get; private set; }

		public IAuthorRequestHandle Author { get; private set; }

		public MaturityRating Rating { get; private set; }

		public ContentWarnings ContentWarnings { get; private set; }

		public IReadOnlyList<IShip> Ships { get; private set; }

		public IReadOnlyList<ICharacterRequestHandle> Characters { get; private set; }

		public IReadOnlyList<ITagRequestHandle> Tags { get; private set; }

		public Int32 NumberWords { get; private set; }

		public Boolean IsCompleted { get; private set; }

		public DateTime DateStarted { get; private set; }

		public DateTime DateLastUpdated { get; private set; }

		public Int32 NumberLikes { get; private set; }

		public Int32 NumberComments { get; private set; }

		public ISeriesEntry SeriesInfo { get; private set; }

		public IChapterInfo ChapterInfo { get; private set; }

		public Language Language { get; private set; }

		public String Summary { get; private set; }

		public String AuthorsNote { get; private set; }

		public String Footnote { get; private set; }

		public String Text { get; private set; }

		#endregion // IFanfic

		public static AO3Fanfic Parse( HtmlDocument document )
		{
			AO3Fanfic parsed = new AO3Fanfic();

			HtmlNode workMetaGroup = document.DocumentNode.SelectSingleNode( "//dl[@class='work meta group']" );
			parsed.Rating = ParseUtils.ParseMaturityRatingFromAO3( workMetaGroup.SelectSingleNode( "dd[@class='rating tags']//a" ).InnerText );
			parsed.ContentWarnings = ParseUtils.ParseContentWarningsFromAO3( workMetaGroup.SelectSingleNode( "dd[@class='warning tags']/ul" ) );

			HtmlNode relationshipsUl = workMetaGroup.SelectSingleNode( "dd[@class='relationship tags']/ul" );
			List<IShip> ships = new List<IShip>();
			if ( relationshipsUl != null )
			{
				foreach ( HtmlNode li in relationshipsUl.Elements( "li" ) )
				{
					String shipTag = li.Element( "a" ).ReadableInnerText().Trim();
					ships.Add( AO3Ship.Parse( shipTag ) );
				}
			}
			parsed.Ships = ships;

			HtmlNode charactersUl = workMetaGroup.SelectSingleNode( "dd[@class='character tags']/ul" );
			List<ICharacterRequestHandle> characters = new List<ICharacterRequestHandle>();
			if ( charactersUl != null )
			{
				foreach ( HtmlNode li in charactersUl.Elements( "li" ) )
				{
					String characterName = li.Element( "a" ).ReadableInnerText().Trim();
					characters.Add( new AO3CharacterRequestHandle( characterName ) );
				}
			}
			parsed.Characters = characters;

			HtmlNode freeformTagsUl = workMetaGroup.SelectSingleNode( "dd[@class='freeform tags']/ul" );
			List<ITagRequestHandle> tags = new List<ITagRequestHandle>();
			if ( freeformTagsUl != null )
			{
				foreach ( HtmlNode li in freeformTagsUl.Elements( "li" ) )
				{
					String tag = li.Element( "a" ).ReadableInnerText().Trim();
					tags.Add( new AO3TagRequestHandle( tag ) );
				}
			}
			parsed.Tags = tags;
			parsed.Language = LanguageUtils.Parse( workMetaGroup.SelectSingleNode( "dd[@class='language']" ).ReadableInnerText().Trim() );

			// We wind up looking at every <dd> anyways, so this is more efficient than needing to make a lot of XPath calls over the same datasets
			Boolean hasDateLastUpdated = false;
			HtmlNode statsDl = workMetaGroup.SelectSingleNode( "dd[@class='stats']/dl" );
			foreach ( HtmlNode dd in statsDl.Elements( "dd" ) )
			{
				String ddClass = dd.GetAttributeValue( "class", null )?.ToLowerInvariant();
				String ddValue = dd.InnerText;
				switch ( ddClass )
				{
					case "words":
						{
							parsed.NumberWords = Int32.Parse( ddValue );
							break;
						}
					case "kudos":
						{
							parsed.NumberLikes = Int32.Parse( ddValue );
							break;
						}
					case "comments":
						{
							parsed.NumberComments = Int32.Parse( ddValue );
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
				parsed.SeriesInfo = AO3SeriesEntry.Parse( seriesSpan );
			}

			parsed.ChapterInfo = AO3ChapterInfo.Parse( document, workMetaGroup );

			HtmlNode prefaceGroup = document.DocumentNode.SelectSingleNode( "//div[@class='preface group']" );
			parsed.Title = prefaceGroup.SelectSingleNode( "h2[@class='title heading']" ).ReadableInnerText().Trim();
			HtmlNode authorA = prefaceGroup.SelectSingleNode( ".//a[@rel='author']" );
			if ( authorA != null )
			{
				parsed.Author = AO3AuthorRequestHandle.Parse( authorA );
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

			HtmlNode workEndnotesBlockquote = document.DocumentNode.SelectSingleNode( "//div[@id='work_endnotes']/blockquote" );
			if ( workEndnotesBlockquote != null )
			{
				parsed.Footnote = workEndnotesBlockquote.ReadableInnerText().Trim();
			}

			HtmlNode userstuffModuleDiv = document.DocumentNode.SelectSingleNode( "//div[@class='userstuff module']" );
			if ( userstuffModuleDiv == null )
			{
				userstuffModuleDiv = document.DocumentNode.SelectSingleNode( "//div[@id='chapters']/div[contains( @class, 'userstuff' )]" );
			}
			userstuffModuleDiv.Element( "h3" )?.Remove();
			parsed.Text = userstuffModuleDiv.ReadableInnerText().Trim();

			return parsed;
		}
	}
}
