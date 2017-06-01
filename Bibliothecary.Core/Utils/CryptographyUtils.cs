using System;
using System.Security.Cryptography;
using System.Text;

namespace Bibliothecary.Core.Utils
{
	public static class CryptographyUtils
	{
		public static String Encrypt( String text )
		{
			if ( text == null )
			{
				throw new ArgumentNullException( nameof( text ) );
			}

			Byte[] bytes = Encoding.Unicode.GetBytes( text );
			Byte[] encrypted = ProtectedData.Protect( bytes, _additionalEntropy, DataProtectionScope.LocalMachine );
			String encryptedStr = Convert.ToBase64String( encrypted );
			return encryptedStr;
		}

		public static String Decrypt( String text )
		{
			if ( text == null )
			{
				throw new ArgumentNullException( nameof( text ) );
			}

			Byte[] encrypted = Convert.FromBase64String( text );
			Byte[] decrypted = ProtectedData.Unprotect( encrypted, _additionalEntropy, DataProtectionScope.LocalMachine );
			String decryptedStr = Encoding.Unicode.GetString( decrypted );
			return decryptedStr;
		}

		static readonly Byte[] _additionalEntropy = { 17, 131, 1, 152, 37, 245, 155, 17 };
	}
}
