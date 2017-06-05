using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

// The SecureString functions were written with a lot of help from http://www.obviex.com/samples/dpapi.aspx, pinvoke.net, http://www.griffinscs.com/?p=12, and a whole bunch of SO questions

namespace Bibliothecary.Core.Utils
{
	public static class CryptographyUtils
	{
		[DllImport( "crypt32.dll", SetLastError = true, CharSet = CharSet.Auto )]
		static extern Boolean CryptProtectData( ref DATA_BLOB pPlainText, String szDescription, ref DATA_BLOB pEntropy, IntPtr pReserved, ref CRYPTPROTECT_PROMPTSTRUCT pPrompt, Int32 dwFlags, ref DATA_BLOB pCipherText );

		[DllImport( "crypt32.dll", SetLastError = true, CharSet = CharSet.Auto )]
		static extern Boolean CryptUnprotectData( ref DATA_BLOB pCipherText, String pszDescription, ref DATA_BLOB pEntropy, IntPtr pReserved, ref CRYPTPROTECT_PROMPTSTRUCT pPrompt, Int32 dwFlags, ref DATA_BLOB pPlainText );

		[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Unicode )]
		// ReSharper disable once InconsistentNaming
		struct DATA_BLOB
		{
			public Int32 cbData;
			public IntPtr pbData;
		}

		[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Unicode )]
		// ReSharper disable once InconsistentNaming
		struct CRYPTPROTECT_PROMPTSTRUCT
		{
			public Int32 cbSize;
			public Int32 dwPromptFlags;
			public IntPtr hwndApp;
			public String szPrompt;
		}

		public static String EncryptString( String text )
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

		public static String EncryptSecureString( SecureString text )
		{
			CRYPTPROTECT_PROMPTSTRUCT prompt = new CRYPTPROTECT_PROMPTSTRUCT
			{
				cbSize = Marshal.SizeOf( typeof( CRYPTPROTECT_PROMPTSTRUCT ) ),
				dwPromptFlags = 0,
				hwndApp = (IntPtr) 0,
				szPrompt = null
			};

			IntPtr unmanagedString = Marshal.SecureStringToBSTR( text );
			DATA_BLOB entropyBlob = new DATA_BLOB();
			DATA_BLOB cipherBlob = new DATA_BLOB();
			try
			{
				DATA_BLOB plaintextBlob = new DATA_BLOB
				{
					pbData = unmanagedString,
					cbData = text.Length * 2 // unicode, so occupies two bytes
				};

				entropyBlob.pbData = Marshal.AllocHGlobal( _additionalEntropy.Length );
				entropyBlob.cbData = _additionalEntropy.Length;
				Marshal.Copy( _additionalEntropy, 0, entropyBlob.pbData, _additionalEntropy.Length );

				Boolean wasSuccess = CryptProtectData( ref plaintextBlob, null, ref entropyBlob, IntPtr.Zero, ref prompt, CRYPTPROTECT_UI_FORBIDDEN | CRYPTPROTECT_LOCAL_MACHINE, ref cipherBlob );
				if ( !wasSuccess )
				{
					Int32 errorCode = Marshal.GetLastWin32Error();
					throw new ApplicationException( "CryptProtectData failed", new Win32Exception( errorCode ) );
				}

				Byte[] cipherTextBytes = new Byte[cipherBlob.cbData];
				Marshal.Copy( cipherBlob.pbData, cipherTextBytes, 0, cipherBlob.cbData );
				String encrypted = Convert.ToBase64String( cipherTextBytes );
				return encrypted;
			}
			finally
			{
				Marshal.ZeroFreeBSTR( unmanagedString );

				if ( entropyBlob.pbData != IntPtr.Zero )
				{
					Marshal.FreeHGlobal( entropyBlob.pbData );
				}

				if ( cipherBlob.pbData != IntPtr.Zero )
				{
					Marshal.FreeHGlobal( cipherBlob.pbData );
				}
			}
		}

		public static String DecryptString( String text )
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

		public static SecureString DecryptSecureString( String encryptedText )
		{
			if ( encryptedText == null )
			{
				throw new ArgumentNullException( nameof( encryptedText ) );
			}

			CRYPTPROTECT_PROMPTSTRUCT prompt = new CRYPTPROTECT_PROMPTSTRUCT
			{
				cbSize = Marshal.SizeOf( typeof( CRYPTPROTECT_PROMPTSTRUCT ) ),
				dwPromptFlags = 0,
				hwndApp = (IntPtr) 0,
				szPrompt = null
			};

			DATA_BLOB cipherBlob = new DATA_BLOB();
			DATA_BLOB entropyBlob = new DATA_BLOB();
			DATA_BLOB plaintextBlob = new DATA_BLOB();
			try
			{
				Byte[] encrypted = Convert.FromBase64String( encryptedText );
				cipherBlob.pbData = Marshal.AllocHGlobal( encrypted.Length );
				cipherBlob.cbData = encrypted.Length;
				Marshal.Copy( encrypted, 0, cipherBlob.pbData, encrypted.Length );

				entropyBlob.pbData = Marshal.AllocHGlobal( _additionalEntropy.Length );
				entropyBlob.cbData = _additionalEntropy.Length;
				Marshal.Copy( _additionalEntropy, 0, entropyBlob.pbData, _additionalEntropy.Length );

				Boolean wasSuccess = CryptUnprotectData( ref cipherBlob, null, ref entropyBlob, IntPtr.Zero, ref prompt, CRYPTPROTECT_UI_FORBIDDEN | CRYPTPROTECT_LOCAL_MACHINE, ref plaintextBlob );
				if ( !wasSuccess )
				{
					Int32 errorCode = Marshal.GetLastWin32Error();
					throw new ApplicationException( "CryptUnprotectData failed", new Win32Exception( errorCode ) );
				}

				SecureString decrypted = new SecureString();
				List<Char> CHARS = new List<Char>();
				for ( Int32 pointerOffset = 0; pointerOffset < plaintextBlob.cbData; pointerOffset += 2 ) // unicode, so occupies two bytes
				{
					Char characterAtOffset = (Char) Marshal.ReadByte( plaintextBlob.pbData, pointerOffset );
					decrypted.AppendChar( characterAtOffset );
					CHARS.Add( characterAtOffset );
				}
				return decrypted;
			}
			finally
			{
				if ( cipherBlob.pbData != IntPtr.Zero )
				{
					Marshal.FreeHGlobal( cipherBlob.pbData );
				}

				if ( entropyBlob.pbData != IntPtr.Zero )
				{
					Marshal.FreeHGlobal( entropyBlob.pbData );
				}

				if ( plaintextBlob.pbData != IntPtr.Zero )
				{
					Marshal.FreeHGlobal( plaintextBlob.pbData );
				}
			}
		}

		static readonly Byte[] _additionalEntropy = { 17, 131, 1, 152, 37, 245, 155, 17 };
		const Int32 CRYPTPROTECT_UI_FORBIDDEN = 0x1;
		const Int32 CRYPTPROTECT_LOCAL_MACHINE = 0x4;
	}
}
