using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Model;

namespace Alexandria.AO3
{
	public sealed class AO3SeriesRequestHandle : IRequestHandle<ISeries>
	{
		public AO3SeriesRequestHandle( String handle )
		{
			Handle = handle;
		}

		public String Handle { get; }
	}
}
