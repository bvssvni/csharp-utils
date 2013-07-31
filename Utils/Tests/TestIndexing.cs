using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestIndexing
	{
		[Test()]
		public void TestUnion()
		{
			var a = new List<int>(){10, 20};
			var b = new List<int>(){20, 30};
			var c = IndexingModule.Union(a, b);
			Assert.True(c.Count == 3);
			Assert.True(c[0] == 10);
			Assert.True(c[1] == 20);
			Assert.True(c[2] == 30);
		}

		[Test()]
		public void TestIntersect()
		{
			var a = new List<int>(){10, 20};
			var b = new List<int>(){20, 30};
			var c = IndexingModule.Intersect(a, b);
			Assert.True(c.Count == 1);
			Assert.True(c[0] == 20);
		}

		[Test()]
		public void TestExcept()
		{
			var a = new List<int>(){10, 20};
			var b = new List<int>(){20, 30};
			var c = IndexingModule.Except(a, b);
			Assert.True(c.Count == 1);
			Assert.True(c[0] == 10);
		}

		[Test()]
		public void TestBinarySearch()
		{
			var a = new List<int>{10, 20, 30};
			var ind = IndexingModule.BinarySearch(a, 20);
			Assert.True(ind == 1);
		}

		[Test()]
		public void TestInsertSorted()
		{
			var a = new List<int>(){10, 30};
			IndexingModule.InsertSorted(a, 20);
			Assert.True(a.Count == 3);
			Assert.True(a[0] == 10);
			Assert.True(a[1] == 20);
			Assert.True(a[2] == 30);
		}
	}
}

