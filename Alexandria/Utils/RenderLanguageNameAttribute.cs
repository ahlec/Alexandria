using System;

namespace Alexandria.Utils
{
	internal class RenderLanguageNameAttribute : Attribute
	{
		public RenderLanguageNameAttribute( String name )
		{
			Name = name;
		}

		public String Name { get; set; }
	}
}
