using System;
using NUnit.Framework;

namespace Utils
{
	[TestFixture()]
	public class TestBinaryHeap
	{
		[Test()]
		public void TestBinaryHeap1()
		{
			var heap = new BinaryHeap<double>(10);
			heap.Push (5);
			heap.Push (4);
			Assert.True(heap.Pop() == 4);
			Assert.True(heap.Pop() == 5);

			heap.Push (12);
			heap.Push (20);
			heap.Push (30);
			heap.Push (50);
			heap.Push (2);
			Assert.True(heap.Pop() == 2);
		}

		[Test()]
		public void TestBinaryHeapCapacity()
		{
			var heap = new BinaryHeap<int>(2);
			heap.Push(0);
			heap.Push(1);
		}
	}
}

