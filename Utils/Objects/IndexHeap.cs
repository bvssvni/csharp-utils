// Credits: Thanks to David Cumps.
// http://weblogs.asp.net/cumpsd/archive/2005/02/13/371719.aspx

using System;

namespace Utils
{
	public class IndexHeap
	{
		public int Cursor = 1;
		private int[] m_items;
		public CompareDelegate Compare;
		
		public int Count {
			get {
				return Cursor - 1;
			}
		}
		
		public delegate int CompareDelegate(int a, int b);
		
		public IndexHeap(int capacity)
		{
			// Add one extra because 0 is not used.
			m_items = new int[capacity + 1];
		}
		
		public void Push(int obj) {
			this.m_items[this.Cursor] = obj;
			var bubbleIndex = this.Cursor;
			int parentIndex;
			int tmpValue;
			while (bubbleIndex != 1) {
				parentIndex = bubbleIndex / 2;
				if (Compare(this.m_items[bubbleIndex], this.m_items[parentIndex]) <= 0) {
					tmpValue = this.m_items[parentIndex];
					this.m_items[parentIndex] = this.m_items[bubbleIndex];
					this.m_items[bubbleIndex] = tmpValue;
					bubbleIndex = parentIndex;
				} else {
					break;
				}
			}             
			
			this.Cursor++;
		}
		
		public int Pop() {
			this.Cursor--;
			var returnItem = this.m_items[1];
			this.m_items[1] = this.m_items[this.Cursor];
			
			int swapItem = 1, parent = 1;
			int tmpIndex;
			do {
				parent = swapItem;
				if ((2 * parent + 1) <= this.Cursor) {
					// Both children exist.
					if (Compare(this.m_items[parent], this.m_items[2 * parent]) >= 0) {
						swapItem = 2 * parent;
					}
					if (Compare(this.m_items[swapItem], this.m_items[2 * parent + 1]) >= 0) {
						swapItem = 2 * parent + 1;
					}
				} else if ((2 * parent) <= this.Cursor) {
					// Only one child exists
					if (Compare(this.m_items[parent], this.m_items[2 * parent]) >= 0) {
						swapItem = 2 * parent;
					}
				}
				
				// One if the parent's children are smaller or equal, swap them.
				if (parent != swapItem) {
					tmpIndex = this.m_items[parent];
					this.m_items[parent] = this.m_items[swapItem];
					this.m_items[swapItem] = tmpIndex;
				}
			} while (parent != swapItem);
			
			return returnItem;
		}
	}
}

