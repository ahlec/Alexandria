using System;
using System.Text;

namespace Alexandria.Utils
{
	public static class StringBuilderExtensions
	{
		public static void TrimEnd( this StringBuilder builder )
		{
			if ( builder == null || builder.Length == 0 )
			{
				return;
			}

			Int32 numberCharactersToRemove = 0;
			for ( Int32 index = builder.Length - 1; index >= 0; --index )
			{
				if ( Char.IsWhiteSpace( builder[index] ) )
				{
					++numberCharactersToRemove;
				}
				else
				{
					break;
				}
			}

			if ( numberCharactersToRemove > 0 )
			{
				builder.Remove( builder.Length - numberCharactersToRemove, numberCharactersToRemove );
			}
		}

		public static void TrimStart( this StringBuilder builder )
		{
			if ( builder == null || builder.Length == 0 )
			{
				return;
			}

			Int32 numberCharactersToRemove = 0;
			for ( Int32 index = 0; index < builder.Length; ++index )
			{
				if ( Char.IsWhiteSpace( builder[index] ) )
				{
					++numberCharactersToRemove;
				}
				else
				{
					break;
				}
			}

			if ( numberCharactersToRemove > 0 )
			{
				builder.Remove( 0, numberCharactersToRemove );
			}
		}

		public static void Trim( this StringBuilder builder )
		{
			builder.TrimStart();
			builder.TrimEnd();
		}
	}
}
