using System;
using NUnit.Framework;
using Play;

namespace Utils
{
	[TestFixture()]
	public class TestVectorGroup
	{
		[Test()]
		public void TestAddXY()
		{
			double[] x = {0, 0, 0, 0};
			double[] y = {0, 0, 0, 0};
			double[] vx = {1, 2, 3, 4};
			double[] vy = {4, 3, 2, 1};
			VectorGroup.Add(Group.All (x), x, vx, x);
			VectorGroup.Add(Group.All (y), y, vy, y);

			Assert.True(x[0] == 1);
			Assert.True(x[1] == 2);
			Assert.True(x[2] == 3);
			Assert.True(x[3] == 4);

			Assert.True(y[0] == 4);
			Assert.True(y[1] == 3);
			Assert.True(y[2] == 2);
			Assert.True(y[3] == 1);
		}

		[Test()]
		public void TestMultiplyXY()
		{
			double[] x = {4, 3, 2, 1};
			double[] y = {1, 2, 3, 4};
			double[] vx = {1, 2, 3, 4};
			double[] vy = {4, 3, 2, 1};
			// VectorGroup.Multiply(Group.Slice(0, 3), x, y, vx, vy, x, y);
			VectorGroup.Multiply (Group.Slice(0, 3), x, vx, x);
			VectorGroup.Multiply (Group.Slice(0, 3), y, vy, y);
			Assert.True(x[0] == 4);
			Assert.True(x[1] == 6);
			Assert.True(x[2] == 6);
			Assert.True(x[3] == 4);
			
			Assert.True(y[0] == 4);
			Assert.True(y[1] == 6);
			Assert.True(y[2] == 6);
			Assert.True(y[3] == 4);
		}

		[Test()]
		public void TestDirectionXY()
		{
			double[] x = {0};
			double[] y = {0};
			double[] tx = {10};
			double[] ty = {0};
			double[] dirx = {0};
			double[] diry = {0};
			VectorGroup.DirectionXY(Group.All(x), x, y, tx, ty, dirx,diry);
			Assert.True(dirx[0] == 1);
			Assert.True(diry[0] == 0);
		}
	}
}

