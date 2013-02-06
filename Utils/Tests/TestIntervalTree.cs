using System;
using NUnit.Framework;

namespace Utils
{
	public class TestIntervalTree
	{
		[Test()]
		public void TestIntervalTree1 ()
		{
			var text = "{}".ToCharArray ();
			var start = Parsing.FromCharInUnicodeArray ('{', text);
			var end = Parsing.FromCharInUnicodeArray ('}', text);
			var trees = IntervalTree.Nested (start.ToIndices(), end.ToIndices());
			Assert.True (trees.Length == 1);
			Assert.True (trees[0].Start == 0);
			Assert.True (trees[0].End == 1);
			Assert.True (trees[0].Children.Count == 0);
		}

		[Test()]
		public void TestIntervalTree2 ()
		{
			var text = "{{}}".ToCharArray ();
			var start = Parsing.FromCharInUnicodeArray ('{', text);
			var end = Parsing.FromCharInUnicodeArray ('}', text);
			var trees = IntervalTree.Nested (start.ToIndices(), end.ToIndices());
			Assert.True (trees.Length == 1);
			Assert.True (trees[0].Start == 0);
			Assert.True (trees[0].End == 3);
			Assert.True (trees[0].Children.Count == 1);
			var child = trees[0].Children[0];
			Assert.True (child.Start == 1);
			Assert.True (child.End == 2);
		}

		[Test()]
		public void TestIntervalTree3 ()
		{
			var text = "{}{}".ToCharArray ();
			var start = Parsing.FromCharInUnicodeArray ('{', text);
			var end = Parsing.FromCharInUnicodeArray ('}', text);
			var trees = IntervalTree.Nested (start.ToIndices(), end.ToIndices());

			Assert.True (trees.Length == 2);
			Assert.True (trees[0].Start == 0);
			Assert.True (trees[0].End == 1);
			Assert.True (trees[1].Start == 2);
			Assert.True (trees[1].End == 3);
		}

		[Test()]
		public void TestIntervalTree4 ()
		{
			var text = "{{}{}}".ToCharArray ();
			var start = Parsing.FromCharInUnicodeArray ('{', text);
			var end = Parsing.FromCharInUnicodeArray ('}', text);
			var trees = IntervalTree.Nested (start.ToIndices(), end.ToIndices());
			
			Assert.True (trees.Length == 1);
			var a = trees[0];
			Assert.True (a.Start == 0);
			Assert.True (a.End == 5);
			Assert.True (a.Children.Count == 2);

			var b = a.Children[0];
			Assert.True (b.Start == 1);
			Assert.True (b.End == 2);
			var c = a.Children[1];
			Assert.True (c.Start == 3);
			Assert.True (c.End == 4);
		}

		[Test()]
		public void TestIntervalTree5 ()
		{
			var text = "{{}{}}{}".ToCharArray ();
			var start = Parsing.FromCharInUnicodeArray ('{', text);
			var end = Parsing.FromCharInUnicodeArray ('}', text);
			var trees = IntervalTree.Nested (start.ToIndices(), end.ToIndices());
			
			Assert.True (trees.Length == 2);
			var a = trees[0];
			Assert.True (a.Start == 0);
			Assert.True (a.End == 5);
			Assert.True (a.Children.Count == 2);
			
			var b = a.Children[0];
			Assert.True (b.Start == 1);
			Assert.True (b.End == 2);
			var c = a.Children[1];
			Assert.True (c.Start == 3);
			Assert.True (c.End == 4);

			var d = trees[1];
			Assert.True (d.Start == 6);
			Assert.True (d.End == 7);
		}

		[Test()]
		public void TestIntervalTree6 ()
		{
			var text = "{{}{}}{{}}".ToCharArray ();
			var start = Parsing.FromCharInUnicodeArray ('{', text);
			var end = Parsing.FromCharInUnicodeArray ('}', text);
			var trees = IntervalTree.Nested (start.ToIndices(), end.ToIndices());
			
			Assert.True (trees.Length == 2);
			var a = trees[0];
			Assert.True (a.Start == 0);
			Assert.True (a.End == 5);
			Assert.True (a.Children.Count == 2);
			
			var b = a.Children[0];
			Assert.True (b.Start == 1);
			Assert.True (b.End == 2);
			var c = a.Children[1];
			Assert.True (c.Start == 3);
			Assert.True (c.End == 4);
			
			var d = trees[1];
			Assert.True (d.Start == 6);
			Assert.True (d.End == 9);
			Assert.True (d.Children.Count == 1);
			Assert.True (d.Children[0].Start == 7);
			Assert.True (d.Children[0].End == 8);
		}

		[Test()]
		public void TestAddNode1 ()
		{
			var tree = new IntervalTree (0, 100);
			bool added = tree.Add (0, 10);
			Assert.True (added);
			Assert.True (tree.Children.Count == 1);
			var child = tree.Children[0];
			Assert.True (child.Start == 0);
			Assert.True (child.End == 10);

			added = tree.Add (9, 15);
			Assert.True (!added);

			added = tree.Add (60, 65);
			Assert.True (added);
			child = tree.Children[1];
			Assert.True (child.Start == 60);
			Assert.True (child.End == 65);

			added = tree.Add (20, 60);
			Assert.True (added);
			Assert.True (tree.Children.Count == 3);
			child = tree.Children[1];
			Assert.True (child.Start == 20);
			Assert.True (child.End == 60);
		}
	}
}

