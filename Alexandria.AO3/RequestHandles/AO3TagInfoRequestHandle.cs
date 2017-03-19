using System;
using Alexandria.Model;

namespace Alexandria.AO3.RequestHandles
{
	public sealed class AO3TagInfoRequestHandle : IRequestHandle<IShipInfo>, IRequestHandle<ITagInfo>
	{
		public AO3TagInfoRequestHandle( String tagName )
		{
			TagName = tagName;
		}

		public String TagName { get; }
	}
}
