using System;
using System.Collections.Generic;
using Alexandria.Model;
using HtmlAgilityPack;

namespace Alexandria.AO3.Utils
{
	public static class AO3ContentWarningUtils
	{
		public static IEnumerable<Int32> GetIds( ContentWarnings warnings )
		{
			if ( warnings.HasFlag( ContentWarnings.Undetermined ) )
			{
				yield return 14;
			}
			if ( warnings.HasFlag( ContentWarnings.Violence ) )
			{
				yield return 17;
			}
			if ( warnings.HasFlag( ContentWarnings.MajorCharacterDeath ) )
			{
				yield return 18;
			}
			if ( warnings.HasFlag( ContentWarnings.None ) )
			{
				yield return 16;
			}
			if ( warnings.HasFlag( ContentWarnings.Rape ) )
			{
				yield return 19;
			}
			if ( warnings.HasFlag( ContentWarnings.Underage ) )
			{
				yield return 20;
			}
		}

		public static ContentWarnings Parse( HtmlNode list )
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
	}
}
