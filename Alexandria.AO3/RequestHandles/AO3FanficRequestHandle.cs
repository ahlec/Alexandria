using System;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.RequestHandles
{
	internal sealed class AO3FanficRequestHandle : IFanficRequestHandle
	{
		public AO3FanficRequestHandle( String handle )
		{
			Handle = handle;
		}

		public String Handle { get; }
	}
}
