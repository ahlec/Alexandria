using System;
using Alexandria.RequestHandles;

namespace Alexandria.AO3.RequestHandles
{
	internal sealed class AO3CharacterRequestHandle : ICharacterRequestHandle
	{
		public AO3CharacterRequestHandle( String name )
		{
			FullName = name;
		}

		public String FullName { get; }

		public override string ToString()
		{
			return FullName;
		}
	}
}
