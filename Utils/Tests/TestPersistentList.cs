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
	}
}

