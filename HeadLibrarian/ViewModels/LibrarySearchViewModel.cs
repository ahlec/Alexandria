using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Searching;

namespace HeadLibrarian.ViewModels
{
	internal sealed class LibrarySearchViewModel : BaseViewModel
	{
		public LibrarySearchViewModel( LibrarySearch search )
		{
			_search = search;
		}

		readonly LibrarySearch _search;
	}
}
