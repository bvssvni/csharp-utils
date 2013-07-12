using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestCompareIndicesModule
	{
		[Test()]
		public void TestCase()
		{
			Assert.True (CompareIndicesModule.ToCompare (0) == -1);
			Assert.True (CompareIndicesModule.ToCompare (1) == 1);
			Assert.True (CompareIndicesModule.ToCompare (2) == -2);
			Assert.True (CompareIndicesModule.ToCompare (3) == 2);
			Assert.True (CompareIndicesModule.ToCompare (4) == -3);
			Assert.True (CompareIndicesModule.ToCompare (5) == 3);

			Assert.True (CompareIndicesModule.ToIndex (-1) == 0);
			Assert.True (CompareIndicesModule.ToIndex (1) == 1);
			Assert.True (CompareIndicesModule.ToIndex (-2) == 2);
			Assert.True (CompareIndicesModule.ToIndex (2) == 3);
			Assert.True (CompareIndicesModule.ToIndex (-3) == 4);
			Assert.True (CompareIndicesModule.ToIndex (3) == 5);
		}
	}
}

