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
	}
}

