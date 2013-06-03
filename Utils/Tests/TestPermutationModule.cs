/*
PermutationModule - Permutates arrays of comparable elements.
BSD license.  
by Sven Nilsen, 2013
http://www.cutoutpro.com  
Version: 0.000 in angular degrees version notation  
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html  

Redistribution and use in source and binary forms, with or without  
modification, are permitted provided that the following conditions are met:  
1. Redistributions of source code must retain the above copyright notice, this  
list of conditions and the following disclaimer.  
2. Redistributions in binary form must reproduce the above copyright notice,  
this list of conditions and the following disclaimer in the documentation  
and/or other materials provided with the distribution.  
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND  
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED  
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE  
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR  
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES  
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;  
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT  
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS  
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.  
The views and conclusions contained in the software and documentation are those  
of the authors and should not be interpreted as representing official policies,  
either expressed or implied, of the FreeBSD Project.  
*/

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

