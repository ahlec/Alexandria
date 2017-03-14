using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Model;
using HtmlAgilityPack;

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

			foreach ( HtmlNode li in list.ChildNodes )
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
	}
}
