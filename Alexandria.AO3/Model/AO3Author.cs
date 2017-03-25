using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Alexandria.Model;
using Alexandria.RequestHandles;
using Alexandria.AO3.RequestHandles;

namespace Alexandria.AO3.Model
{
	internal sealed class AO3Author : IAuthor
	{
		AO3Author()
		{
		}

		#region IAuthor

		public String Name { get; private set; }

		public IReadOnlyList<String> Nicknames { get; private set; }

		public DateTime DateJoined { get; private set; }

		public String Location { get; private set; }

		public DateTime? Birthday { get; private set; }

		public String Biography { get; private set; }

		public IReadOnlyList<IFanficRequestHandle> Works { get; private set; }

		#endregion IAuthor

		public static AO3Author Parse( HtmlDocument profileDocument )
		{
			throw new NotImplementedException();
		}
	}
}
