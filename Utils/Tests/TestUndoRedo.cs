using System;
using NUnit.Framework;

namespace Utils.Persistency
{
	[TestFixture()]
	public class TestUndoRedo
	{
		[Test()]
		public void TestCase()
		{
			var data = new PersistentValue<string> ("one");
			var undoRedo = new UndoRedo<PersistentValue<string>> (data);
			Assert.True (data.Value == "one");
			Assert.True (undoRedo.Count == 0);
			Assert.True (undoRedo.Cursor == 0);
			Assert.True (undoRedo.PreviousDescription == null);
			Assert.False (undoRedo.CanUndo);
			Assert.False (undoRedo.CanRedo);

			undoRedo.NewAction ("Add Two");
			data.Value = "two";
			Assert.True (data.Value == "two");
			Assert.True (undoRedo.Count == 1);
			Assert.True (undoRedo.Cursor == 1);
			Assert.True (undoRedo.PreviousDescription == "Add Two");
			Assert.True (undoRedo.CanUndo);
			Assert.False (undoRedo.CanRedo);

			undoRedo.NewAction ("Add Three");
			data.Value = "three";
			Assert.True (data.Value == "three");
			Assert.True (undoRedo.Count == 2);
			Assert.True (undoRedo.Cursor == 2);
			Assert.True (undoRedo.PreviousDescription == "Add Three");
			Assert.True (undoRedo.CanUndo);
			Assert.False (undoRedo.CanRedo);

			undoRedo.Undo ();
			Assert.True (data.Value == "two");
			Assert.True (undoRedo.Count == 2);
			Assert.True (undoRedo.Cursor == 1);
			Assert.True (undoRedo.PreviousDescription == "Add Two");
			Assert.True (undoRedo.CanUndo);
			Assert.True (undoRedo.CanRedo);

			undoRedo.Redo ();
			Assert.True (data.Value == "three");
			Assert.True (undoRedo.Count == 2);
			Assert.True (undoRedo.Cursor == 2);
			Assert.True (undoRedo.PreviousDescription == "Add Three");
			Assert.True (undoRedo.CanUndo);
			Assert.False (undoRedo.CanRedo);

			undoRedo.Undo ();
			Assert.True (data.Value == "two");
			Assert.True (undoRedo.PreviousDescription == "Add Two");

			undoRedo.Undo ();
			Assert.True (data.Value == "one");
			Assert.True (undoRedo.PreviousDescription == null);

			undoRedo.Redo ();
			Assert.True (data.Value == "two");
			Assert.True (undoRedo.PreviousDescription == "Add Two");

			undoRedo.NewAction ("Add Four");
			data.Value = "four";
			Assert.True (undoRedo.Count == 2);
			Assert.True (undoRedo.CanUndo);
			Assert.False (undoRedo.CanRedo);
		}
	}
}

