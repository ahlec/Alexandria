using System.ServiceModel;

namespace Bibliothecary.Core
{
	[ServiceContract]
	public interface IBibliothecaryService
	{
		[OperationContract]
		void RestartProjectTimer();

		[OperationContract]
		void RefreshProjects();

		[OperationContract]
		void AggregateProjectsNow();
	}
}
