using System;
using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
	public interface IFanfic : IRequestable
	{
		String Title { get; }

		IAuthorRequestHandle Author { get; }

		MaturityRating Rating { get; }

		ContentWarnings ContentWarnings { get; }

		IReadOnlyList<IShip> Ships { get; }

		IReadOnlyList<ICharacterRequestHandle> Characters { get; }

		IReadOnlyList<ITagRequestHandle> Tags { get; }

		Int32 NumberWords { get; }

		DateTime DateStarted { get; }

		DateTime DateLastUpdated { get; }

		Int32 NumberLikes { get; }

		Int32 NumberComments { get; }

		ISeriesEntry SeriesInfo { get; }

		IChapterInfo ChapterInfo { get; }

		Language Language { get; }

		String Summary { get; }

		String AuthorsNote { get; }

		String Footnote { get; }

		String Text { get; }
	}
}
