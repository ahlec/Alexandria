using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.Utils;
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

		public static IReadOnlyList<ICharacterRequestHandle> ParseShipCharacters( String shipTag, out ShipType type )
		{
			foreach ( KeyValuePair<String, ShipType> separator in _shipNameSeparators )
			{
				Int32 currentStartIndex = 0;
				Int32 nextSeparatorIndex = shipTag.IndexOf( separator.Key, currentStartIndex );
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
			return new List<ICharacterRequestHandle>();
		}

		public static String ReadableInnerText( this HtmlNode node )
		{
			// Strip out all of the HTML tags, EXCEPT for <br> and <br /> and <p>, which should be transformed into newline characters
			String innerHtml = node.InnerHtml;
			StringBuilder builder = new StringBuilder( innerHtml.Length );
			Boolean currentlyInsideTag = false;
			Int32 currentTagIndex = 0;
			Boolean wasLinebreakTag = false;
			Char firstCharacterInTag = '\0';
			foreach ( Char character in innerHtml )
			{
				if ( character == '<' )
				{
					currentlyInsideTag = true;
					currentTagIndex = 0;
					wasLinebreakTag = false;
					continue;
				}
				else if ( character == '>' )
				{
					currentlyInsideTag = false;
					if ( wasLinebreakTag )
					{
						builder.AppendLine();
					}
					wasLinebreakTag = false;
					continue;
				}

				if ( !currentlyInsideTag )
				{
					builder.Append( character );
				}
				else
				{
					switch ( currentTagIndex )
					{
						case 0:
							{
								wasLinebreakTag = ( character == 'b' || character == 'B' || character == 'p' || character == 'P' );
								firstCharacterInTag = character;
								break;
							}
						case 1:
							{
								if ( wasLinebreakTag )
								{
									if ( firstCharacterInTag == 'b' || firstCharacterInTag == 'B' )
									{
										wasLinebreakTag = ( character == 'r' || character == 'R' );
									}
									else
									{
										wasLinebreakTag = Char.IsWhiteSpace( character );
									}
								}
								break;
							}
						default:
							{
								wasLinebreakTag = wasLinebreakTag && Char.IsWhiteSpace( character );
								break;
							}
					}
					currentTagIndex++;
				}
			}

			builder.Trim();

			String text = HttpUtility.HtmlDecode( builder.ToString() );
			return text;
		}

		static readonly ImmutableDictionary<String, ShipType> _shipNameSeparators = new Dictionary<String, ShipType>
		{
			{ "/", ShipType.Romantic },
			{ "\\", ShipType.Romantic },
			{ "&", ShipType.Platonic }
		}.ToImmutableDictionary();
	}
}
