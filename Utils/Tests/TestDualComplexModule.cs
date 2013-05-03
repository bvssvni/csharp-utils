using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestDualComplexModule
	{
		[Test()]
		public void TestSum()
		{
			var list = Cheap<DualF>.FromArray (new DualF (1, 2), new DualF (2, 1));
			var items = Cheap<DualF>.Items;
			var sum = DualModule.Sum (list, 2);
			int start = 0, end = 0;
			sum.GetRange (ref start, ref end);
			Assert.True (end - start == 1);
			Assert.True (items[start].X == 3);
			Assert.True (items[start].Dx == 3);

			list.Dispose ();
			sum.Dispose ();
			Cheap<DualF>.Defragment ();
		}
	}
}

