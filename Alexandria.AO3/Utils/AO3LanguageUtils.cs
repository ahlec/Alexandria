using System;
using Alexandria.Model;

namespace Alexandria.AO3.Utils
{
	public static class AO3LanguageUtils
	{
		class LanguageJump
		{
			public LanguageJump( Language language, Int32 id )
			{
				LanguageEnumId = (Int32) language;
				Id = id;
			}

			public Int32 LanguageEnumId { get; }

			public Int32 Id { get; }
		}

		public static Int32 GetId( Language language )
		{
			Int32 inputLanguageEnumId = (Int32) language;
			foreach ( LanguageJump jump in _languageJumps )
			{
				Int32 differenceFromJump = inputLanguageEnumId - jump.LanguageEnumId;
				if ( differenceFromJump < 0 )
				{
					continue;
				}

				return jump.Id + differenceFromJump;
			}

			throw new ApplicationException( "There must always be a base language jump (should be English)" );
		}

		static readonly LanguageJump[] _languageJumps =
		{
			new LanguageJump( Language.Bosnian, 82 ),
			new LanguageJump( Language.Bengali, 79 ),
			new LanguageJump( Language.Sindarin, 75 ),
			new LanguageJump( Language.Slovene, 72 ),
			new LanguageJump( Language.OldIcelandicMaybeIdk, 69 ),
			new LanguageJump( Language.Somali, 64 ),
			new LanguageJump( Language.Soko, 61 ),
			new LanguageJump( Language.Swahili, 58 ),
			new LanguageJump( Language.Malaysian, 56 ),
			new LanguageJump( Language.Slovak, 53 ),
			new LanguageJump( Language.English, 1 )
		};
	}
}
