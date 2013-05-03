using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestDualComplexModule
	{
		[Test()]
		public void TestMultiply () {
			var a = new float[] {1, 2};
			var b = new float[] {3, 5};
			var c = new float[] {0, 0};
			DualModule.Multiply (a, b, c);
			var d = new float[] {3, 11};
			Assert.True (c[0] == d[0] && c[1] == d[1]);
		}

		[Test()]
		public void TestMultiplyScalar () {
			var a = new float[] {1, 2};
			var b = new float[] {3, 5, 4, 6};
			var c = new float[] {0, 0, 0, 0};
			DualModule.Multiply (a, b, c);
			var d = new float[] {3, 11, 4, 14};

			Console.WriteLine ("{0} {1} {2} {3}", c[0], c[1], c[2], c[3]);

			Assert.True (DualModule.AllEqual (c, d));
		}
	}
}

