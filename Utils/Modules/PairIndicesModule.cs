using System;

namespace Utils
{
	/// <summary>
	/// Pair indices module.
	/// 
	/// Converts from and to an index space that stores unique number for each pair.
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
	}
}

