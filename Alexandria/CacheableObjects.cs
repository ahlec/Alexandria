using System;

namespace Alexandria
{
	[Flags]
	public enum CacheableObjects
	{
		None				= 0,
		FanficHtml			= 1,
		FanficJson			= 2,
		AuthorHtml			= 4,
		AuthorJson			= 8,
		AuthorFanficsHtml	= 16,
		TagHtml				= 32,
		TagJson				= 64,
		TagFanficsHtml		= 128,
		SeriesHtml			= 256,
		SeriesJson			= 512,

		All			= ~0
	}
}
