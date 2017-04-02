using System;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.RequestHandles
{
	internal sealed class AO3TagRequestHandle : ITagRequestHandle
	{
		public AO3TagRequestHandle( String tagName )
		{
			Text = tagName;
		}

		public String Text { get; }

		public override string ToString()
		{
			return Text;
		}
	}
}
