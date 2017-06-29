using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3.Utils
{
	internal static class ParseUtils
	{
		public static IReadOnlyList<ICharacterRequestHandle> ParseShipCharacters( String shipTag, out ShipType type )
		{
			foreach ( KeyValuePair<String, ShipType> separator in _shipNameSeparators )
			{
				Int32 currentStartIndex = 0;
				Int32 nextSeparatorIndex = shipTag.IndexOf( separator.Key, currentStartIndex, StringComparison.InvariantCultureIgnoreCase );
				if ( nextSeparatorIndex < 0 )
				{
					continue;
				}


				List<ICharacterRequestHandle> characters = new List<ICharacterRequestHandle>();

				while ( nextSeparatorIndex >= 0 )
				{
					String character = shipTag.Substring( currentStartIndex, nextSeparatorIndex - currentStartIndex );
					characters.Add( new AO3CharacterRequestHandle( character.Trim() ) );
					currentStartIndex = nextSeparatorIndex + separator.Key.Length;
					nextSeparatorIndex = shipTag.IndexOf( separator.Key, currentStartIndex, StringComparison.InvariantCultureIgnoreCase );
				}

				if ( currentStartIndex < shipTag.Length - 1 )
				{
					characters.Add( new AO3CharacterRequestHandle( shipTag.Substring( currentStartIndex ).Trim() ) );
				}

				type = separator.Value;
				return characters;
			}

			type = ShipType.Unknown;
			return new List<ICharacterRequestHandle>();
		}

		public static IEnumerable<Tuple<String, HtmlNode>> EnumerateDlTable( this HtmlNode dl )
		{
			String lastDtText = null;
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

				yield return new Tuple<String, HtmlNode>( lastDtText, child );
				lastDtText = null;
			}

			if ( lastDtText != null )
			{
				yield return new Tuple<String, HtmlNode>( lastDtText, null );
			}
		}

		static readonly IReadOnlyDictionary<String, ShipType> _shipNameSeparators = new Dictionary<String, ShipType>
		{
			{ "/", ShipType.Romantic },
			{ "\\", ShipType.Romantic },
			{ "&", ShipType.Platonic }
		};
	}
}
