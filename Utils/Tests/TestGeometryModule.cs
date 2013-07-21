using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestGeometryModule
	{
		[Test()]
		public void TestCircle()
		{
			Assert.True (GeometryModule.AreaOfCircleByRadius (0) == 0);
			Assert.True (GeometryModule.AreaOfCircleByRadius (1) == Math.PI);
		}

		[Test()]
		public void TestFilledHalfCircle () {
			Assert.True (GeometryModule.AreaOfFilledHalfCircleFromCenterByFactor (0) == 0);
			Assert.True (GeometryModule.AreaOfFilledHalfCircleFromCenterByFactor (1) == 0.5 * Math.PI);
		}

		[Test()]
		public void TestFilledCircle () {
			Assert.True (GeometryModule.AreaOfFilledCircleFromEdgeByFactor (0) == 0);
			Assert.True (GeometryModule.AreaOfFilledCircleFromEdgeByFactor (1) == Math.PI);
		}
	}
}

