using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestNumber
	{
		[Test()]
		public void TestAddDouble()
		{
			var a = 0.5;
			var b = 0.5;
			var c = Number.Add(a, b);
			Assert.True((double)c == 1.0);
		}

		[Test()]
		public void TestSubtractDouble()
		{
			var a = 0.5;
			var b = 0.5;
			var c = Number.Subtract(a, b);
			Assert.True((double)c == 0.0);
		}

		[Test()]
		public void TestAddNumberDouble()
		{
			var a = new Number(){X = 0.5, dX = 0.0};
			var b = 0.5;
			var c = a + b;
			Assert.True((double)c.X == 1.0);
			Assert.True((double)c.dX == 0.0);
		}

		[Test()]
		public void TestSubtractNumberDouble()
		{
			var a = new Number(){X = 0.5, dX = 0.0};
			var b = 0.5;
			var c = a - b;
			Assert.True((double)c.X == 0.0);
			Assert.True((double)c.dX == 0.0);
		}

		[Test()]
		public void TestAddDoubleNumber()
		{
			var a = 0.5;
			var b = new Number(){X = 0.5, dX = 0.0};
			var c = a + b;
			Assert.True((double)c.X == 1.0);
			Assert.True((double)c.dX == 0.0);
		}

		[Test()]
		public void TestSubtractDoubleNumber()
		{
			var a = 0.5;
			var b = new Number(){X = 0.5, dX = 0.0};
			var c = a - b;
			Assert.True((double)c.X == 0.0);
			Assert.True((double)c.dX == 0.0);
		}

		[Test()]
		public void TestAddNumberNumber()
		{
			var a = new Number(){X = 0.5, dX = 0.0};
			var b = new Number(){X = 0.5, dX = 0.0};
			var c = a + b;
			Assert.True((double)c.X == 1.0);
			Assert.True((double)c.dX == 0.0);
		}

		[Test()]
		public void TestSubtractNumberNumber()
		{
			var a = new Number(){X = 0.5, dX = 0.0};
			var b = new Number(){X = 0.5, dX = 0.0};
			var c = a - b;
			Assert.True((double)c.X == 0.0);
			Assert.True((double)c.dX == 0.0);
		}

		[Test()]
		public void TestConjugate()
		{
			var a = new Number(){X = 0.5, dX = 0.4};
			var b = (Number)Number.Conjugate(a);
			Assert.True((double)b.X == 0.5);
			Assert.True((double)b.dX == -0.4);
		}

		[Test()]
		public void TestConjugateComplex()
		{
			var a = Number.Complex(0.5, 0.4);
			var b = (Number)Number.Conjugate(a);
			Assert.True(Number.Equal(b.X, 0.5));
			Assert.True(Number.Equal(b.Img, -0.4));
			Assert.True(b == Number.Complex(0.5, -0.4));
		}

		[Test()]
		public void TestConjugateQuaternion()
		{
			var a = Number.Quaternion(0.5, 0.4, 0.3, 0.2);
			var b = (Number)Number.Conjugate(a);
			Assert.True((double)b.QW == 0.5);
			Assert.True((double)b.QI == -0.4);
			Assert.True((double)b.QJ == -0.3);
			Assert.True((double)b.QK == -0.2);
			Assert.True(b == Number.Quaternion(0.5, -0.4, -0.3, -0.2));
		}

		[Test()]
		public void TestMultiplyComplex()
		{
			var a = Number.Complex(2, 0);
			var b = Number.Complex(0, 1);
			var c = a * b;
			Assert.True(Number.Equal(c.X, 0.0));
			Assert.True(Number.Equal(c.Img, 2.0));
			Assert.True(c == Number.Complex(0.0, 2.0));
			var d = c * b;
			Assert.True(d == Number.Complex(-2.0, 0.0));
			var e = d * b;
			Assert.True(e == Number.Complex(0.0, -2.0));
			var f = e * b;
			Assert.True(f == a);
		}

		[Test()]
		public void TestLengthComplex()
		{
			var a = Number.Complex(3, 4);
			var b = a * Number.Conjugate(a);
			Assert.True(b == 5 * 5);
		}

		[Test()]
		public void TestLengthQuaternion()
		{
			int s0 = 13;
			int s1 = 5;
			int s2 = 3;
			int s3 = 7;
			var a = Number.Quaternion(s0, s1, s2, s3);
			var b = a * Number.Conjugate(a);
			Assert.True(b == s0*s0 + s1*s1 + s2*s2 + s3*s3);
		}
	}
}

