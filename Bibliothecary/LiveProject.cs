using System;
using Bibliothecary.Data;

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

		public TimeSpan TimeRemaining { get; private set; }

		public Boolean IsTimeToSearch { get; private set; }

		public void ResetCountdown()
		{
			IsTimeToSearch = false;
			TimeRemaining = Project.UpdateFrequency;
		}

		public void Update( TimeSpan elapsedTime )
		{
			if ( IsTimeToSearch )
			{
				return;
			}

			if ( elapsedTime > TimeRemaining )
			{
				IsTimeToSearch = true;
				return;
			}

			TimeRemaining -= elapsedTime;
		}
	}
}
