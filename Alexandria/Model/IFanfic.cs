using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Model
{
	public interface IFanfic
	{
		String Title { get; }

		MaturityRating Rating { get; }

		ContentWarnings ContentWarnings { get; }

		Int32 NumberWords { get; }

		/*
		 * Boolean IsCompleted { get; }

		DateTime DateStartedUtc { get; }

		DateTime DateLastUpdatedUtc { get; }

		Int32 NumberLikes { get; }

		Int32 NumberComments { get; }*/
	}
}
