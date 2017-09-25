// -----------------------------------------------------------------------
// This code is part of the Alexandria project (https://bitbucket.org/ahlec/alexandria/).
// Written and maintained by Alec Deitloff.
// Archive of Our Own (https://archiveofourown.org) is owned by the Organization for Transformative Works (http://www.transformativeworks.org/).
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Alexandria.AO3.RequestHandles;
using Alexandria.Model;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.Utils
{
    internal static class ParseUtils
    {
        public static IReadOnlyList<ICharacterRequestHandle> ParseShipCharacters( AO3Source source, string shipTag, out ShipType type )
        {
            foreach ( KeyValuePair<string, ShipType> separator in _shipNameSeparators )
            {
                int currentStartIndex = 0;
                int nextSeparatorIndex = shipTag.IndexOf( separator.Key, currentStartIndex, StringComparison.InvariantCultureIgnoreCase );
                if ( nextSeparatorIndex < 0 )
                {
                    continue;
                }

                List<ICharacterRequestHandle> characters = new List<ICharacterRequestHandle>();

                while ( nextSeparatorIndex >= 0 )
                {
                    string character = shipTag.Substring( currentStartIndex, nextSeparatorIndex - currentStartIndex );
                    characters.Add( new AO3CharacterRequestHandle( source, character.Trim() ) );
                    currentStartIndex = nextSeparatorIndex + separator.Key.Length;
                    nextSeparatorIndex = shipTag.IndexOf( separator.Key, currentStartIndex, StringComparison.InvariantCultureIgnoreCase );
                }

                if ( currentStartIndex < shipTag.Length - 1 )
                {
                    characters.Add( new AO3CharacterRequestHandle( source, shipTag.Substring( currentStartIndex ).Trim() ) );
                }

                type = separator.Value;
                return characters;
            }

            type = ShipType.Unknown;
            return new List<ICharacterRequestHandle>();
        }

        public static IEnumerable<Tuple<string, HtmlNode>> EnumerateDlTable( this HtmlNode dl )
        {
            string lastDtText = null;
            foreach ( HtmlNode child in dl.ChildNodes )
            {
                if ( child.Name.Equals( "dt" ) )
                {
                    lastDtText = child.InnerText.Trim().TrimEnd( ':' );
                    continue;
                }

                if ( !child.Name.Equals( "dd" ) )
                {
                    continue;
                }

                yield return new Tuple<string, HtmlNode>( lastDtText, child );
                lastDtText = null;
            }

            if ( lastDtText != null )
            {
                yield return new Tuple<string, HtmlNode>( lastDtText, null );
            }
        }

        static readonly IReadOnlyDictionary<string, ShipType> _shipNameSeparators = new Dictionary<string, ShipType>
        {
            { "/", ShipType.Romantic },
            { "\\", ShipType.Romantic },
            { "&", ShipType.Platonic }
        };
    }
}
