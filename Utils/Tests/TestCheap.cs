using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestCheap
	{
		[Test()]
		public void Test()
		{
			var a = new Cheap<int>(1, 2);
			int start = 0;
			int end = 0;
			Assert.True(a.GetRange(ref start, ref end));
			Assert.True(start == 0);
			Assert.True(end == 2);
			var b = new Cheap<int>(3, 4);
			Assert.True(b.GetRange(ref start, ref end));
			Assert.True(start == 2);
			Assert.True(end == 4);
			a.Dispose();
			Assert.True(b.GetRange(ref start, ref end));
			Assert.True(start == 2);
			Assert.True(end == 4);
			Cheap<int>.Defragment();
			Assert.True(b.GetRange(ref start, ref end));
			Assert.True(start == 0);
			Assert.True(end == 2);
			
			/*
			long startTime = DateTime.Now.ToFileTimeUtc();
			for (int i = 0; i < (1 << 19); i++) {
				var c = new Cheap<int>(new int[1024]);
				c.Dispose();
				if (Cheap<int>.Items.Length > (1 << 20)) {
					System.GC.Collect();
					Cheap<int>.Defragment();
				}
			}
			long diffTime = DateTime.Now.ToFileTimeUtc() - startTime;
			Console.WriteLine(diffTime * 100e-9);
			Assert.False(true);
			*/
		}
	}
}

