using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestClosestNeighbor
	{
		[Test()]
		public void TestClosest()
		{
			var rnd = new Random(0);
			var n = 4;
			var stride = 64;
			var m = stride * stride;
			var list = new List<int>(n);
			for (int i = 0; i < n; i++)
			{
				var r = rnd.Next(m);
				list.Add(r);
			}

			list.Sort();

			var a = ClosestNeighborModule.Closest(list, stride, 58 + 58 * stride);
			Assert.True(a == 58 + 58 * stride);
			a = ClosestNeighborModule.Closest(list, stride, 59 + 58 * stride);
			Assert.True(a == 58 + 58 * stride);
			a = ClosestNeighborModule.Closest(list, stride, 58 + 59 * stride);
			Assert.True(a == 58 + 58 * stride);
			a = ClosestNeighborModule.Closest(list, stride, 57 + 58 * stride);
			Assert.True(a == 58 + 58 * stride);
			a = ClosestNeighborModule.Closest(list, stride, 58 + 57 * stride);
			Assert.True(a == 58 + 58 * stride);

			var b = ClosestNeighborModule.Closest(list, stride, 27 + 16 * stride);
			Assert.True(b == 27 + 16 * stride);
			b = ClosestNeighborModule.Closest(list, stride, 28 + 16 * stride);
			Assert.True(b == 27 + 16 * stride);
			b = ClosestNeighborModule.Closest(list, stride, 27 + 17 * stride);
			Assert.True(b == 27 + 16 * stride);
			b = ClosestNeighborModule.Closest(list, stride, 26 + 16 * stride);
			Assert.True(b == 27 + 16 * stride);
			b = ClosestNeighborModule.Closest(list, stride, 27 + 15 * stride);
			Assert.True(b == 27 + 16 * stride);
		}

		[Test()]
		public void TestWithinRadius()
		{
			var rnd = new Random(0);
			var n = 4;
			var stride = 64;
			var m = stride * stride;
			var list = new List<int>(n);
			for (int i = 0; i < n; i++)
			{
				var r = rnd.Next(m);
				list.Add(r);
			}
			
			list.Sort();

			var within = ClosestNeighborModule.WithinRadius(1, list, stride, 0 + 0 * stride);
			Assert.True(within.Count == 0);

			within = ClosestNeighborModule.WithinRadius(1, list, stride, 58 + 58 * stride);
			Assert.True(within.Count == 1);
			Assert.True(within[0] == 58 + 58 * stride);
			within = ClosestNeighborModule.WithinRadius(1, list, stride, 59 + 58 * stride);
			Assert.True(within.Count == 1);
			Assert.True(within[0] == 58 + 58 * stride);
			within = ClosestNeighborModule.WithinRadius(1, list, stride, 58 + 59 * stride);
			Assert.True(within.Count == 1);
			Assert.True(within[0] == 58 + 58 * stride);
			within = ClosestNeighborModule.WithinRadius(1, list, stride, 57 + 58 * stride);
			Assert.True(within.Count == 1);
			Assert.True(within[0] == 58 + 58 * stride);
			within = ClosestNeighborModule.WithinRadius(1, list, stride, 58 + 57 * stride);
			Assert.True(within.Count == 1);
			Assert.True(within[0] == 58 + 58 * stride);

			within = ClosestNeighborModule.WithinRadius(45, list, stride, 0 + 0 * stride);
			Assert.True(within.Count == 2);
			Assert.True(within[0] == 43 + 9 * stride);
			Assert.True(within[1] == 27 + 16 * stride);
		}
	}
}

