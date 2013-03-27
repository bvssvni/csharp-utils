using System;

namespace Utils
{
	public interface IUndoAction<App> :
		IDisposable
	{
		void Undo(App app);
		void Redo(App app);
	}
}

