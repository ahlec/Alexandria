using System;
using Bibliothecary.Core;

namespace Bibliothecary
{
	internal sealed class LiveProject
	{
		public LiveProject( Project project )
		{
			Project = project;
			ResetCountdown();
		}

		public Project Project { get; }

		public DateTime NextSearchTime { get; private set; }

		public Boolean IsTimeToSearch { get; private set; }

		/// <summary>
		/// Will usually be just 1. However, if the computer was asleep and we're WAAAAY past the time we should have searched
		/// next, then this'll be greater than 1.
		/// </summary>
		public Int32 NumberOfTimesToSearch { get; private set; }

		public void ResetCountdown()
		{
			IsTimeToSearch = false;
			NextSearchTime = DateTime.Now + Project.UpdateFrequency;
			NumberOfTimesToSearch = 0;
		}

		public void Update( TimeSpan elapsedTime )
		{
			if ( DateTime.Now < NextSearchTime )
			{
				IsTimeToSearch = false;
				NumberOfTimesToSearch = 0;
				return;
			}

			IsTimeToSearch = true;
			TimeSpan difference = DateTime.Now - NextSearchTime;
			NumberOfTimesToSearch = Math.Max( 1, (Int32) Math.Floor( difference.TotalMinutes / Project.UpdateFrequency.TotalMinutes ) );
		}
	}
}
