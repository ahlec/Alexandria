using System;
using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
	public interface IChapterInfo
	{
		String ChapterTitle { get; }

		Int32 ChapterNumber { get; }

		Int32? TotalNumberChapters { get; }

		IReadOnlyList<IFanficRequestHandle> Chapters { get; }
	}
}
