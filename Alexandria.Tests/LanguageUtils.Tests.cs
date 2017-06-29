using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alexandria.Model;
using Alexandria.Utils;

namespace Alexandria.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.UtilTestsCategory )]
	public class LanguageUtilsTests
	{
		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ), AllowDerivedTypes = false )]
		public void BaseLanguageUtils_ParseThrowsOnNull()
		{
			LanguageUtils.Parse( null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ), AllowDerivedTypes = false )]
		public void BaseLanguageUtils_ParseThrowsOnEmpty()
		{
			LanguageUtils.Parse( String.Empty );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ), AllowDerivedTypes = false )]
		public void BaseLanguageUtils_ParseThrowsOnWhitespace()
		{
			LanguageUtils.Parse( "   " );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentException ), AllowDerivedTypes = false )]
		public void BaseLanguageUtils_ThrowsOnInvalidLanguage()
		{
			LanguageUtils.Parse( "C#" );
		}

		[TestMethod]
		public void BaseLanguageUtils_ParsesAllEnumValues()
		{
			foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
			{
				Language parsed = LanguageUtils.Parse( language.ToString() );
				Assert.AreEqual( language, parsed );
			}
		}

		[TestMethod]
		public void BaseLanguageUtils_ParsesAllNativeNameValues()
		{
			foreach ( Language language in Enum.GetValues( typeof( Language ) ) )
			{
				ILanguageInfo info = LanguageUtils.GetInfo( language );
				Language parsed = LanguageUtils.Parse( info.NativeName );
				Assert.AreEqual( language, parsed );
			}
		}
	}
}
