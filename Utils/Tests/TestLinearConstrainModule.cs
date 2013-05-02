using System;
using System.Drawing;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestLinearConstrainModule
	{
		[Test()]
		public void TestBetween()
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

		[Test()]
		public void TestWithin () 
		{
			var rect = new RectangleF (0, 0, 100, 100);
			float x = 0, y = 0;
			float radius = 2;
			float newX = 0, newY = 0;

			Assert.True (LinearConstrainModule.Within (rect, radius, x, y, out newX, out newY));
			Assert.True (newX == 2);
			Assert.True (newY == 2);

			x = 100;
			y = 100;
			Assert.True (LinearConstrainModule.Within (rect, radius, x, y, out newX, out newY));
			Assert.True (newX == 98);
			Assert.True (newY == 98);

			x = 50;
			y = 50;
			Assert.True (LinearConstrainModule.Within (rect, radius, x, y, out newX, out newY));
			Assert.True (newX == 50);
			Assert.True (newY == 50);

			radius = 101;
			Assert.False (LinearConstrainModule.Within (rect, radius, x, y, out newX, out newY));
			Assert.True (newX == 50);
			Assert.True (newY == 50);

		}
	}
}

