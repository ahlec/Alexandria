using System;
using System.Collections.Generic;
using Alexandria.RequestHandles;

namespace Alexandria.Model
{
	public interface ITag : IRequestable
	{
		String Text { get; }

		IReadOnlyList<ITagRequestHandle> ParentTags { get; }

		IReadOnlyList<ITagRequestHandle> SynonymousTags { get; }
	}
}
