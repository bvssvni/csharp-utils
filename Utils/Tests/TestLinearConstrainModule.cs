using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestLinearConstrainModule
	{
		[Test()]
		public void TestCase()
		{
			float min = 0;
			float max = 10;
			float x = 5;
			float radius = 2;
			float result = 0;

			Assert.True (LinearConstrainModule.Between (min, max, radius, x, out result));
			Assert.True (result == 5);

			x = 0;
			Assert.True (LinearConstrainModule.Between (min, max, radius, x, out result));
			Assert.True (result == 2);

			x = 10;
			Assert.True (LinearConstrainModule.Between (min, max, radius, x, out result));
			Assert.True (result == 8);

			x = 5;
			radius = 6;
			Assert.False (LinearConstrainModule.Between (min, max, radius, x, out result));
			Assert.True (result == 5);
		}
	}
}

