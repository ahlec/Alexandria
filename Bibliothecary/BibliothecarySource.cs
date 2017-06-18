using System;
using System.Collections.Generic;
using Alexandria;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Bibliothecary.Core;
using Bibliothecary.Core.Publishing;
using NLog;

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

		public void ProcessProject( Project project, Database database, Logger log, Int32 maxResultsMultiplier )
		{
			if ( maxResultsMultiplier <= 0 )
			{
				throw new ArgumentOutOfRangeException( nameof( maxResultsMultiplier ) );
			}

			EmailClient emailClient = null;
			if ( project.PublishingInfo.UsesEmail )
			{
				emailClient = project.PublishingInfo.CreateEmailClient();
				log.Info( $"-> Set up {nameof( emailClient )} to send to {project.PublishingInfo.RecipientEmail}" );
			}
			TumblrClient tumblrClient = null;
			if ( project.PublishingInfo.UsesTumblr )
			{
				tumblrClient = project.PublishingInfo.CreateTumblrClient();
				log.Info( $"-> Set up {nameof( tumblrClient )} to post to {project.PublishingInfo.TumblrBlogName}" );
			}

			// Check to see if we have any publishing options configured. If we don't, then don't do anything (project not fully configured)
			if ( emailClient == null && tumblrClient == null )
			{
				log.Warn( "-> Not supposed to publish to either email or tumblr, so stopping before searching." );
				return;
			}

			IQueryResultsPage<IFanfic, IFanficRequestHandle> results = Source.Search( project.SearchQuery );
			Int32 numberFanficsProcessed = 0;
			Int32 maxNumberOfResults = project.MaxResultsPerSearch * maxResultsMultiplier;
			ProcessProjectResults( project, results, emailClient, tumblrClient, log, maxNumberOfResults, ref numberFanficsProcessed );
			while ( numberFanficsProcessed < maxNumberOfResults && results.HasMoreResults )
			{
				results = results.RetrieveNextPage();
				ProcessProjectResults( project, results, emailClient, tumblrClient, log, maxNumberOfResults, ref numberFanficsProcessed );
			}
			log.Info( $"-> Processed a total of {numberFanficsProcessed} result(s)" );
		}

		void ProcessProjectResults( Project project, IQueryResultsPage<IFanfic, IFanficRequestHandle> results, EmailClient emailClient, TumblrClient tumblrClient, Logger log, Int32 maxNumberOfResults, ref Int32 numberFanficsProcessed )
		{
			List<IFanficRequestHandle> reportedFanfics = new List<IFanficRequestHandle>();
			foreach ( IFanficRequestHandle fanficHandle in project.FilterUnreportedQueryResults( results.Results, DatabaseHandle ) )
			{
				if ( numberFanficsProcessed >= maxNumberOfResults )
				{
					return;
				}

				IFanfic fanfic = Source.MakeRequest( fanficHandle );

				Boolean didSuccessfullyReport = true;
				try
				{
					//emailClient?.SendMail( fanfic );
					tumblrClient?.Post( fanfic, Source );
				}
				catch ( Exception ex )
				{
					log.Warn( $"-> Encountered {ex.GetType().Name} exception when processing {DatabaseHandle} fanfic {fanficHandle.Handle}: {ex.Message}" );
					didSuccessfullyReport = false;
				}

				if ( didSuccessfullyReport )
				{
					log.Info( $"-> Reported {DatabaseHandle} fanfic {fanficHandle.Handle}: {fanfic.Title}" );
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
