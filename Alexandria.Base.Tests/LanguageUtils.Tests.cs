using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
