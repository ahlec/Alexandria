using System;

namespace Alexandria
{
	[Flags]
	public enum CacheableObjects
	{
		None			= 0,
		FanficHtml		= 1,
		FanficJson		= 2,
		AuthorHtml		= 4,
		AuthorJson		= 8,
		TagHtml			= 16,
		TagJson			= 32,
		TagFanficsHtml	= 64,

		All			= ~0
	}
}
