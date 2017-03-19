using System;
using Alexandria.Model;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3Tag : ITag
	{
		public AO3Tag( String tag )
		{
			Text = tag;
			Info = new AO3TagInfoRequestHandle( tag );
		}

		public String Text { get; }

		public IRequestHandle<ITagInfo> Info { get; }
	}
}
