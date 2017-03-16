using System;

namespace Alexandria.Model
{
	public interface IFanfic
	{
		String Title { get; }

		//IAuthor Author { get; }

		MaturityRating Rating { get; }

		ContentWarnings ContentWarnings { get; }

		Int32 NumberWords { get; }

		DateTime DateStartedUtc { get; }

		//DateTime DateLastUpdatedUtc { get; }

		Int32 NumberLikes { get; }

		Int32 NumberComments { get; }

		ISeriesEntry SeriesInfo { get; }
	}
}
