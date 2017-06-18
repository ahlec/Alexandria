using System;
using Alexandria.Model;

namespace Bibliothecary.Core.Publishing
{
	public sealed class TumblrClient
	{
		public String ConsumerKey { get; set; }

		public String ConsumerSecret { get; set; }

		public String OauthToken { get; set; }

		public String OauthTokenSecret { get; set; }

		public String BlogName { get; set; }

		public void Post( IFanfic fanfic )
		{
			throw new NotImplementedException();
		}
	}
}
