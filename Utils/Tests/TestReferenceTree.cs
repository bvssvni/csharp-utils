using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestReferenceTree
	{
		[Test()]
		public void TestConstruction ()
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

		[Test()]
		public void TestInsert () {
			var a = new ReferenceTree<int> (1);
			var b = new ReferenceTree<int> (2) {
				Left = a
			};
			// 'a' got a reference because it is owned by 'b'.
			Assert.True (a.References == 1);
			Assert.True (b.References == 0);

			// When inserting, since there is only one reference,
			// the returned tree should be the same object as 'b'.
			Assert.True (b.Insert (0) == b);
			Assert.True (b.Left.Left.Data == 0);

			var c = new ReferenceTree<int> (3) {
				Left = a
			};
			// 'a' got two references because it is owned by 'b' and 'c'.
			Assert.True (a.References == 2);
			Assert.True (c.References == 0);

			// Now that 'a' is shared, we create a new tree.
			var d = c.Insert (-1);
			Assert.False (c == d);
			c = d;

			Assert.True (c.Contains (-1));
			Assert.True (c.Contains (1));
			Assert.True (c.Contains (3));
			Assert.True (c.References == 0);

			// Check that old tree was not influenced.
			Assert.False (b.Contains (-1));

			d = c.Insert (-2);
			Assert.True (d.Contains (-2));
			Assert.True (d.Contains (-1));
			Assert.True (d.Contains (1));
			Assert.True (d.Contains (3));
		}

	}
}

