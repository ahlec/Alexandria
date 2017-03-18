using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Model;

namespace Alexandria.AO3.RequestHandles
{
	public sealed class AO3CharacterRequestHandle : IRequestHandle<ICharacter>
	{
		public AO3CharacterRequestHandle( String name )
		{
			Name = name;
		}

		public String Name { get; }
	}
}
