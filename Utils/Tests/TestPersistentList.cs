using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Utils.Persistency
{
	[TestFixture()]
	public class TestPersistentList
	{
		[Test()]
		public void TestAdd()
		{
			var list = new PersistentList<string> ();
			list.Store ();
			list.Add ("hello");;
			list.Restore ();
			Assert.True (list.Count == 0);
		}
		
		[Test()]
		public void TestChange () {
			var list = new PersistentList<string> ();
			list.Add ("hello");
			list.Store ();
			list [0] = "world";
			list.Restore ();
			Assert.True (list [0] == "hello");
		}

		[Test()]
		public void TestMultipleAdd () {
			var list = new PersistentList<string> ();
			list.Store ();
			list.Add ("one");
			list.Store ();
			list.Add ("two");
			list.Store ();
			list.Add ("three");
			Assert.True (list.Count == 3);
			list.Restore ();
			Assert.True (list.Count == 2);
			list.Restore ();
			Assert.True (list.Count == 1);
			list.Restore ();
			Assert.True (list.Count == 0);
		}
	}
}

