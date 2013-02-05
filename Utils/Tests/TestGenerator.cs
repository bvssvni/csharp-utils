using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestGenerator
	{
		[Test()]
		public void TestSimpleRangeHasCorrectValues ()
		{
			var gen = StandardGenerators.Range (0, 5, 1);
			int i = gen.Reset ();
			Assert.True (i == 0);
			gen.Next (ref i);
			Assert.True (i == 1);
			gen.Next (ref i);
			Assert.True (i == 2);
			gen.Next (ref i);
			Assert.True (i == 3);
			gen.Next (ref i);
			Assert.True (i == 4);
		}

		[Test()]
		public void TestOrOfTwoSimpleRangesGotCorrectValues ()
		{
			var a = StandardGenerators.Range (0, 2, 1);
			var b = StandardGenerators.Range (2, 4, 1);
			var c = Generator<int>.Or (a, b);
			int i = c.Reset ();
			Assert.True (i == 0);
			c.Next (ref i);
			Assert.True (i == 1);
			c.Next (ref i);
			Assert.True (i == 2);
			c.Next (ref i);
			Assert.True (i == 3);
			c.Next (ref i);
			Assert.True (i == 4);
		}

		[Test()]
		public void TestAndOfTwoSimpleRangesGotCorrectValues ()
		{
			var a = StandardGenerators.Range (0, 10, 1);
			var b = StandardGenerators.Range (2, 12, 2);
			var c = Generator<int>.And (a, b);
			int i = c.Reset ();
			Assert.True (i == 2);
			c.Next (ref i);
			Assert.True (i == 4);
			c.Next (ref i);
			Assert.True (i == 6);
			c.Next (ref i);
			Assert.True (i == 8);
			Assert.True (c.Next (ref i) == false);
		}

		[Test()]
		public void TestExceptOfTwoSimpleRangesGotCorrectValues ()
		{
			var a = StandardGenerators.Range (0, 10, 1);
			var b = StandardGenerators.Range (2, 8, 1);
			var c = Generator<int>.Except (a, b);
			int i = c.Reset ();
			Assert.True (i == 0);
			c.Next (ref i);
			Assert.True (i == 1);
			c.Next (ref i);
			Assert.True (i == 8);
			c.Next (ref i);
			Assert.True (i == 9);
			Assert.True (c.Next (ref i) == false);
		}

		[Test()]
		public void TestExceptOfTwoAdvancesRangesGotCorrectValues ()
		{
			var a = StandardGenerators.Range (0, 10, 1);
			var b = StandardGenerators.Range (2, 8, 2);
			int j = 1;
			b.Next (ref j);
			Console.WriteLine (j);
			Assert.True (j == 2);

			var c = Generator<int>.Except (a, b);
			int i = c.Reset ();
			Assert.True (i == 0);
			c.Next (ref i);
			Assert.True (i == 1);
			c.Next (ref i);
			Assert.True (i == 3);
			c.Next (ref i);
			Assert.True (i == 5);
			c.Next (ref i);
			Assert.True (i == 7);
			c.Next (ref i);
			Console.WriteLine (i.ToString ());
			Assert.True (i == 8);
			c.Next (ref i);
			Assert.True (i == 9);
			Assert.True (c.Next (ref i) == false);
		}
	}
}

