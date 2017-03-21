using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Model;

namespace Alexandria.RequestHandles
{
	public interface ISeriesRequestHandle : IRequestHandle<ISeries>
	{
		String Title { get; }
	}
}
