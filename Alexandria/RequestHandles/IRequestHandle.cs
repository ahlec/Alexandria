using System;

namespace Alexandria.RequestHandles
{
	public interface IRequestHandle<T> where T : IRequestable
	{
	}
}
