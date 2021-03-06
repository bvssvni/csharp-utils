 /*
BinaryHeap - Heap for finding minimum item.  
BSD license.  
by Sven Nilsen, 2013
http://www.cutoutpro.com  
Version: 0.001 in angular degrees version notation  
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html  

0.001 - Added 'Peek'.

Redistribution and use in source and binary forms, with or without  
modification, are permitted provided that the following conditions are met:  
1. Redistributions of source code must retain the above copyright notice, this  
list of conditions and the following disclaimer.  
2. Redistributions in binary form must reproduce the above copyright notice,  
this list of conditions and the following disclaimer in the documentation  
and/or other materials provided with the distribution.  
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND  
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED  
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE  
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR  
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES  
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;  
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT  
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS  
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.  
The views and conclusions contained in the software and documentation are those  
of the authors and should not be interpreted as representing official policies,  
either expressed or implied, of the FreeBSD Project.  
*/

// Credits: Thanks to David Cumps.
// http://weblogs.asp.net/cumpsd/archive/2005/02/13/371719.aspx

using System;

namespace Utils
{
	public class BinaryHeap<T> where T : IComparable
	{
		public int Cursor = 1;
		private T[] m_items;
		
		public int Count {
			get {
				return Cursor - 1;
			}
		}
		
		public BinaryHeap (int capacity)
		{
			// Add one extra because 0 is not used.
			m_items = new T[capacity + 1];
		}
		
		public void Push(T obj) {
			this.m_items[this.Cursor] = obj;
			var bubbleIndex = this.Cursor;
			int parentIndex;
			T tmpValue;
			while (bubbleIndex != 1) {
				parentIndex = bubbleIndex / 2;
				if (this.m_items[bubbleIndex].CompareTo(this.m_items[parentIndex]) <= 0) {
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

		public T Peek() {
			return this.m_items[1];
		}
		
		public T Pop() {
			this.Cursor--;
			var returnItem = this.m_items[1];
			this.m_items[1] = this.m_items[this.Cursor];
			
			int swapItem = 1, parent = 1;
			T tmpIndex;
			do {
				parent = swapItem;
				if ((2 * parent + 1) <= this.Cursor) {
					// Both children exist.
					if (this.m_items[parent].CompareTo(this.m_items[2 * parent]) >= 0) {
						swapItem = 2 * parent;
					}
					if (this.m_items[swapItem].CompareTo(this.m_items[2 * parent + 1]) >= 0) {
						swapItem = 2 * parent + 1;
					}
				} else if ((2 * parent) <= this.Cursor) {
					// Only one child exists
					if (this.m_items[parent].CompareTo(this.m_items[2 * parent]) >= 0) {
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

