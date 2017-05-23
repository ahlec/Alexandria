using System;
using System.Diagnostics;

namespace Bibliothecary.Extensions
{
	public static class EventLogExtensions
	{
		public static void WriteException( this EventLog log, Exception ex )
		{
			if ( ex == null )
			{
				log.WriteEntry( $"META ERROR: {nameof( ex )} was null when passed to {nameof( WriteException )}!" );
				return;
			}

			log.WriteEntry( $"Encountered {ex.GetType().Name} exception: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}" );
		}
	}
}
