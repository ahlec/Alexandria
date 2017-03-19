using System;

namespace Alexandria.Model
{
	public interface ITag
	{
		String Text { get; }

		IRequestHandle<ITagInfo> Info { get; }
	}
}
