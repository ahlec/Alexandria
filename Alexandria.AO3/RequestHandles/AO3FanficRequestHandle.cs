using System;
using Alexandria.RequestHandles;
using HtmlAgilityPack;

namespace Alexandria.AO3.RequestHandles
{
	internal sealed class AO3FanficRequestHandle : IFanficRequestHandle
	{
		public AO3FanficRequestHandle( String handle )
		{
			Handle = handle;
		}

		public String Handle { get; }

		internal static AO3FanficRequestHandle ParseFromWorkLi( HtmlNode li )
		{
			String fanficHandle = li.GetAttributeValue( "id", null ).Substring( "work_".Length );
			return new AO3FanficRequestHandle( fanficHandle );
		}

		public override String ToString()
		{
			return Handle;
		}
	}
}
