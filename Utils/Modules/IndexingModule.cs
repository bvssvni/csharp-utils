/*

IndexingModule - Useful methods for dealing with list of indices.
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
	public static class IndexingModule
	{
		public static List<int> Union(List<int> a, List<int> b)
		{
			var c = new List<int>();
			var max = int.MaxValue;
			var na = a.Count;
			var nb = b.Count;
			for (int i = 0, j = 0; i < na || j < nb;) {
				var pa = i < na ? a[i] : max;
				var pb = j < nb ? b[j] : max;
				var min = pa < pb ? pa : pb;
				if (pa == min) i++;
				if (pb == min) j++;
				
				c.Add(min);
			}
			return c;
		}

		public static List<int> Intersect(List<int> a, List<int> b)
		{
			var c = new List<int>();
			var max = int.MaxValue;
			var na = a.Count;
			var nb = b.Count;
			for (int i = 0, j = 0; i < na && j < nb;) {
				var pa = i < na ? a[i] : max;
				var pb = j < nb ? b[j] : max;
				var min = pa < pb ? pa : pb;
				if (pa == min) i++;
				if (pb == min) j++;
				if (pa == pb) c.Add(min);
			}
			return c;
		}

		public static List<int> Except(List<int> a, List<int> b)
		{
			var c = new List<int>();
			var max = int.MaxValue;
			var na = a.Count;
			var nb = b.Count;
			for (int i = 0, j = 0; i < na;) {
				var pa = i < na ? a[i] : max;
				var pb = j < nb ? b[j] : max;
				var min = pa < pb ? pa : pb;
				if (pa == min) i++;
				if (pb == min) j++;
				if (pb != min) c.Add(min);
			}
			return c;
		}

		public static int BinarySearch(List<int> a, int find)
		{
			int low = 0, high = a.Count, i;
			while (low < high) {
				i = (low + high - 1) / 2;
				if (a[i] < find) { low = i + 1; continue; };
				if (a[i] > find) { high = i; continue; };
				return i;
			}
			return -high - 1;
		}

		public static void InsertSorted(List<int> a, int item)
		{
			var ind = BinarySearch(a, item);
			if (ind >= 0) {return;}
			
			ind = -(ind + 1);
			a.Insert(ind, item);
		}
	}
}

