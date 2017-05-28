using System;
using System.Security.Cryptography;
using System.Text;

namespace Bibliothecary.Data.Utils
{
	internal static class CryptographyUtils
	{
		public static String Encrypt( String text )
		{
			Byte[] bytes = Encoding.UTF8.GetBytes( text );
			Byte[] encrypted = ProtectedData.Protect( bytes, _additionalEntropy, DataProtectionScope.CurrentUser );
			String encryptedStr = Encoding.ASCII.GetString( encrypted );
			return encryptedStr;
		}

		public static String Decrypt( String text )
		{
			Byte[] encrypted = Encoding.ASCII.GetBytes( text );
			Byte[] decrypted = ProtectedData.Unprotect( encrypted, _additionalEntropy, DataProtectionScope.CurrentUser );
			String decryptedStr = Encoding.UTF8.GetString( decrypted );
			return decryptedStr;
		}

		static readonly Byte[] _additionalEntropy = { 17, 131, 1, 152, 37, 245, 155, 17 };
	}
}
