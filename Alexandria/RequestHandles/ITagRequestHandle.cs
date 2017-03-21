using System;
using Alexandria.Model;

namespace Alexandria.RequestHandles
{
	public interface ITagRequestHandle : IRequestHandle<ITag>
	{
		String Text { get; }
	}
}
