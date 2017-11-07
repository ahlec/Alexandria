// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://github.com/ahlec/Alexandria).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Alexandria.Utils;

namespace Alexandria.AO3
{
    public static class AO3Validation
    {
        /// <summary>
        /// Determines if the provided string can potentially be a valid author, or if there is a error with its
        /// composition that would produce an error.
        /// <para />
        /// NOTE: This doesn't actually confirm if there is an author with this name. This function merely validates
        /// whether the proposed name is valid to use as the name of an author.
        /// </summary>
        /// <param name="name">The proposed author name that should be validated by this function.</param>
        /// <returns>If the name is valid, true; if the name has one or more errors, then false.</returns>
        public static bool IsValidAuthorName( string name )
        {
            return IsValidAuthorName( name, out string _ );
        }

        /// <summary>
        /// Determines if the provided string can potentially be a valid author, or if there is a error with its
        /// composition that would produce an error. This overload will provide a human readable reason as to
        /// what the error is.
        /// <para />
        /// NOTE: This doesn't actually confirm if there is an author with this name. This function merely validates
        /// whether the proposed name is valid to use as the name of an author.
        /// </summary>
        /// <param name="name">The proposed author name that should be validated by this function.</param>
        /// <param name="reason">The first reason why the name is invalid. Should the name be valid (return value of true), this will be null.</param>
        /// <returns>If the name is valid, true; if the name has one or more errors, then false.</returns>
        //
        // Reference: owarchive/app/models/pseud.rb as of f872a7599774cd5da29e700c35c7e280ca70f485
        public static bool IsValidAuthorName( string name, out string reason )
        {
            if ( !IsValidStringSetup( name, MinNameLength, MaxNameLength, out string setupReason ) )
            {
                reason = setupReason;
                return false;
            }

            bool hasNumberOrLetter = false;
            foreach ( char character in name )
            {
                if ( !IsValidCharacterForAuthorName( character ) )
                {
                    reason = $"Name contains '{character}', which is an invalid character for an author name";
                    return false;
                }

                if ( char.IsLetterOrDigit( character ) )
                {
                    hasNumberOrLetter = true;
                }
            }

            if ( !hasNumberOrLetter )
            {
                reason = "Name must contain at least one letter or digit.";
                return false;
            }

            reason = null;
            return true;
        }

        /// <summary>
        /// Determines if the provided string is a valid tag, or if there is an error with its composition that would
        /// produce an error if attempted to be used as a tag.
        /// </summary>
        /// <param name="tag">The proposed tag that should be validated by this function.</param>
        /// <returns>If the tag is valid, true; if the tag has one or more errors, then false.</returns>
        public static bool IsValidTag( string tag )
        {
            return IsValidTag( tag, out string _ );
        }

        /// <summary>
        /// Determines if the provided string is a valid tag, or if there is an error with its composition that would
        /// produce an error if attempted to be used as a tag. This overload will provide a human readable reason as to
        /// what the error is.
        /// </summary>
        /// <param name="tag">The proposed tag that should be validated by this function.</param>
        /// <param name="reason">The first reason why the tag is invalid. Should the tag be valid (return value of true), this will be null.</param>
        /// <returns>If the tag is valid, true; if the tag has one or more errors, then false.</returns>
        //
        // Reference: owarchive/app/models/tag.rb as of c27d54cf14a3356e296bd1f0c4ca3796eb0076fa
        public static bool IsValidTag( string tag, out string reason )
        {
            if ( !IsValidStringSetup( tag, MinTagLength, MaxTagLength, out string setupReason ) )
            {
                reason = setupReason;
                return false;
            }

            if ( tag.Any( character => _invalidTagCharacters.Contains( character ) ) )
            {
                reason = $"Tags cannot contain any of the following characters: {string.Join( " ", _invalidTagCharacters )}";
                return false;
            }

            reason = null;
            return true;
        }

        static bool IsValidStringSetup( string str, int minLength, int maxLength, out string reason )
        {
            if ( str == null )
            {
                reason = "No string ws provided (input was null).";
                return false;
            }

            if ( str.Length < minLength )
            {
                reason = $"The length of the provided string ({str.Length}) is less than the minimum length required ({minLength}).";
                return false;
            }

            if ( str.Length > maxLength )
            {
                reason = $"The length of the provided string ({str.Length}) is greater than the maximum length allowed ({maxLength}).";
                return false;
            }

            reason = null;
            return true;
        }

        static bool IsValidCharacterForAuthorName( char character )
        {
            if ( char.IsLetterOrDigit( character ) )
            {
                return true;
            }

            if ( CharUtils.IsSpace( character ) )
            {
                return true;
            }

            if ( character == '_' )
            {
                return true;
            }

            if ( CharUtils.IsDash( character ) )
            {
                return true;
            }

            return false;
        }

        const int MinNameLength = 1;
        const int MaxNameLength = 40;

        /// <summary>
        /// The minimum length of a valid tag.
        /// </summary>
        const int MinTagLength = 1;

        /// <summary>
        /// The maximum length of a valid tag.
        /// </summary>
        const int MaxTagLength = 100;

        static readonly HashSet<char> _invalidTagCharacters = new HashSet<char> { ',', '*', '<', '>', '^', '{', '}', '=', '`', '\\', '%' };
    }
}
