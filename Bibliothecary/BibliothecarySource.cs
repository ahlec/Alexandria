using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Bibliothecary.Core;
using Bibliothecary.Core.Publishing;

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
			EmailClient emailClient = null;
			if ( project.PublishingInfo.UsesEmail )
			{
				emailClient = new EmailClient
				{
					FromEmail = project.PublishingInfo.SenderEmail,
					Host = project.PublishingInfo.SenderHost,
					Port = project.PublishingInfo.SenderPort,
					EnableSsl = project.PublishingInfo.DoesSenderUseSsl,
					ToEmail = project.PublishingInfo.RecipientEmail
				};

				if ( project.PublishingInfo.DoesSenderRequireCredentials )
				{
					emailClient.SetCredentials( project.PublishingInfo.SenderUsername, project.PublishingInfo.SenderPassword );
				}
			}
			// Check to see if we have any publishing options configured. If we don't, then don't do anything (project not fully configured)
			if ( emailClient == null )
			{
				return;
			}

			IQueryResultsPage<IFanfic, IFanficRequestHandle> results = Source.Search( project.SearchQuery );
			Int32 numberFanficsProcessed = 0;
			ProcessProjectResults( project, results, emailClient, ref numberFanficsProcessed );
			while ( numberFanficsProcessed < project.MaxResultsPerSearch && results.HasMoreResults )
			{
				results = results.RetrieveNextPage();
				ProcessProjectResults( project, results, emailClient, ref numberFanficsProcessed );
			}
		}

		void ProcessProjectResults( Project project, IQueryResultsPage<IFanfic, IFanficRequestHandle> results, EmailClient emailClient, ref Int32 numberFanficsProcessed )
		{
			List<IFanficRequestHandle> reportedFanfics = new List<IFanficRequestHandle>();
			foreach ( IFanficRequestHandle fanficHandle in project.FilterUnreportedQueryResults( results.Results, DatabaseHandle ) )
			{
				if ( numberFanficsProcessed >= project.MaxResultsPerSearch )
				{
					return;
				}

				IFanfic fanfic = Source.MakeRequest( fanficHandle );

				Boolean didSuccessfullyReport = true;
				try
				{
					emailClient?.SendMail( fanfic );
				}
				catch ( Exception ex )
				{
					didSuccessfullyReport = false;
				}

				if ( didSuccessfullyReport )
				{
					reportedFanfics.Add( fanficHandle );
				}

				++numberFanficsProcessed;
			}

			if ( reportedFanfics.Count > 0 )
			{
				project.MarkFanficsAsReported( reportedFanfics, DatabaseHandle );
			}
		}
	}
}
