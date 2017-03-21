using System;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.RequestHandles
{
	internal sealed class AO3SeriesRequestHandle : ISeriesRequestHandle
	{
		public AO3SeriesRequestHandle( String title )
		{
			Title = title;
		}

		public String Title { get; }
	}
}
