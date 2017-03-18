using System;
using Alexandria.Model;

namespace Alexandria.AO3.RequestHandles
{
	public sealed class AO3FanficRequestHandle : IRequestHandle<IFanfic>
	{
		public AO3FanficRequestHandle( String handle )
		{
			Handle = handle;
		}

		public String Handle { get; }
	}
}
