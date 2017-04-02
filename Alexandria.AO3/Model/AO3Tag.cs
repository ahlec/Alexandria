using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;
using Alexandria.AO3.Utils;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3Tag : ITag
	{
		AO3Tag()
		{
		}

		#region ITag

		public String Text { get; private set; }

		public IReadOnlyList<ITagRequestHandle> ParentTags { get; private set; }

		public IReadOnlyList<ITagRequestHandle> SynonymousTags { get; private set; }

		#endregion

		internal static AO3Tag Parse( HtmlDocument document )
		{
			AO3Tag parsed = new AO3Tag();

			HtmlNode mainDiv = document.DocumentNode.SelectSingleNode( "//div[@class='tags-show region']" );

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
	}
}
