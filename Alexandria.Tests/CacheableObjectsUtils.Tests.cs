// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.Caching;
using Alexandria.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alexandria.Tests
{
    [TestClass]
    [TestCategory( UnitTestConstants.UtilTestsCategory )]
    public class CacheableObjectsUtilsTests
    {
        class ObjectInfo
        {
            public ObjectInfo( CacheableObjects obj, bool isHtml, bool isJson )
            {
                CacheableObject = obj;
                IsHtml = isHtml;
                IsJson = isJson;
            }

            public CacheableObjects CacheableObject { get; }

            public bool IsHtml { get; }

            public bool IsJson { get; }
        }

        public CacheableObjectsUtilsTests()
        {
            _objectInfo = new List<ObjectInfo>
            {
                new ObjectInfo( CacheableObjects.None, false, false ),
                new ObjectInfo( CacheableObjects.FanficHtml, true, false ),
                new ObjectInfo( CacheableObjects.FanficJson, false, true ),
                new ObjectInfo( CacheableObjects.AuthorHtml, true, false ),
                new ObjectInfo( CacheableObjects.AuthorJson, false, true ),
                new ObjectInfo( CacheableObjects.AuthorFanficsHtml, true, false ),
                new ObjectInfo( CacheableObjects.TagHtml, true, false ),
                new ObjectInfo( CacheableObjects.TagJson, false, true ),
                new ObjectInfo( CacheableObjects.TagFanficsHtml, true, false ),
                new ObjectInfo( CacheableObjects.SeriesHtml, true, false ),
                new ObjectInfo( CacheableObjects.SeriesJson, false, true ),
                new ObjectInfo( CacheableObjects.All, false, false )
            };
        }

        [TestMethod]
        public void CacheableObjectsUtils_UnitTestDataIsValid()
        {
            foreach ( CacheableObjects obj in Enum.GetValues( typeof( CacheableObjects ) ) )
            {
                Assert.IsTrue( _objectInfo.Any( info => info.CacheableObject == obj ) );
            }
        }

        [TestMethod]
        public void CacheableObjectsUtils_IsHtmlObject()
        {
            foreach ( ObjectInfo info in _objectInfo )
            {
                Assert.AreEqual( info.IsHtml, CacheableObjectsUtils.IsHtmlObject( info.CacheableObject ) );
            }
        }

        [TestMethod]
        public void CacheableObjectsUtils_IsJsonObject()
        {
            foreach ( ObjectInfo info in _objectInfo )
            {
                Assert.AreEqual( info.IsJson, CacheableObjectsUtils.IsJsonObject( info.CacheableObject ) );
            }
        }

        readonly IReadOnlyList<ObjectInfo> _objectInfo;
    }
}
