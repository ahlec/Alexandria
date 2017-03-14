using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Model
{
	public interface ISeries
	{
		IAuthor Author { get; }

		DateTime DateTimeStartedUtc { get; }

		Boolean IsCompleted { get; }

		IFanfic FirstWork { get; }
	}
}
