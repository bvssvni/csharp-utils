using System;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// Pair indices module.
	/// 
	/// Converts from and to an index space that stores unique number for each pair.
	/// This index space can be expanded continiously.
	/// For each pair, the following is true:
	/// 
	/// 	x < y
	/// 
	/// </summary>
	public static class PairIndicesModule
	{
		public static void Position (int index, out int x, out int y) {
			y = (int)((-1 + Math.Sqrt (8 * index + 1)) / 2) + 1;
			x = index - y * (y + 1) / 2 + y;
		}

		public static int Index (int x, int y) {
			return x + y * (y - 1) / 2;
		}
		
		public static int Count (int n) {
			return n * (n - 1) / 2;
		}

		public delegate T GenerateDelegate<T> (int i, int j);

		public static List<T> Generate<T> (int n, GenerateDelegate<T> f) {
			var list = new List<T> (Count (n));
			for (int i = 0; i < n; i++) {
				for (int j = 0; j < i; j++) {
					list.Add (f (j, i));
				}
			}

			return list;
		}
	}
}

