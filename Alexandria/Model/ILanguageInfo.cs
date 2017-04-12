using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Model
{
	public interface ILanguageInfo
	{
		Language Language { get; }

		String DisplayName { get; }

		String NativeName { get; }
	}
}
