using System;

namespace Alexandria.Utils
{
	[AttributeUsage( AttributeTargets.Field )]
	internal class NativeLanguageAttribute : Attribute
	{
		public NativeLanguageAttribute( String native )
		{
			Native = native;
		}

		public String Native { get; }
	}
}
