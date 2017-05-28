using System;
using Bibliothecary.Data.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bibliothecary.Data.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.UtilTestsCategory )]
	public class Test_CryptographyUtils
	{
		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void CryptographyUtils_Encrypt_ThrowsOnNull()
		{
			CryptographyUtils.Encrypt( null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void CryptographyUtils_Decrypt_ThrowsOnNull()
		{
			CryptographyUtils.Decrypt( null );
		}

		[TestMethod]
		public void CryptographyUtils_SuccessfullyRoundTrips()
		{
			String[] testStrings =
			{
				"Hello World",
				"Alexandria",
				"Archive of Our Own",
				"Cryptography",
				"S73R3k!"
			};

			foreach ( String testString in testStrings )
			{
				String encrypted = CryptographyUtils.Encrypt( testString );
				String decrypted = CryptographyUtils.Decrypt( encrypted );
				Assert.AreEqual( testString, decrypted );
			}
		}
	}
}
