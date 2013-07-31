using System;
using NUnit.Framework;

namespace Utils.Persistency
{
	[TestFixture()]
	public class TestPersistent
	{
		[Test()]
		public void TestStore()
		{
			var a = new Persistent<string>("one");
			a.Store();
			a.Value = "two";
			a.Undo();
			Assert.True(a.Value == "one");
			a.Redo();
			Assert.True(a.Value == "two");
		}

		[Test()]
		public void TestList()
		{
			var a = new PersistentList<string>();
			a.Add("one");
			a.Store();
			a.Add("two");
			a.Undo();
			Assert.True(a.Count == 1);
			Assert.True(a[0] == "one");
			a.Redo();
			Assert.True(a.Count == 2);
			Assert.True(a[0] == "one");
			Assert.True(a[1] == "two");
		}

		[Test()]
		public void TestDictionary()
		{
			var a = new PersistentDictionary<string, int>();
			a.Add("one", 1);
			a.Store();
			a.Add("two", 2);
			a.Undo();
			Assert.True(a.Count == 1);
			Assert.True(a["one"] == 1);
			a.Redo();
			Assert.True(a.Count == 2);
			Assert.True(a["one"] == 1);
			Assert.True(a["two"] == 2);
		}

		[Test()]
		public void TestUndoRedo()
		{
			var a = new Persistent<string>("what?");
			var undoRedo = new UndoRedo<Persistent<string>>(a);
			undoRedo.NewAction("Say something");
			a.Value = "hello";
			undoRedo.Undo();
			Assert.True(a.Value == "what?");
			undoRedo.Redo();
			Assert.True(a.Value == "hello");
		}

		[Test()]
		public void TestPersistentStructureString()
		{
			var a = new Persistent<Persistent<string>>(new Persistent<string>("one"));
			a.Store();
			a.Value.Value = "two";
			a.Undo();
			Assert.True(a.Value.Value == "one");
			a.Redo();
			Assert.True(a.Value.Value == "two");
		}

		[Test()]
		public void TestPersistentStructureList()
		{
			var a = new PersistentList<Persistent<string>>();
			a.Add(new Persistent<string>("one"));
			a.Store();
			a[0].Value = "two";
			a.Undo();
			Assert.True(a[0].Value == "one");
			a.Redo();
			Assert.True(a[0].Value == "two");
		}

		[Test()]
		public void TestPersistentStructureDictionary()
		{
			var a = new PersistentDictionary<string, Persistent<int>>();
			a.Add("one", new Persistent<int>(1));
			a.Store();
			a["one"].Value = 2;
			a.Undo();
			Assert.True(a["one"].Value == 1);
			a.Redo();
			Assert.True(a["one"].Value == 2);
		}

		private class LoadedTestObject : ILoaded
		{
			private bool m_used = true;

			public bool Loaded
			{
				get
				{
					return m_used;
				}
				set
				{
					m_used = value;
				}
			}
		}

		[Test()]
		public void TestLoaded()
		{
			var a = new LoadedTestObject();
			Assert.True(a.Loaded);
			var persistent = new Persistent<LoadedTestObject>(a);
			var b = new LoadedTestObject();
			persistent.Store();
			Assert.True(a.Loaded);
			persistent.Value = b;
			persistent.Store();
			Assert.False(a.Loaded);
			var c = new LoadedTestObject();
			persistent.Value = c;
			persistent.Store();
			// previous:c, current:c
			Assert.False(a.Loaded);
			Assert.False(b.Loaded);
			persistent.Undo();
			// previous:b, current:c, next:c
			persistent.Undo();
			// previous:a, current:b, next:c
			Assert.True(persistent.Value == b);
			Assert.False(a.Loaded);
			Assert.True(b.Loaded);
			Assert.False(c.Loaded);
			persistent.Undo();
			// current:a, next:b
			Assert.True(a.Loaded);
			Assert.False(b.Loaded);
			Assert.False(c.Loaded);
			persistent.Redo();
			// previous:a, current:b, next:c
			persistent.Redo();
			// previous:b, current:c, next:c
			Assert.False(a.Loaded);
			Assert.False(b.Loaded);
			Assert.True(c.Loaded);
			persistent.Redo();
			// previous:c, current:c
			Assert.False(a.Loaded);
			Assert.False(b.Loaded);
			Assert.True(c.Loaded);
		}
	}
}

