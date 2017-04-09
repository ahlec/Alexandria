using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.Utils;

namespace Alexandria.Base.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.UtilTestsCategory )]
	public class CacheableObjectsUtilsTests
	{
		class ObjectInfo
		{
			public ObjectInfo( CacheableObjects obj, Boolean isHtml, Boolean isJson )
			{
				CacheableObject = obj;
				IsHtml = isHtml;
				IsJson = isJson;
			}

			public readonly CacheableObjects CacheableObject;
			public readonly Boolean IsHtml;
			public readonly Boolean IsJson;
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
				new ObjectInfo( CacheableObjects.TagHtml, true, false ),
				new ObjectInfo( CacheableObjects.TagJson, false, true ),
				new ObjectInfo( CacheableObjects.TagFanficsHtml, true, false ),
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
