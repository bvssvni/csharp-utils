// Credits: Thanks to David Cumps.
// http://weblogs.asp.net/cumpsd/archive/2005/02/13/371719.aspx

using System;

namespace Utils
{
	public class BinaryHeap<T> where T : IComparable
	{
		private int m_count = 1;
		private T[] m_items;
		
		public int Count {
			get {
				return m_count - 1;
			}
		}
		
		public BinaryHeap (int capacity)
		{
			// Add one extra because 0 is not used.
			m_items = new T[capacity + 1];
		}
		
		public void Push(T obj) {
			this.m_items[this.m_count] = obj;
			var bubbleIndex = this.m_count;
			while (bubbleIndex != 1) {
				var parentIndex = bubbleIndex / 2;
				if (this.m_items[bubbleIndex].CompareTo(this.m_items[parentIndex]) <= 0) {
					var tmpValue = this.m_items[parentIndex];
					this.m_items[parentIndex] = this.m_items[bubbleIndex];
					this.m_items[bubbleIndex] = tmpValue;
					bubbleIndex = parentIndex;
				} else {
					break;
				}
			}             
			
			this.m_count++;
		}
		
		public T Pop() {
			this.m_count--;
			var returnItem = this.m_items[1];
			this.m_items[1] = this.m_items[this.m_count];
			
			int swapItem = 1, parent = 1;
			do {
				parent = swapItem;
				if ((2 * parent + 1) <= this.m_count) {
					// Both children exist.
					if (this.m_items[parent].CompareTo(this.m_items[2 * parent]) >= 0) {
						swapItem = 2 * parent;
					}
					if (this.m_items[swapItem].CompareTo(this.m_items[2 * parent + 1]) >= 0) {
						swapItem = 2 * parent + 1;
					}
				} else if ((2 * parent) <= this.m_count) {
					// Only one child exists
					if (this.m_items[parent].CompareTo(this.m_items[2 * parent]) >= 0) {
						swapItem = 2 * parent;
					}
				}
				
				// One if the parent's children are smaller or equal, swap them.
				if (parent != swapItem) {
					var tmpIndex = this.m_items[parent];
					this.m_items[parent] = this.m_items[swapItem];
					this.m_items[swapItem] = tmpIndex;
				}
			} while (parent != swapItem);
			
			return returnItem;
		}
	}
}

