using System;

namespace Alexandria.Model
{
	public interface ILanguageInfo
	{
		Language Language { get; }

		String DisplayName { get; }

		String NativeName { get; }
	}
}
