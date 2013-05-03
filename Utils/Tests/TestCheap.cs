using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestCheap
	{
		[Test()]
		public void Test1()
		{
			var a = Cheap<int>.FromArray (1, 2);
			int start = 0;
			int end = 0;
			Assert.True(a.GetRange(ref start, ref end));
			Assert.True(start == 0);
			Assert.True(end == 2);
			var b = Cheap<int>.FromArray (3, 4);
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
			b.Dispose();
			Cheap<int>.Defragment();

			/*/
			// We are creating a large object that makes the buffer reallocate,
			// but when we remove it
			long startTime = DateTime.Now.ToFileTimeUtc();
			for (int i = 0; i < (1 << 22); i++) {
				var c = new Cheap<int>(1, 2, 3);
				c.Dispose();
			}
			long diffTime = DateTime.Now.ToFileTimeUtc() - startTime;
			Console.WriteLine(diffTime * 100e-9);
			Assert.False(true);
			//*/

		}

		[Test()]
		public void Test2() {
			var a = Cheap<int>.FromArray (1, 2);
			var b = Cheap<int>.FromArray (3, 4);
			a.Dispose();
			Cheap<int>.Semaphore++;
			// This will not have any effect since we are preventing
			// it from defragmenting.
			Cheap<int>.Defragment();
			Assert.True(Cheap<int>.Items[0] == 1);
			Assert.True(Cheap<int>.Items[1] == 2);
			Cheap<int>.Semaphore--;

			Cheap<int>.Defragment();
			Assert.True(Cheap<int>.Items[0] == 3);
			Assert.True(Cheap<int>.Items[1] == 4);

			b.Dispose();
			Cheap<int>.Defragment();
		}

		[Test()]
		public void Test3() {
			var a = Cheap<int>.FromArray (1, 2);
			var b = Cheap<int>.FromArray (3, 4);
			a.ForEach((ref int item) => item = 5);
			Assert.True(Cheap<int>.Items[0] == 5);
			Assert.True(Cheap<int>.Items[1] == 5);
			Assert.True(Cheap<int>.Items[2] == 3);
			Assert.True(Cheap<int>.Items[3] == 4);
			b.ForEach((ref int item) => item = 7);
			Assert.True(Cheap<int>.Items[0] == 5);
			Assert.True(Cheap<int>.Items[1] == 5);
			Assert.True(Cheap<int>.Items[2] == 7);
			Assert.True(Cheap<int>.Items[3] == 7);
			a.Dispose();
			b.Dispose();
			Cheap<int>.Defragment();
		}
	}
}

