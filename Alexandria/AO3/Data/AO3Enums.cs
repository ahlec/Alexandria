// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Alexandria.Model;

namespace Alexandria.AO3.Data
{
    internal static class AO3Enums
    {
        public static IReadOnlyList<AO3EnumDefinition<MaturityRating>> MaturityRatings { get; } = new List<AO3EnumDefinition<MaturityRating>>
        {
            new AO3EnumDefinition<MaturityRating>( MaturityRating.NotRated, 9, "not rated" ),
            new AO3EnumDefinition<MaturityRating>( MaturityRating.General, 10, "general audiences" ),
            new AO3EnumDefinition<MaturityRating>( MaturityRating.Teen, 11, "teen and up audiences" ),
            new AO3EnumDefinition<MaturityRating>( MaturityRating.Adult, 12, "mature" ),
            new AO3EnumDefinition<MaturityRating>( MaturityRating.Explicit, 13, "explicit" )
        };

        public static IReadOnlyList<AO3EnumDefinition<ContentWarnings>> ContentWarnings { get; } = new List<AO3EnumDefinition<ContentWarnings>>
        {
            new AO3EnumDefinition<ContentWarnings>( Alexandria.Model.ContentWarnings.Undetermined, 14, "creator chose not to use archive warnings" ),
            new AO3EnumDefinition<ContentWarnings>( Alexandria.Model.ContentWarnings.None, 16, "no archive warnings apply" ),
            new AO3EnumDefinition<ContentWarnings>( Alexandria.Model.ContentWarnings.Violence, 17, "graphic depictions of violence" ),
            new AO3EnumDefinition<ContentWarnings>( Alexandria.Model.ContentWarnings.MajorCharacterDeath, 18, "major character death" ),
            new AO3EnumDefinition<ContentWarnings>( Alexandria.Model.ContentWarnings.Rape, 19, "rape/non-con" ),
            new AO3EnumDefinition<ContentWarnings>( Alexandria.Model.ContentWarnings.Underage, 20, "underage" )
        };
    }
}
