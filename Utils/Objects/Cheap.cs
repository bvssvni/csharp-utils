/*
Cheap - Precise memory management for C#.
BSD license.
by Sven Nilsen, 2012
http://www.cutoutpro.com
Version: 0.001 in angular degrees version notation
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html

0.002 - Changed constructor to static method.
0.001 - Added Semaphore and action.

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

using System;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// Uses static array to store data for precise memory management.
	/// Exposes the static array by giving out start and end indices.
	/// Not thread safe.
	/// 
	/// Call 'Dispose' release data.
	/// Remember to call 'Cheap<T>.Defragment()' occationally to free memory.
	/// 
	/// For structs: Reduces fragmentation of memory.
	/// For objects: Allows objects to be released under controlled conditions.
	/// </summary>
	public sealed class Cheap<T> : IDisposable
	{
		private const int MIN_ITEM_BUFFER_SIZE = 1024;
		private const int MIN_SLICE_BUFFER_SIZE = 128;
		
		private int pos;
		private UInt64 id;
		private bool disposed = false;

		// Defragmentation is only allowed when the semaphore is 0.
		public static int Semaphore;
		private static UInt64 LastId;
		public static T[] Items;
		private static Slice[] Slices;
		private static int ItemLength;
		// Contains the number of slices added to array.
		private static int SliceLength;
		// Contains the slice position of fragmentation start.
		// This speeds up application when there is 
		private static int SliceOffset;
		
		private struct Slice {
			public UInt64 Id;
			public int Offset;
			public int Count;
		}
		
		static Cheap() {
			Items = new T[MIN_ITEM_BUFFER_SIZE];
			Slices = new Slice[MIN_SLICE_BUFFER_SIZE];
			ItemLength = 0;
			SliceLength = 0;
			LastId = 0;
			SliceOffset = 0;
			Semaphore = 0;
		}

		public delegate void CheapAction(ref T item);

		public void ForEach(CheapAction action) {
			// Prevent defragmentation while we do the actions.
			Semaphore++;
			int start = 0;
			int end = 0;
			GetRange(ref start, ref end);
			for (int i = start; i < end; i++) {
				action(ref Items[i]);
			}
			Semaphore--;
		}
		
		/// <summary>
		/// Packs the lists together in memory.
		/// </summary>
		public static void Defragment() {
			if (Semaphore > 0) return;

			int moveSlices = 0;
			int moveItems = 0;
			int slice_length = SliceLength;
			int p;
			for (int i = SliceOffset; i < slice_length; ++i) {
				var slice = Slices[i];
				if (slice.Id == UInt64.MaxValue) {
					// Found a deleted slice.
					// Items in slices after this moves 
					moveItems += slice.Count;
					++moveSlices;
				} else {
					// Move the items of the slices.
					Slices[i - moveSlices] = Slices[i];
					Slices[i - moveSlices].Offset -= moveItems;
					for (int j = 0; j < slice.Count; ++j) {
						p = slice.Offset + j;
						Items[p - moveItems] = Items[p];
						Items[p] = default(T);
					}
				}
			}

			for (int i = 0; i < moveSlices; i++) {
				// Delete the old moved slice.
				Slices[slice_length - i - 1].Id = UInt64.MaxValue;
			}

			SliceLength -= moveSlices;
			ItemLength -= moveItems;
			SliceOffset = SliceLength;

			if (moveSlices > 0) {
				// Resize slice array down if taking up less than 50% of buffer.
				if (SliceLength < (MIN_SLICE_BUFFER_SIZE >> 1) && Slices.Length > (MIN_SLICE_BUFFER_SIZE << 1)) {
					Array.Resize<Slice>(ref Slices, MIN_SLICE_BUFFER_SIZE);
				} else if (SliceLength >= MIN_SLICE_BUFFER_SIZE && SliceLength < (Slices.Length >> 1)) {
					Array.Resize<Slice>(ref Slices, SliceLength << 1);
				}
			}

			if (moveItems > 0) {
				// Resize slice array down if taking up less than 50% of buffer.
				if (ItemLength < (MIN_ITEM_BUFFER_SIZE >> 1) && Items.Length > (MIN_ITEM_BUFFER_SIZE << 1)) {
					Array.Resize<T>(ref Items, MIN_ITEM_BUFFER_SIZE);
				} else if (ItemLength >= MIN_ITEM_BUFFER_SIZE && ItemLength < (Items.Length >> 1)) {
					Array.Resize<T>(ref Items, ItemLength << 1);
				}
			}
		}
		
		public bool GetRange(ref int start, ref int end)
		{
			// Get the correct position in case of defragmentation.
			int pos = this.pos;
			while (Slices[pos].Id > this.id) --pos;
			
			if (Slices[pos].Id != this.id) return false;
			
			this.pos = pos;
			var slice = Slices[pos];
			start = slice.Offset;
			end = slice.Offset + slice.Count;
			return true;
		}

		public static Cheap<T> WithCapacity (int capacity) {
			Cheap<T> ch = new Cheap<T> ();
			
			// Resize items array if necessary.
			if (capacity + ItemLength > Items.Length) {
				Array.Resize<T>(ref Items, (capacity + ItemLength) << 1);
			}
			
			// Resize the slice array if necessary.
			if (SliceLength + 1 > Slices.Length) {
				Array.Resize<Slice>(ref Slices, Slices.Length << 1);
			}
			
			// Insert data.
			int start = ItemLength;
			// Move defragment offset if adding right after it.
			if (SliceOffset == SliceLength) ++SliceOffset;
			
			ItemLength += capacity;
			ch.pos = SliceLength++;
			Slices[ch.pos] = new Slice() {
				Offset = start, 
				Count = capacity, 
				Id = ch.id = LastId++
			};
			
			return ch;
		}

		public static Cheap<T> FromArray (params T[] data) {
			Cheap<T> ch = new Cheap<T> ();
			int n = data.Length;
			
			// Resize items array if necessary.
			if (n + ItemLength > Items.Length) {
				Array.Resize<T>(ref Items, (n + ItemLength) << 1);
			}
			
			// Resize the slice array if necessary.
			if (SliceLength + 1 > Slices.Length) {
				Array.Resize<Slice>(ref Slices, Slices.Length << 1);
			}
			
			// Insert data.
			int start = ItemLength;
			// Move defragment offset if adding right after it.
			if (SliceOffset == SliceLength) ++SliceOffset;
			
			ItemLength += n;
			ch.pos = SliceLength++;
			data.CopyTo(Items, start);
			Slices[ch.pos] = new Slice() {
				Offset = start, 
				Count = n, 
				Id = ch.id = LastId++
			};

			return ch;
		}
		
		public void Dispose() {
			if (disposed) return;
			
			disposed = true;
			// Get the correct position in case of defragmentation.
			int pos = this.pos;
			while (Slices[pos].Id > this.id) --pos;

			// Flag the slice as deleted.
			Slices[pos].Id = UInt64.MaxValue;
			if (pos < SliceOffset) {
				SliceOffset = pos;
			}
			// When removing from end, update settings directly.
			// In case of removing and adding large objects, prevents reallocation of buffer.
			if (Slices[pos].Offset + Slices[pos].Count == ItemLength) {
				ItemLength -= Slices[pos].Count;
				--SliceLength;
			}
			
			GC.SuppressFinalize(this);
		}
		
		~Cheap() {
			Dispose();
		}
	}
}

