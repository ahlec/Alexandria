using System;
using Bibliothecary.Core;
using Bibliothecary.Core.Publishing;

namespace HeadLibrarian.ViewModels
{
	public partial class TumblrTagRuleListViewModel
	{
		class AddTagUndoAction : BaseUndoAction
		{
			public AddTagUndoAction( TumblrTagRuleListViewModel viewModel, PublishingInfo publishingInfo, TumblrTagRule itemAdded )
			{
				_viewModel = viewModel;
				_publishingInfo = publishingInfo;
				_itemAdded = itemAdded;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _publishingInfo.RemoveTumblrTag( _itemAdded, out Int32 index ) );
				_viewModel.InvokeItemRemoved( _itemAdded, index );
			}

			public override void Redo()
			{
				AssertModelSetFunction( _publishingInfo.AddTumblrTag( _itemAdded ) );
				_viewModel.InvokeItemAdded( _itemAdded, _publishingInfo.TumblrTags.Count - 1 );
			}

			readonly TumblrTagRuleListViewModel _viewModel;
			readonly PublishingInfo _publishingInfo;
			readonly TumblrTagRule _itemAdded;
		}

		class RemoveTagUndoAction : BaseUndoAction
		{
			public RemoveTagUndoAction( TumblrTagRuleListViewModel viewModel, PublishingInfo publishingInfo, TumblrTagRule itemRemoved, Int32 index )
			{
				_viewModel = viewModel;
				_publishingInfo = publishingInfo;
				_itemRemoved = itemRemoved;
				_index = index;
			}

			public override void Undo()
			{
				AssertModelSetFunction( _publishingInfo.AddTumblrTag( _itemRemoved, _index ) );
				_viewModel.InvokeItemAdded( _itemRemoved, _index );
			}

			public override void Redo()
			{
				AssertModelSetFunction( _publishingInfo.RemoveTumblrTag( _itemRemoved, out Int32 index ) );
				_viewModel.InvokeItemRemoved( _itemRemoved, index );
			}

			readonly TumblrTagRuleListViewModel _viewModel;
			readonly PublishingInfo _publishingInfo;
			readonly TumblrTagRule _itemRemoved;
			readonly Int32 _index;
		}
	}
}
