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
	}
}

