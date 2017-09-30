// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Alexandria.Tests.AO3
{
    [SuppressMessage( "ReSharper", "InconsistentNaming", Justification = "Allowing variable names of tags to break naming convention but keep the variable as close to canonical tag text as possible for readibility." )]
    internal static class UnitTestConstants
    {
        public const string FullFanficParsingTestsCategory = "Full Fanfic Parsing (AO3)";
        public const string FullAuthorParsingTestsCategory = "Full Author Parsing (AO3)";
        public const string FullTagParsingTestsCategory = "Full Tag Parsing (AO3)";
        public const string FullSeriesParsingTestsCategory = "Full Series Parsing (AO3)";
        public const string FanficParsingTestsCategory = "Fanfic Parsing (AO3)";
        public const string UtilTestsCategory = "Utils (AO3)";
        public const string QueryResultsTestsCategory = "Query Results (AO3)";

        public const string FicHandlePrinceAmongWolves = "538425";
        public const string FicHandlePossibilityOfSilence = "3592305";
        public const string FicHandleItsNotMyLovestory = "6598738";
        public const string FicHandleTwoWeeksSleepIsADistantDream = "1050114";

        public const string AuthorUsernameCrossroadswrite = "crossroadswrite";

        public const string TagStilesStilinski = "Stiles Stilinski";
        public const string TagPOVJackFrost = "POV Jack Frost (Guardians of Childhood)";

        public const string ShipSterek = "Derek Hale/Stiles Stilinski";

        public const string SeriesHandleBodiceRipperVerse = "100664";
        public const string SeriesHandleJanuaryJackrabbitWeek2014 = "69272";

        public static readonly string[] TagPOVJackFrostFanficHandles =
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
