
namespace HeadLibrarian
{
	public interface IUndoRedoAction
	{
		void Undo();

		void Redo();
	}
}
