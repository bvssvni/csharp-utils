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
			Assert.True (DualModule.AllEquals (c, d, float.Epsilon));
		}

		[Test()]
		public void TestDivide () {
			var a = new float[] {1, 2};
			var b = new float[] {3, 5};
			var c = new float[] {0, 0};
			DualModule.Divide (a, b, c);
			var d = new float[] {1.0f/3.0f, 1.0f/9.0f};
			Assert.True (DualModule.AllEquals (c, d, 0.0001f));
		}

		[Test()]
		public void TestMultiplyScalar () {
			var a = new float[] {1, 2};
			var b = new float[] {3, 5, 4, 6};
			var c = new float[] {0, 0, 0, 0};
			DualModule.Multiply (a, b, c);
			var d = new float[] {3, 11, 4, 14};
			Assert.True (DualModule.AllEquals (c, d, float.Epsilon));
		}

		[Test()]
		public void TestAnyEquals () {
			var a = new float[] {1, 2};
			var b = new float[] {3, 5, 1, 2};
			Assert.True (DualModule.AnyEquals (a, b));
		}

		[Test()]
		public void TestAdd () {
			var a = new float[] {1, 2};
			var b = new float[] {3, 4};
			var c = new float[2];
			DualModule.Add (a, b, c);
			var d = new float[] {4, 6};
			Assert.True (DualModule.AllEquals (c, d, float.Epsilon));
		}

		[Test()]
		public void TestSubtract() {
			var a = new float[] {1, 2};
			var b = new float[] {3, 4};
			var c = new float[2];
			DualModule.Subtract (a, b, c);
			var d = new float[] {-2, -2};
			Assert.True (DualModule.AllEquals (c, d, float.Epsilon));
		}
	}
}

