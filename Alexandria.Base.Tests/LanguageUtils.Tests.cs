using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.Utils;

namespace Alexandria.Base.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.UtilTestsCategory )]
	public class LanguageUtilsTests
	{
		[TestMethod]
		public void BaseLanguageUtils_ParsesAllEnumValues()
		{
			foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
			{
				Language parsed = LanguageUtils.Parse( language.ToString() );
				Assert.AreEqual( language, parsed );
			}
		}
	}
}
