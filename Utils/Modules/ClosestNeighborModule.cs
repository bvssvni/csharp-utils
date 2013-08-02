using System;
using System.Collections.Generic;

namespace Utils
{
	public static class ClosestNeighborModule
	{
		public static int Closest(List<int> index, int stride, int find)
		{
			int n = index.Count;
			int ind = IndexingModule.BinarySearch(index, find);
			if (n == 0)
			{
				return -1;
			}
			if (ind >= 0)
			{
				return index[ind];
			}

			int findY = find / stride;
			int findX = find - findY * stride;

			int after = -(ind + 1);
			int before = after - 1;

			// Shrinks down as we find closer points.
			long minDist = long.MaxValue;
			int minIndex = -1;

			long afterX, afterY, beforeX, beforeY;
			long afterD, beforeD, minD;
			while (before >= 0 || after < n)
			{
				beforeY = before < 0 ? long.MinValue : index[before] / stride;
				afterY = after >= n ? long.MaxValue : index[after] / stride;
				if (beforeY <= -minDist && afterY >= minDist)
				{
					// There is no closer point left in vertical direction.
					break;
				}

				if (before < 0)
				{
					beforeD = long.MaxValue;
				}
				else
				{
					beforeX = index[before] - beforeY * stride - findX;
					beforeY -= findY;
					beforeD = beforeX * beforeX + beforeY * beforeY;
				}

				if (after >= n)
				{
					afterD = long.MaxValue;
				}
				else
				{
					afterX = index[after] - afterY * stride - findX;
					afterY -= findY;
					afterD = afterX * afterX + afterY * afterY;
				}

				minD = beforeD < afterD ? beforeD : afterD;
				if (minD == beforeD)
				{
					if (minD < minDist)
					{
						minDist = minD;
						minIndex = index[before];
					}

					before--;
				}
				else if (minD == afterD)
				{
					if (minD < minDist)
					{
						minDist = minD;
						minIndex = index[after];
					}

					after++;
				}
			}

			return minIndex;
		}

		public static List<int> WithinRadius(double radius, List<int> index, int stride, int find)
		{
			int n = index.Count;
			int ind = IndexingModule.BinarySearch(index, find);
			int findY = find / stride;
			int findX = find - findY * stride;
			
			int after = ind < 0 ? -(ind + 1) : ind;
			int before = after - 1;
			
			// Get square of radius to save operations.
			// Shrinks down as we find closer points.
			long minDist = (long)(radius * radius);
			
			long afterX, afterY, beforeX, beforeY;
			long afterD, beforeD, minD;
			var list = new List<int>();
			while (before >= 0 || after < n)
			{
				beforeY = before < 0 ? long.MinValue : index[before] / stride;
				afterY = after >= n ? long.MaxValue : index[after] / stride;
				if (beforeY <= -minDist && afterY >= minDist)
				{
					// There is no closer point left in vertical direction.
					break;
				}
				
				if (before < 0)
				{
					beforeD = long.MaxValue;
				}
				else
				{
					beforeX = index[before] - beforeY * stride - findX;
					beforeY -= findY;
					beforeD = beforeX * beforeX + beforeY * beforeY;
				}
				
				if (after >= n)
				{
					afterD = long.MaxValue;
				}
				else
				{
					afterX = index[after] - afterY * stride - findX;
					afterY -= findY;
					afterD = afterX * afterX + afterY * afterY;
				}
				
				minD = beforeD < afterD ? beforeD : afterD;
				if (minD == beforeD)
				{
					if (minD <= minDist)
					{
						list.Insert(0, index[before]);
					}
					
					before--;
				}
				else if (minD == afterD)
				{
					if (minD <= minDist)
					{
						list.Add(index[after]);
					}
					
					after++;
				}
			}
			
			return list;
		}
	}
}

