using System;
using System.ServiceModel;
using Bibliothecary.Core;

namespace Bibliothecary
{
	[ServiceBehavior( InstanceContextMode = InstanceContextMode.Single )]
	public sealed class BibliothecaryService : IBibliothecaryService
	{
		public void RestartProjectTimer()
		{
		}

		public void RefreshProjects()
		{
		}

		public void AggregateProjectsNow()
		{
		}

		public void MarkTimeElapsed( Double time )
		{
		}
	}
}
