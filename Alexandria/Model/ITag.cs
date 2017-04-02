using System;
using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
	public interface ITag : IRequestable
	{
		TagType Type { get; }

		String Text { get; }

		IReadOnlyList<ITagRequestHandle> ParentTags { get; }

		IReadOnlyList<ITagRequestHandle> SynonymousTags { get; }

		IQueryResultsPage<IFanfic, IFanficRequestHandle> QueryFanfics();
	}
}
