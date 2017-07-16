using System;
using System.Diagnostics.CodeAnalysis;

namespace Alexandria.Tests.AO3
{
	[SuppressMessage( "ReSharper", "InconsistentNaming" )]
	internal static class UnitTestConstants
	{
		public const String FullFanficParsingTestsCategory = "AO3 - Full Fanfic Parsing";
		public const String FullAuthorParsingTestsCategory = "AO3 - Full Author Parsing";
		public const String FullTagParsingTestsCategory = "AO3 - Full Tag Parsing";
		public const String FullSeriesParsingTestsCategory = "AO3 - Full Series Parsing";
		public const String FanficParsingTestsCategory = "AO3 - Fanfic Parsing";
		public const String UtilTestsCategory = "AO3 - Utils";
		public const String QueryResultsTestsCategory = "AO3 - Query Results";

		public const String FicHandle_PrinceAmongWolves = "538425";
		public const String FicHandle_PossibilityOfSilence = "3592305";
		public const String FicHandle_ItsNotMyLovestory = "6598738";
		public const String FicHandle_Homesick = "10317524";

		public const String AuthorUsername_Crossroadswrite = "crossroadswrite";

		public const String Tag_StilesStilinski = "Stiles Stilinski";
		public const String Tag_POVJackFrost = "POV Jack Frost (Guardians of Childhood)";

		public const String Ship_Sterek = "Derek Hale/Stiles Stilinski";

		public const String SeriesHandle_BodiceRipperVerse = "100664";
		public const String SeriesHandle_JanuaryJackrabbitWeek2014 = "69272";

		public static readonly String[] Tag_POVJackFrostFanficHandles =
		{
			"10659105",
			"7367401",
			"1312102",
			"5663545",
			"5480645",
			"4084660",
			"2662784",
			"1068792",
			"1068772"
		};
	}
}
