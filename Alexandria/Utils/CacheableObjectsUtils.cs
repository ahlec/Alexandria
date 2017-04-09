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
				case CacheableObjects.TagHtml:
				case CacheableObjects.TagFanficsHtml:
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
