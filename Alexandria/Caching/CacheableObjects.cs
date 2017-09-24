// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Caching
{
    [Flags]
    public enum CacheableObjects
    {
        None = 0,
        FanficHtml = 1,
        FanficJson = 2,
        AuthorHtml = 4,
        AuthorJson = 8,
        AuthorFanficsHtml = 16,
        TagHtml = 32,
        TagJson = 64,
        TagFanficsHtml = 128,
        SeriesHtml = 256,
        SeriesJson = 512,

        All = ~0
    }
}
