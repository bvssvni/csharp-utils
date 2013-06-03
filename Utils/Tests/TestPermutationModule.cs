using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestPermutationModule
	{
		private bool CompareArrays (int[] a, int[] b) {
			if (a.Length != b.Length) {
				return false;
			}

			for (int i = 0; i < a.Length; i++) {
				if (a[i] != b[i]) {
					return false;
				}
			}

			return true;
		}

		private void PrintArray (int[] a) {
			for (int i = 0; i < a.Length; i++) {
				Console.WriteLine (a[i]);
			}
		}

		[Test()]
		public void TestCase()
		{
			var arr = new int[] {0, 1, 2, 3};
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {0, 1, 3, 2}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {0, 2, 1, 3}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {0, 2, 3, 1}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {0, 3, 1, 2}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {0, 3, 2, 1}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {1, 0, 2, 3}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {1, 0, 3, 2}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {1, 2, 0, 3}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {1, 2, 3, 0}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {1, 3, 0, 2}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {1, 3, 2, 0}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {2, 0, 1, 3}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {2, 0, 3, 1}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {2, 1, 0, 3}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {2, 1, 3, 0}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {2, 3, 0, 1}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {2, 3, 1, 0}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {3, 0, 1, 2}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {3, 0, 2, 1}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {3, 1, 0, 2}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {3, 1, 2, 0}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {3, 2, 0, 1}));
			Assert.True (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {3, 2, 1, 0}));
			Assert.False (PermutationModule.Increase (arr));
			Assert.True (CompareArrays (arr, new int[] {3, 2, 1, 0}));
		}
	}
}

