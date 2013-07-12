using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestPairIndicesModule
	{
		[Test()]
		public void TestPair()
		{
			int x = 0;
			int y = 0;
			PairIndicesModule.Position (0, out x, out y);
			Assert.True (PairIndicesModule.Index (x, y) == 0);
			Assert.True (x == 0 && y == 1);
			PairIndicesModule.Position (1, out x, out y);
			Assert.True (PairIndicesModule.Index (x, y) == 1);
			Assert.True (x == 0 && y == 2);
			PairIndicesModule.Position (2, out x, out y);
			Assert.True (PairIndicesModule.Index (x, y) == 2);
			Assert.True (x == 1 && y == 2);
			PairIndicesModule.Position (3, out x, out y);
			Assert.True (PairIndicesModule.Index (x, y) == 3);
			Assert.True (x == 0 && y == 3);
			PairIndicesModule.Position (4, out x, out y);
			Assert.True (PairIndicesModule.Index (x, y) == 4);
			Assert.True (x == 1 && y == 3);
			PairIndicesModule.Position (5, out x, out y);
			Assert.True (PairIndicesModule.Index (x, y) == 5);
			Assert.True (x == 2 && y == 3);
			PairIndicesModule.Position (6, out x, out y);
			Assert.True (PairIndicesModule.Index (x, y) == 6);
			Assert.True (x == 0 && y == 4);
		}

		[Test()]
		public void TestPairLoop () {
			var ind = new int [] {0, 1, 2, 3, 4, 5, 6};
			int n = 4;
			int k = 0;
			for (int i = 0; i < n; i++) {
				for (int j = 0; j < i; j++) {
					int x, y;
					var index = PairIndicesModule.Index (j, i);
					Assert.True (index == ind [k++]);
					PairIndicesModule.Position (index, out y, out x);
					Assert.True (i == x);
					Assert.True (j == y);
				}
			}
		}

		[Test()]
		public void TestGenerate () {
			// Generate a list determining which numbers can be divided by which.
			var a = PairIndicesModule.Generate<bool> (10, (int i, int j) => i == 0 ? false : j % i == 0);
			var b = new bool [] {
				false,	// 1
				false, 	true,	// 2
				false, 	true, 	false,	// 3
				false, 	true, 	true, 	false,	// 4
				false, 	true, 	false, 	false, 	false, 	// 5
				false, 	true, 	true, 	true, 	false, 	false,	// 6
				false, 	true,	false,	false,	false,	false, 	false,	// 7
				false, 	true,	true,	false,	true,	false,	false, 	false,	// 8
				false,	true,	false,	true,	false,	false,	false,	false,	false	// 9
			};

			int n = PairIndicesModule.Count (10);
			for (int i = 0; i < n; i++) {
				Assert.True (a [i] == b [i]);
			}

		}
	}
}

