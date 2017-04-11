using System;

namespace Alexandria.Utils
{
	public static class CacheableObjectsUtils
	{
		public static Boolean IsHtmlObject( CacheableObjects obj )
		{
			switch ( obj )
			{
				case CacheableObjects.FanficHtml:
				case CacheableObjects.AuthorHtml:
				case CacheableObjects.AuthorFanficsHtml:
				case CacheableObjects.TagHtml:
				case CacheableObjects.TagFanficsHtml:
				case CacheableObjects.SeriesHtml:
					{
						return true;
					}
				default:
					{
						return false;
					}
			}
		}

		public static Boolean IsJsonObject( CacheableObjects obj )
		{
			switch ( obj )
			{
				case CacheableObjects.FanficJson:
				case CacheableObjects.AuthorJson:
				case CacheableObjects.TagJson:
				case CacheableObjects.SeriesJson:
					{
						return true;
					}
				default:
					{
						return false;
					}
			}
		}
	}
}
