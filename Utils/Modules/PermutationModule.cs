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
using System.Collections.Generic;

namespace Utils
{
	public class PermutationModule
	{
		public static bool Increase<T> (T[] items) where T : IComparable {
			int n = items.Length;
			int minIndex = -1;
			for (int i = n - 2; i >= 0; --i) {
				// Find the place where the order is not reverse.
				int result = items [i].CompareTo (items[i + 1]);
				if (result >= 0) {
					continue;
				}

				minIndex = -1;
				for (int j = n - 1; j > i; --j) {
					// Ignore those who are not less than the swapped.
					if (items [j].CompareTo (items [i]) <= 0) {
						continue;
					}

					if (minIndex == -1 || items [j].CompareTo (items [minIndex]) < 0) {
						minIndex = j;
					}
				}

				// Swap the item with the smallest one.
				var tmp = items [i];
				items [i] = items [minIndex];
				items [minIndex] = tmp;

				// Sort the rest of items.
				Array.Sort<T> (items, i + 1, n - i - 1);
				return true;
			}

			return false;
		}

		public static int Compare<T> (T[] a, T[] b) where T : IComparable {
			if (a.Length != b.Length) {
				return a.Length.CompareTo (b.Length);
			}

			int n = a.Length;
			int res = 0;
			for (int i = 0; i < n; i++) {
				res = a [i].CompareTo (b [i]);
				if (res != 0) {
					return res;
				}
			}

			return 0;
		}
	}
}

