using System;
using System.Runtime.InteropServices;
using System.Security;
using Bibliothecary.Core.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bibliothecary.Core.Tests
{
	[TestClass]
	[TestCategory( UnitTestConstants.UtilTestsCategory )]
	public class Test_CryptographyUtils
	{
		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void CryptographyUtils_EncryptString_ThrowsOnNull()
		{
			CryptographyUtils.EncryptString( null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void CryptographyUtils_EncryptSecureString_ThrowsOnNull()
		{
			CryptographyUtils.EncryptSecureString( null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void CryptographyUtils_DecryptString_ThrowsOnNull()
		{
			CryptographyUtils.DecryptString( null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void CryptographyUtils_DecryptSecureString_ThrowsOnNull()
		{
			CryptographyUtils.DecryptSecureString( null );
		}

		[TestMethod]
		public void CryptographyUtils_EncryptString_SuccessfullyRoundTrips()
		{
			foreach ( String testString in _testStrings )
			{
				String encrypted = CryptographyUtils.EncryptString( testString );
				String decrypted = CryptographyUtils.DecryptString( encrypted );
				Assert.AreEqual( testString, decrypted );
			}
		}

		[TestMethod]
		public void CryptographyUtils_EncryptSecureString_SuccessfullyRoundTrips()
		{
			foreach ( String testString in _testStrings )
			{
				SecureString inputString = ConvertToSecureString( testString );
				String encrypted = CryptographyUtils.EncryptSecureString( inputString );
				SecureString decrypted = CryptographyUtils.DecryptSecureString( encrypted );

				String finalString;
				IntPtr unmanagedString = IntPtr.Zero;
				try
				{
					unmanagedString = Marshal.SecureStringToGlobalAllocUnicode( decrypted );
					finalString = Marshal.PtrToStringUni( unmanagedString );
				}
				finally
				{
					Marshal.ZeroFreeGlobalAllocUnicode( unmanagedString );
				}

				Assert.AreEqual( testString, finalString );
			}
		}

		static SecureString ConvertToSecureString( String str )
		{
			SecureString secureString = new SecureString();
			foreach ( Char character in str )
			{
				secureString.AppendChar( character );
			}
			return secureString;
		}

		static readonly String[] _testStrings =
		{
			"Hello World",
			"Alexandria",
			"Archive of Our Own",
			"Cryptography",
			"S73R3k!"
		};
	}
}
