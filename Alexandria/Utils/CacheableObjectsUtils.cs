// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using Alexandria.Caching;

namespace Alexandria.Utils
{
    public static class CacheableObjectsUtils
    {
        public static bool IsHtmlObject( CacheableObjects obj )
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

        public static bool IsJsonObject( CacheableObjects obj )
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
