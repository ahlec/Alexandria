using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria
{
	public interface IRequestHandle<T> where T : IRequestable
	{
	}
}
