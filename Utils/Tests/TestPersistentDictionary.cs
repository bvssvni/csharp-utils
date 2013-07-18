using System;
using NUnit.Framework;

namespace Utils.Persistency
{
	[TestFixture()]
	public class TestPersistentDictionary
	{
		[Test()]
		public void TestAdd()
		{
			var a = new PersistentDictionary<int, string> ();
			a.Store ();
			a.Add (10, "Hello");
			a.Restore ();
			Assert.True (a.Count == 0);
		}
		
		[Test()]
		public void TestChange () {
			var a = new PersistentDictionary<int, string> ();
			a.Add (10, "Hello");
			a.Store ();
			a [10] = "World!";
			a.Restore ();
			Assert.True (a[10] == "Hello");
		}
	}
}

