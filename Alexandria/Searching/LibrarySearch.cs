using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Model;

namespace Alexandria.Searching
{
	public sealed class LibrarySearch
	{
		public String Title { get; set; }

		public Boolean OnlyIncludeCompleteFanfics { get; set; }

		public Language? Language { get; set; }
	}
}
