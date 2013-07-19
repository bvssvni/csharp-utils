using System;

namespace Utils.Persistency
{
	public class UndoRedo<T> where T : PersistentBase<T>, new()
	{
		public T Current = new T ();
		public T Future = new T ();

		public UndoRedo()
		{
		}

		public void Save () {
			Current.Store ();
		}

		public void Redo () {

		}
	}
}

