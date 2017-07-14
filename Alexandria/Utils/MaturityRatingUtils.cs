using System;
using Alexandria.Model;

namespace Alexandria.Utils
{
	public static class MaturityRatingUtils
	{
		public static String GetDisplayName( this MaturityRating rating )
		{
			switch ( rating )
			{
				case MaturityRating.NotRated:
					return "Not Rated";
				default:
					return rating.ToString();
			}
		}
	}
}
