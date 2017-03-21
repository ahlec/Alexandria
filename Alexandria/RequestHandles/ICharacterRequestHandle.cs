using System;
using Alexandria.Model;

namespace Alexandria.RequestHandles
{
	public interface ICharacterRequestHandle : IRequestHandle<ICharacter>
	{
		String FullName { get; }
	}
}
