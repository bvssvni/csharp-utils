using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestLineIterationModule
	{
		[Test()]
		public void TestHorizontal()
		{
			int i = 0;
			int[] expectedPos = new int[] {
				0, 0,
				1, 0,
			};
			foreach (var it in LineIterationModule.FourConnected (0, 0, 2, 0)) {
				Assert.True (expectedPos [2*i] == it.X);
				Assert.True (expectedPos [2*i+1] == it.Y);
				i++;
			}

			i = 0;
			foreach (var it in LineIterationModule.EightConnected (0, 0, 2, 0)) {
				Assert.True (expectedPos [2*i] == it.X);
				Assert.True (expectedPos [2*i+1] == it.Y);
				i++;
			}
		}

		[Test()]
		public void TestDiagonalFourConnected()
		{
			int i = 0;
			int[] expectedPos = new int[] {
				0, 0,
				1, 0,
				1, 1,
				2, 1,
			};
			foreach (var it in LineIterationModule.FourConnected (0, 0, 2, 2)) {
				// TEST
				Console.WriteLine ("{0} {1}", it.X, it.Y);

				Assert.True (expectedPos [2*i] == it.X);
				Assert.True (expectedPos [2*i+1] == it.Y);
				i++;
			}
		}

		[Test()]
		public void TestDiagonalEightConnected()
		{
			int i = 0;
			int[] expectedPos = new int[] {
				0, 0,
				1, 1,
			};
			foreach (var it in LineIterationModule.EightConnected (0, 0, 2, 2)) {
				Assert.True (expectedPos [2*i] == it.X);
				Assert.True (expectedPos [2*i+1] == it.Y);
				i++;
			}
		}

		[Test()]
		public void TestDiagonalFourteenConnected () {
			int i = 0;
			int[] expectedPos = new int[] {
				0, 0, 0,
				1, 1, 1,
			};
			foreach (var it in LineIterationModule.FourteenConnected (0, 0, 0, 2, 2, 2)) {
				Assert.True (expectedPos [3*i] == it.X);
				Assert.True (expectedPos [3*i+1] == it.Y);
				Assert.True (expectedPos [3*i+2] == it.Z);
				i++;
			}
		}

	}
}

