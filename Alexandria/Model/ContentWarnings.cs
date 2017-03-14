using System;

namespace Alexandria.Model
{
	[Flags]
	public enum ContentWarnings
	{
		None = 0,
		Undetermined = 1,
		Violence = 2,
		MajorCharacterDeath = 4,
		Rape = 8,
		Underage = 16
	}
}
