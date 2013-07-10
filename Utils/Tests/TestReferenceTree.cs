using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestReferenceTree
	{
		[Test()]
		public void TestCase()
		{
			var a = new ReferenceTree<int> (2) {
				Left = new ReferenceTree<int> (1),
				Right = new ReferenceTree<int> (3)
			};
			Assert.True (a.Contains (1));
			Assert.True (a.Contains (2));
			Assert.True (a.Contains (3));

			// This cuts off 3 since 4 is larger than 2.
			var c = new ReferenceTree<int> (4) {
				Parent = a
			};
			Assert.True (c.Contains (1));
			Assert.True (c.Contains (2));
			Assert.True (c.Contains (4));
			Assert.False (c.Contains (3));

			// This cuts off 1 and 2 since 3 is smaller than 4.
			var d = new ReferenceTree<int> (3) {
				Parent = c
			};
			Assert.True (d.Contains (3));
			Assert.True (d.Contains (4));
			Assert.False (d.Contains (1));
			Assert.False (d.Contains (2));

			// Cuts away 3, adds 4 but puts 3 back again.
			var e = new ReferenceTree<int> (4) {
				Parent = a,
				Left = a.Right
			};
			Assert.True (e.Contains (1));
			Assert.True (e.Contains (2));
			Assert.True (e.Contains (3));
			Assert.True (e.Contains (4));

		}
	}
}

