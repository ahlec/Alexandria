using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
	public interface IAuthor : IRequestable
	{
		String Name { get; }

		IReadOnlyList<String> Nicknames { get; }

		DateTime DateJoined { get; }

		String Location { get; }

		DateTime? Birthday { get; }

		String Biography { get; }

		IReadOnlyList<IFanficRequestHandle> Works { get; }
	}
}
