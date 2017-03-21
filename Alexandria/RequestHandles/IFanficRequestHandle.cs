using System;
using Alexandria.Model;

namespace Alexandria.RequestHandles
{
	public interface IFanficRequestHandle : IRequestHandle<IFanfic>
	{
		String Handle { get; }
	}
}
