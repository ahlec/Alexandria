using System;
using System.Linq;
using Alexandria;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Bibliothecary.Core;

namespace Bibliothecary
{
	public sealed class BibliothecarySource
	{
		public BibliothecarySource( LibrarySource source, String databaseHandle )
		{
			Source = source;
			DatabaseHandle = databaseHandle;
		}

		public LibrarySource Source { get; }

		public String DatabaseHandle { get; }

		public void ProcessProject( Project project, Database database )
		{
			IQueryResultsPage<IFanfic, IFanficRequestHandle> results = Source.Search( project.SearchQuery );
			Int32 numberFanficsProcessed = 0;
			ProcessProjectResults( project, results, ref numberFanficsProcessed );
			while ( numberFanficsProcessed < project.MaxResultsPerSearch && results.HasMoreResults )
			{
				results = results.RetrieveNextPage();
				ProcessProjectResults( project, results, ref numberFanficsProcessed );
			}
		}

		void ProcessProjectResults( Project project, IQueryResultsPage<IFanfic, IFanficRequestHandle> results, ref Int32 numberFanficsProcessed )
		{
			foreach ( IFanficRequestHandle fanfic in project.FilterUnreportedQueryResults( results.Results, DatabaseHandle ) )
			{
				if ( numberFanficsProcessed >= project.MaxResultsPerSearch )
				{
					return;
				}

				++numberFanficsProcessed;
			}
		}
	}
}
