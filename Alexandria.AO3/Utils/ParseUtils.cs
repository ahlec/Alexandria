using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3.Utils
{
	internal static class ParseUtils
	{
		public static MaturityRating ParseMaturityRatingFromAO3( String name )
		{
			switch ( name.ToLowerInvariant() )
			{
				case "general audiences":
					return MaturityRating.General;
				case "teen and up audiences":
					return MaturityRating.Teen;
				case "mature":
					return MaturityRating.Adult;
				case "explicit":
					return MaturityRating.Explicit;
				case "not rated":
					return MaturityRating.NotRated;
				default:
					throw new ArgumentException( $"Unknown AO3 maturity rating: {name}", nameof( name ) );
			}
		}

		public static ContentWarnings ParseContentWarningsFromAO3( HtmlNode list )
		{
			ContentWarnings parsed = ContentWarnings.None;

			foreach ( HtmlNode li in list.Elements( "li" ) )
			{
				String tag = li.FirstChild.InnerText;
				switch ( tag.ToLowerInvariant() )
				{
					case "no archive warnings apply":
						{
							parsed |= ContentWarnings.None;
							break;
						}
					case "creator chose not to use archive warnings":
						{
							parsed |= ContentWarnings.Undetermined;
							break;
						}
					case "graphic depictions of violence":
						{
							parsed |= ContentWarnings.Violence;
							break;
						}
					case "major character death":
						{
							parsed |= ContentWarnings.MajorCharacterDeath;
							break;
						}
					case "rape/non-con":
						{
							parsed |= ContentWarnings.Rape;
							break;
						}
					case "underage":
						{
							parsed |= ContentWarnings.Underage;
							break;
						}
					default:
						throw new ArgumentException( $"Unable to parse the built-in AO3 content warning tag for '{tag.ToLowerInvariant()}'" );
				}
			}

			return parsed;
		}

		public static IReadOnlyList<IRequestHandle<ICharacter>> ParseShipCharacters( String shipTag, out ShipType type )
		{
			foreach ( KeyValuePair<String, ShipType> separator in _shipNameSeparators )
			{
				Int32 currentStartIndex = 0;
				Int32 nextSeparatorIndex = shipTag.IndexOf( separator.Key, currentStartIndex );
				if ( nextSeparatorIndex < 0 )
				{
					continue;
				}


				List<IRequestHandle<ICharacter>> characters = new List<IRequestHandle<ICharacter>>();

				while ( nextSeparatorIndex >= 0 )
				{
					String character = shipTag.Substring( currentStartIndex, nextSeparatorIndex - currentStartIndex );
					characters.Add( new AO3CharacterRequestHandle( character.Trim() ) );
					currentStartIndex = nextSeparatorIndex + separator.Key.Length;
					nextSeparatorIndex = shipTag.IndexOf( separator.Key, currentStartIndex );
				}

				if ( currentStartIndex < shipTag.Length - 1 )
				{
					characters.Add( new AO3CharacterRequestHandle( shipTag.Substring( currentStartIndex ).Trim() ) );
				}

				type = separator.Value;
				return characters;
			}

			type = ShipType.Unknown;
			return new List<IRequestHandle<ICharacter>>();
		}

		public static String ReadableInnerText( this HtmlNode node )
		{
			String text = node.InnerText;
			return text.Replace( "&amp;", "&" );
		}

		static readonly ImmutableDictionary<String, ShipType> _shipNameSeparators = new Dictionary<String, ShipType>
		{
			{ "/", ShipType.Romantic },
			{ "\\", ShipType.Romantic },
			{ "&", ShipType.Platonic }
		}.ToImmutableDictionary();
	}
}
