using System;
using System.Collections.Generic;

namespace HeadLibrarian
{
	public delegate void UndoRedoStackEventHandler( Int32 currentUndoStackSize, Int32 currentRedoStackSize );

	public static class UndoRedoStack
	{
		public static void Push( IUndoRedoAction action )
		{
			if ( action == null )
			{
				throw new ArgumentNullException( nameof( action ) );
			}

			lock ( _stackLock )
			{
				_redoStack.Clear();
				_undoStack.Push( action );
			}
		}

		public static Int32 UndoStackSize
		{
			get
			{
				lock ( _stackLock )
				{
					return _undoStack.Count;
				}
			}
		}

		public static Int32 RedoStackSize
		{
			get
			{
				lock ( _stackLock )
				{
					return _redoStack.Count;
				}
			}
		}

		public static void Undo()
		{
			lock ( _stackLock )
			{
				if ( _undoStack.Count == 0 )
				{
					return;
				}

				IUndoRedoAction action = _undoStack.Pop();
				action.Undo();
				_redoStack.Push( action );

				UndoPerformed?.Invoke( _undoStack.Count, _redoStack.Count );
			}
		}

		public static event UndoRedoStackEventHandler UndoPerformed;

		public static void Redo()
		{
			lock ( _stackLock )
			{
				if ( _redoStack.Count == 0 )
				{
					return;
				}

				IUndoRedoAction action = _redoStack.Pop();
				action.Redo();
				_undoStack.Push( action );

				RedoPerformed?.Invoke( _undoStack.Count, _redoStack.Count );
			}
		}

		public static event UndoRedoStackEventHandler RedoPerformed;

		static readonly Object _stackLock = new Object();
		static readonly Stack<IUndoRedoAction> _undoStack = new Stack<IUndoRedoAction>();
		static readonly Stack<IUndoRedoAction> _redoStack = new Stack<IUndoRedoAction>();
	}
}
