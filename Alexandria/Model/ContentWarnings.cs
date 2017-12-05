// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;

namespace Alexandria.Model
{
    /// <summary>
    /// A set of flags detailing major content warnings around things in fanfics that
    /// are generally considered either triggering or indicative of content some people
    /// might find unsuitable.
    /// </summary>
    [Flags]
    public enum ContentWarnings
    {
        /// <summary>
        /// The author(s) has made an explicit statement that there are no content warnings
        /// that apply to this fanfic.
        /// </summary>
        None = 0,

        /// <summary>
        /// The author(s) has made NO STATEMENT as to whether there are any content warnings
        /// that would apply to this fanfic.
        /// </summary>
        Undetermined = 1,

        /// <summary>
        /// This fanfic features explicit violence or gore.
        /// </summary>
        Violence = 2,

        /// <summary>
        /// This fanfic features one (or more) major characters dying.
        /// </summary>
        MajorCharacterDeath = 4,

        /// <summary>
        /// This fanfic features rape, sexual assault, or other non-consentual sexual acts.
        /// </summary>
        Rape = 8,

        /// <summary>
        /// This fanfic features minors/underage characters engaging in sexual acts, either with
        /// other minors or with non-minors, either consentually or non-consentually.
        /// </summary>
        Underage = 16
    }
}
