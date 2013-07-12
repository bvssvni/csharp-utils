using System;

namespace Utils
{
	/// <summary>
	/// Compare indices module.
	/// 
	/// Allows conversion to and from indices from compare result.
	/// Since ICompare interface only allows int as return type,
	/// one needs to map this to index space to allow higher dimensions.
	/// 
	/// For example, in 2D we have many possible maps
	/// 
	/// 	l, r, t, b						-> 		0, 1, 2, 3
	/// 	l, r, t, b, tl, br, tr, bl		-> 		0, 1, 2, 3, 4, 5, 6
	/// 
	/// This can be organized in a such way that sorting in one dimension is possible.
	/// When we compare, we can split in positive and negative parts.
	/// 
	/// Here is a quad tree comparison that can also be sorted in one dimension:
	/// 
	/// 	-----------------
	/// 	|	-1	|	1	|
	/// 	-----------------
	/// 	|	-2	|	2	|
	/// 	-----------------
	/// 
	/// The geometrical mapping is not significant as long we can map the spaces.
	/// 
	/// 	-5	-4	-3	-2	-1	0	1	2	3	4	5
	/// 	8	6	4	2	0		1	3	5	7	9
	/// 
	/// </summary>
	public static class CompareIndicesModule
	{
		public static int ToIndex (int c) {
			return c > 0 ? c + c - 1 : -c - c - 2;
		}

		public static int ToCompare (int i) {
			return (i & 1) == 0 ? (-i - 2) >> 1 : (i + 1) >> 1;
		}

	}
}

