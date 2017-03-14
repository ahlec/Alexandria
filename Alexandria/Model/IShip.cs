using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Model
{
	public interface IShip
	{
		String Name { get; }

		IEnumerable<ICharacter> Characters { get; }
	}
}
