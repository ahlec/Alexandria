using System;

namespace Bibliothecary.Core.Utils
{
	internal sealed class TumblrPhotosetContentWarning
	{
		public TumblrPhotosetContentWarning( String text )
		{
			Text = text;
		}

		public String Text { get; }
	}
}
