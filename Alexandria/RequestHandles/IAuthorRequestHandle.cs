using System;
using Alexandria.Model;

namespace Alexandria.RequestHandles
{
	public interface IAuthorRequestHandle : IRequestHandle<IAuthor>
	{
		String Username { get; }
	}
}
