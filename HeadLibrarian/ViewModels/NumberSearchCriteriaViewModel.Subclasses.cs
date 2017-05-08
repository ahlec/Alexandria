using Alexandria.Searching;

namespace HeadLibrarian.ViewModels
{
	public abstract partial class NumberSearchCriteriaViewModel
	{
		public class WordCount : NumberSearchCriteriaViewModel
		{
			public WordCount( ProjectViewModel viewModel, LibrarySearch search ) : base( viewModel, search )
			{
			}

			/// <inheritdoc />
			protected override NumberSearchCriteria ActualObject
			{
				get => Search.WordCount;
				set => Search.WordCount = value;
			}
		}

		public class NumberLikes : NumberSearchCriteriaViewModel
		{
			public NumberLikes( ProjectViewModel viewModel, LibrarySearch search ) : base( viewModel, search )
			{
			}

			/// <inheritdoc />
			protected override NumberSearchCriteria ActualObject
			{
				get => Search.NumberLikes;
				set => Search.NumberLikes = value;
			}
		}

		public class NumberComments : NumberSearchCriteriaViewModel
		{
			public NumberComments( ProjectViewModel viewModel, LibrarySearch search ) : base( viewModel, search )
			{
			}

			/// <inheritdoc />
			protected override NumberSearchCriteria ActualObject
			{
				get => Search.NumberComments;
				set => Search.NumberComments = value;
			}
		}
	}
}
