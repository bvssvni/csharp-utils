/*
Play - Group-oriented programming for C#.  
BSD license.  
by Sven Nilsen, 2012  
http://www.cutoutpro.com  
Version: 0.000 in angular degrees version notation  
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html  

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
using System.Text;

namespace Utils
{
	// A group is a object loose coupled with data that precalculates a truth value about some information.
	// It is both a way to extract information out of data and a way to access data faster than by looking for it.
	public class Group : List<int>, IComparable
	{
		public Group()
		{

		}

		public Group(int[] array) : base(array)
		{
		}

		public Group (Group copyFrom) : base(copyFrom.ToArray())
		{
		}

		public Group (int capacity) : base(capacity)
		{
		}

		public override string ToString()
		{
			var strb = new StringBuilder();
			int n = this.Count;
			for (int i = 0; i < n; i++) {
				strb.Append(this[i].ToString());
				if (i < n - 1) strb.Append(", ");
			}
			return strb.ToString();
		}

		/// <summary>
		/// Creates a group using boolean samples of data.
		/// </summary>
		/// <returns>
		/// A group describing the truth intervals in the samples.
		/// </returns>
		/// <param name='data'>
		/// Data.
		/// </param>
		public static Group FromBoolSamples(bool[] data, bool invert = false)
		{
			return Predicate<bool>(delegate(bool item) {
				return item ^ invert;
			}, data);
		}

		/// <summary>
		/// Creates a group from an ordered map.
		/// 
		/// The indices in the map has to increase.
		/// Gaps are added to the group when one index differs more from the previous than 1.
		/// </summary>
		/// <returns>
		/// Returns a group corresponding to a sorted map.
		/// </returns>
		/// <param name='map'>
		/// The ordered map to create group from.
		/// </param>
		public static Group FromOrderedMap(int[] map)
		{
			var g = new Group();
			int n = map.Length;
			int last = map[0];
			g.Add (last);
			for (int i = 1; i < n; i++) {
				if (map[i] != last + 1) {
					g.Add (last);
					g.Add (map[i]); 
				}
				last = map[i];
			}
			if ((g.Count & 1) == 1) g.Add (map[map.Length-1] + 1);

			return g;
		}

		/// <summary>
		/// Creates a group by examining each possibility in a potential.
		/// A potential is an interval where each index represents a state.
		/// Usually this is very simple structures, because the space grows rapidly.
		/// </summary>
		/// <returns>
		/// Returns the group where all members are possibilities where the condition is true.
		/// </returns>
		/// <param name='first'>
		/// The first index in the potential.
		/// </param>
		/// <param name='last'>
		/// The last index in the potential.
		/// </param>
		/// <param name='f'>
		/// A function that returns true or false for each possible state.
		/// </param>
		public static Group FromPotential(int first, int last, IsTrue<int> f)
		{
			var g = new Group();
			bool was = false;
			bool has = false;
			for (int i = first; i <= last; i++) {
				has = f(i);
				if (has != was) g.Add (i);

				was = has;
			}
			if ((g.Count & 1) == 1) g.Add (last+1);

			return g;
		}

		/// <summary>
		/// Finds the largest interval in the group.
		/// </summary>
		/// <returns>
		/// The largest interval in the group.
		/// </returns>
		public Group MaxInterval()
		{
			int max = int.MinValue;
			int maxStart = 0;
			int maxEnd = 0;
			int n = this.Count/2;
			for (int i = 0; i < n; i++) {
				int start = this[2*i];
				int end = this[2*i+1];
				int interval = end - start;
				if (interval > max) {
					max = interval;
					maxStart = start;
					maxEnd = end;
				}
			}

			if (max == int.MinValue) return null;

			return new Group(new int[]{maxStart, maxEnd});
		}

		/// <summary>
		/// Finds the maximum leap, starting at 0.
		/// </summary>
		/// <returns>
		/// Returns the maximum leap in group.
		/// </returns>
		public Group MaxLeap()
		{
			int max = int.MinValue;
			int maxStart = 0;
			int maxEnd = 0;
			int n = this.Count/2;
			for (int i = 0; i < n; i++) {
				int j = 2*i - 1;
				int start = j < 0 ? 0 : this[j];
				int end = this[2*i];
				int interval = end - start;
				if (interval > max) {
					max = interval;
					maxStart = start;
					maxEnd = end;
				}
			}
			
			if (max == int.MinValue) return null;
			
			return new Group(new int[]{maxStart, maxEnd});
		}

		/// <summary>
		/// Finds the smallest interval in the group.
		/// </summary>
		/// <returns>
		/// Returns the smallest interval in the group.
		/// </returns>
		public Group MinInterval()
		{
			int min = int.MaxValue;
			int minStart = 0;
			int minEnd = 0;
			int n = this.Count/2;
			for (int i = 0; i < n; i++) {
				int start = this[2*i];
				int end = this[2*i+1];
				int interval = end - start;
				if (interval < min) {
					min = interval;
					minStart = start;
					minEnd = end;
				}
			}

			if (min == int.MaxValue) return null;

			return new Group(new int[]{minStart, minEnd});
		}

		/// <summary>
		/// Finds the minimum leap, starting at 0.
		/// </summary>
		/// <returns>
		/// The minimum leap in group.
		/// </returns>
		public Group MinLeap()
		{
			int min = int.MaxValue;
			int minStart = 0;
			int minEnd = 0;
			int n = this.Count/2;
			for (int i = 0; i < n; i++) {
				int j = 2*i-1;
				int start = j < 0 ? 0 : this[j];
				int end = this[2*i];
				int interval = end - start;
				if (interval < min) {
					min = interval;
					minStart = start;
					minEnd = end;
				}
			}
			
			if (min == int.MaxValue) return null;
			
			return new Group(new int[]{minStart, minEnd});
		}

		public delegate bool IsTrue<T>(T item);

		/// <summary>
		/// A predicate is a function that returns true or false.
		/// This method constructs a group from feeding a function one by one with data.
		/// The indices in the list of data is used to set the group.
		/// </summary>
		/// <param name='func'>
		/// A predicate function.
		/// </param>
		/// <param name='data'>
		/// The data to check for condition.
		/// </param>
		/// <typeparam name='T'>
		/// The type of data in the list.
		/// </typeparam>
		public static Group Predicate<T>(IsTrue<T> func, IList<T> data)
		{
			Group g = new Group();
			int n = data.Count;
			bool has = false;
			bool was = false;
			for (int i = 0; i < n; i++) {
				var a = data [i];
				has = func(a);
				if (has != was) {
					g.Add(i);
				}
				was = has;
			}
			if (was) {
				g.Add(n);
			}
			return g;
		}

		public delegate bool IsTrueByIndex(int index);

		/// <summary>
		/// Creates a group filtered by a function using the index as argument.
		/// Use closures to access data in addition to the index.
		/// </summary>
		/// <param name='g'>
		/// The group to filter.
		/// </param>
		/// <param name='func'>
		/// A function that takes an index as argument and returns a bool value.
		/// </param>
		public static Group Filter(Group g, IsTrueByIndex func)
		{
			if (g == null)
				return null;

			Group res = new Group();
			bool was = false;
			bool has = false;
			int n = g.Count / 2;
			for (int i = 0; i < n; i++) {
				was = false;
				has = false;

				int start = g [i * 2];
				int end = g [i * 2 + 1];
				for (int j = start; j < end; j++) {
					has = func(j);
					if (has != was) {
						res.Add(j);
					}
					was = has;
				}

				if (was) {
					res.Add(end);
				}
			}

			return res;
		}

		public static Group operator *(Group a, IsTrueByIndex func)
		{
			return Group.Filter(a, func);
		}

		/// <summary>
		/// Adds a member to a group.
		/// </summary>
		/// <param name='a'>
		/// The group to add the member to.
		/// </param>
		/// <param name='id'>
		/// The id of the new member.
		/// </param>
		public static Group Add(Group a, int id)
		{
			var b = new Group();
			b.Add(id);
			b.Add(id + 1);
			return a + b;
		}

		public static Group operator +(Group a, int id)
		{
			return Group.Add(a, id);
		}

		public static Group Subtract(Group a, int id)
		{
			var b = new Group();
			b.Add (id);
			b.Add (id + 1);
			return a - b;
		}

		public static Group operator -(Group a, int id)
		{
			return Group.Subtract (a, id);
		}

		// Returns the number of members in the group.
		[PerformanceLevel(29)]
		public static int Size(Group a)
		{
			int size = 0;
			int n = a.Count - 1;
			for (int i = 0; i < n; i += 2) size += a[i+1] - a[i];

			return size;
		}

		public int CompareTo(object obj)
		{
			// If the object is not a group, compare by size.
			if (!(obj is Group))
				return Group.Size(this).CompareTo(obj);

			// Compare groups.
			var a = this;
			var b = (Group)obj;
			var na = a.Count;
			var nb = b.Count;

			if (na != nb)
				return na.CompareTo(nb);

			for (int i = 0; i < na; i++) {
				if (a [i] < b [i])
					return -1;
				if (a [i] > b [i])
					return 1;
			}
			return 0;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Returns true if the group is empty, which includes null.
		public static bool IsEmpty(Group a)
		{
			if (a == null || a.Count == 0)
				return true;

			return false;
		}

		// Intersects two groups and creates a new one.
		public static Group Intersect(Group a, Group b)
		{
			Group arr = new Group();

			int alength = a.Count;
			int blength = b.Count;
			if (alength == 0 || blength == 0)
				return arr;

			int i = 0, j = 0; 
			bool isA = false; 
			bool isB = false; 
			bool was = false;
			bool has = false;
			int pa, pb, min;
			while (i < alength && j < blength) {
				// Get the last value from each group.
				pa = i >= alength ? int.MaxValue : a [i];
				pb = j >= blength ? int.MaxValue : b [j];
				min = pa < pb ? pa : pb;

				// Advance the one with least value, both if they got the same.
				if (pa == min) {
					isA = !isA; 
					i++;
				}
				if (pb == min) {
					isB = !isB;
					j++;
				}

				// Find out if the new change should be added to the result.
				has = isA && isB;
				if (has != was)
					arr.Add(min);

				was = has;
			}
			
			return arr;
		}

		/// <summary>
		/// Creates a union of 'a'  and 'b'.
		/// The returned group contains all members of 'a' and 'b'.
		/// </summary>
		/// <param name='a'>
		/// The first group to join.
		/// </param>
		/// <param name='b'>
		/// The second group to join.
		/// </param>
		public static Group Union(Group a, Group b)
		{
			Group list = new Group();

			int a_length = a.Count;
			int b_length = b.Count;

			if (a_length == 0 && b_length == 0)
				return list;

			if (a_length == 0) {
				list.AddRange(b);
				
				return list;
			}
			if (b_length == 0) {
				list.AddRange(a);
				
				return list;
			}

			int i = 0, j = 0; 
			bool isA = false; 
			bool isB = false; 
			bool was = false;
			bool has = false;
			int pa, pb, min;
			while (i < a_length || j < b_length) {
				// Get the least value.
				pa = i >= a_length ? int.MaxValue : a [i];
				pb = j >= b_length ? int.MaxValue : b [j];
				min = pa < pb ? pa : pb;

				// Advance the least value, both if both are equal.
				if (pa == min) {
					isA = !isA;
					i++;
				}
				if (pb == min) {
					isB = !isB;
					j++;
				}

				// Add to result if this changes the truth value.
				has = isA || isB;
				if (has != was)
					list.Add(min);
				
				was = isA || isB;
			}
			
			return list;
		}
	
		// Removes all members of _b_ from group _a_.
		/// <summary>
		/// Creates a group that contains member of 'a' but not 'b'.
		/// </summary>
		/// <param name='a'>
		/// The group with the members to choose from.
		/// </param>
		/// <param name='b'>
		/// The group that we should not have members from.
		/// </param>
		public static Group Subtract(Group a, Group b)
		{
			int a_length = a.Count;
			int b_length = b.Count;
			if (b_length == 0) {
				Group c = new Group(a.ToArray());
				return c;
			}

			Group arr = new Group();
			
			if (a_length == 0 || b_length == 0)
				return arr;
			
			int i = 0, j = 0; 
			bool isA = false; 
			bool isB = false;
			bool was = false;
			bool has = false;
			int pa, pb, min; 
			while (i < a_length) {
				// Get the last value from each group.
				pa = i >= a_length ? int.MaxValue : a [i];
				pb = j >= b_length ? int.MaxValue : b [j];
				min = pa < pb ? pa : pb;

				// Advance the group with least value, both if they are equal.
				if (pa == min) {
					isA = !isA;
					i++;
				}
				if (pb == min) {
					isB = !isB;
					j++;
				}

				// If it changes the truth value, add to result.
				has = isA && !isB;
				if (has != was)
					arr.Add(min);

				was = has;
			}
			
			return arr;
		}

		public static Group operator +(Group a, Group b)
		{
			return Group.Union(a, b);
		}

		public static Group operator *(Group a, Group b)
		{
			return Group.Intersect(a, b);
		}

		public static Group operator -(Group a, Group b)
		{
			return Group.Subtract(a, b);
		}

		/// <summary>
		/// Returns a group that contains the indices from 'start' including 'end'.
		/// </summary>
		/// <param name='start'>
		/// The first index.
		/// </param>
		/// <param name='end'>
		/// The last index.
		/// </param>
		public static Group Slice(int start, int end)
		{
			if (start > end) {
				// Swap to get correct order.
				var tmp = start;
				start = end;
				end = tmp;
			}
			var g = new Group(new int[]{start, end + 1});
			return g;
		}

		public static Group All(System.Collections.IList list)
		{
			return new Group(new int[]{0, list.Count});
		}

		/// <summary>
		/// Returns an enumerator iterating forward a list.
		/// </summary>
		/// <param name='list'>
		/// The list to iterate through.
		/// </param>
		/// <typeparam name='T'>
		/// The type of object to return.
		/// </typeparam>
		public IEnumerable<T> Forward<T>(IList<T> list)
		{
			int n = this.Count / 2;
			for (int i = 0; i < n; i++) {
				int start = this [i * 2];
				int end = this [i * 2 + 1];
				for (int j = start; j < end; j++) {
					yield return list [j];
				}
			}
		}

		/// <summary>
		/// Iterates through the indices in group forward.
		/// </summary>
		/// <returns>
		/// The indices.
		/// </returns>
		public IEnumerable<int> IndicesForward()
		{
			int n = this.Count / 2;
			for (int i = 0; i < n; i++) {
				int start = this [i * 2];
				int end = this [i * 2 + 1];
				for (int j = start; j < end; j++) {
					yield return j;
				}
			}
		}

		/// <summary>
		/// Returns an enumerator iterating backward a list.
		/// </summary>
		/// <param name='list'>
		/// The list to iterate through.
		/// </param>
		/// <typeparam name='T'>
		/// The type of object to return.
		/// </typeparam>
		public IEnumerable<T> Backward<T>(IList<T> list)
		{
			int n = this.Count / 2;
			for (int i = n-1; i >= 0; i--) {
				int start = this [i * 2];
				int end = this [i * 2 + 1];
				for (int j = end-1; j >= start; j--) {
					yield return list [j];
				}
			}
		}

		/// <summary>
		/// Iterates through the indices in the group backwards.
		/// </summary>
		/// <returns>
		/// The indices.
		/// </returns>
		public IEnumerable<int> IndicesBackward()
		{
			int n = this.Count / 2;
			for (int i = n-1; i >= 0; i--) {
				int start = this [i * 2];
				int end = this [i * 2 + 1];
				for (int j = end-1; j >= start; j--) {
					yield return j;
				}
			}
		}


		/// <summary>
		/// Converts from an internal index within the group to external.
		/// If it is outside the group, -1 is returned.
		/// 
		/// This method might be slow when called for each member of a group.
		/// A more efficient way is using the 'Map()' function.
		/// </summary>
		/// <param name='internalIndex'>
		/// The internal index to convert to external.
		/// </param>
		public int External(int internalIndex)
		{
			int n = this.Count / 2;
			for (int i = 0; i < n; i++) {
				int start = this[i*2];
				int end = this[i*2 + 1];
				if (internalIndex < 0) return -1;
				if (internalIndex < end - start) {
					return internalIndex + start;
				} else {
					internalIndex -= end - start;
				}
			}
			return -1;
		}

		/// <summary>
		/// Converts from an external index to an index internal to the group.
		/// If it is outside the group, -1 is returned.
		/// </summary>
		/// <param name='externalIndex'>
		/// The external index.
		/// </param>
		public int Internal(int externalIndex)
		{
			int n = this.Count / 2;
			int c = 0;
			for (int i = 0; i < n; i++) {
				int invEnd = i == 0 ? 0 : this[i*2 - 1];
				int start = this[i*2];
				int end = this[i*2 + 1];
				c += start - invEnd;

				if (externalIndex < start) return -1;
				if (externalIndex < end) return externalIndex - c;

			}
			return -1;
		}

		public bool ContainsIndex(int index)
		{
			int larger = this.BinarySearch(index);
			if (larger >= 0) {
				return (larger % 2) == 0;
			} else {
				return (~larger % 2) == 1;
			}
		}

		/// <summary>
		/// Finds the first group that contains an item.
		/// </summary>
		/// <param name='groups'>
		/// The groups to search for item.
		/// </param>
		/// <param name='item'>
		/// The item to search for.
		/// </param>
		public static int First(IList<Group> groups, int item)
		{
			int n = groups.Count;
			for (int i = 0; i < n; i++) {
				if (groups[i].ContainsIndex(item)) return i;
			}

			return -1;
		}

		/// <summary>
		/// Finds the last group that contains an item.
		/// </summary>
		/// <param name='groups'>
		/// The groups to search for item.
		/// </param>
		/// <param name='item'>
		/// The item to search for.
		/// </param>
		public static int Last(IList<Group> groups, int item)
		{
			int n = groups.Count;
			for (int i = n-1; i >= 0; i--) {
				if (groups[i].ContainsIndex(item)) return i;
			}
			
			return -1;
		}

		/// <summary>
		/// Finds the most similar group in a list of groups, searching using a filter.
		/// Uses XOR Boolean operation to find the mismatch.
		/// If two groups have equally mismatch the one with lower indices will be returned.
		/// </summary>
		/// <returns>
		/// Returns a group that is the most similar to this group.
		/// </returns>
		/// <param name='groups'>
		/// A list of groups to search for the similar.
		/// </param>
		/// <param name='filter'>
		/// A filter to control which groups to search.
		/// </param>
		public int MostSimilar(IList<Group> groups, Group filter)
		{
			var minIndex = -1;
			var minSize = int.MaxValue;
			var minGroup = null as Group;
			foreach (var i in filter.IndicesForward()) {
				var xor = (groups[i] - this) + (this - groups[i]);
				var size = Group.Size(xor);
				if (size > minSize) continue;
				if (size != minSize || groups[i].CompareTo(minGroup) < 0) {
					minIndex = i;
					minSize = size;
					minGroup = groups[i];

					if (minSize == 0) break;
				}
			}
			return minIndex;
		}

		/// <summary>
		/// Creates an array that maps internal indices of a group to external indices.
		/// This can be used for random access of members in a group.
		/// Since it only maps indices, it allows writing to the original array or list.
		/// The returned map is always ordered.
		/// </summary>
		public int[] Map()
		{
			int size = Group.Size (this);
			int[] map = new int[size];
			int n = this.Count / 2;
			int j = 0;
			for (int i = 0; i < n; i++) {
				int start = this[i*2];
				int end = this[i*2 + 1];
				for (int k = start; k < end; k++) {
					map[j++] = k;
				}
			}
			return map;
		}

		/// <summary>
		/// Creates an array of items that are member of the group.
		/// The items are specified using a list as parameter.
		/// 
		/// This method can be used when a function takes an array as arguments but doesn't allow group as filter.
		/// Notice that writing to a such array does not affect the original.
		/// </summary>
		/// <returns>
		/// Returns an array of members mapped from list.
		/// </returns>
		/// <param name='list'>
		/// The list containing members.
		/// </param>
		/// <typeparam name='T'>
		/// The type of items.
		/// </typeparam>
		public T[] MapTo<T>(IList<T> list)
		{
			int size = Group.Size (this);
			T[] map = new T[size];
			int n = this.Count / 2;
			int j = 0;
			for (int i = 0; i < n; i++) {
				int start = this[i*2];
				int end = this[i*2 + 1];
				for (int k = start; k < end; k++) {
					map[j++] = list[k];
				}
			}
			return map;
		}

		/// <summary>
		/// Creates a map within the internal space of group g.
		/// 
		/// This can be used when data is mapped by a group and you want to create
		/// groups that preserve the same relationship order to each other.
		/// 
		/// The ordered of all groups mapped with a group is preserved,
		/// but the indices changes and members outside the group is removed.
		/// </summary>
		/// <returns>
		/// Returns an array mapping indices of this group into the internal space of g.
		/// </returns>
		/// <param name='g'>
		/// The group to map with.
		/// </param>
		[PerformanceLevel(24)]
		public int[] MapWith(Group g)
		{
			var c = g * this;
			int minSize = Group.Size (c);
			int[] map = new int[minSize];
			int n = g.Count >> 1;
			int m = this.Count;
			int j = 0;
			int s = 0;
			int t = 0;
			int start, end;
			int k;
			for (int i = 0; i < n; i++) {
				start = g[i << 1];
				end = g[(i << 1) + 1];
				for (k = start; k < end; k++, t++) {
					if (s < m && this[s] <= k) s++;
					if ((s & 1) == 0) continue;

					map[j++] = t;
					if (j == minSize) return map;
				}
			}
			return map;
		}

		/// <summary>
		/// Creates a group that correspond to a map with 'g'.
		/// 
		/// The group returned has members in the same order
		/// as other groups transformed.
		/// Boolean operations among the transformed group are still valid,
		/// but contains only members that are in 'g'.
		/// 
		/// The algorithm corresponds to the following operation:
		/// return Group.FromOrderedMap(this.MapWith(a));
		/// 
		/// But uses intersection and offset to map directly without creating a map.
		/// This makes it approximately twice as fast.
		/// 
		/// </summary>
		/// <returns>
		/// Returns a group that is transformed to the internal space of 'g'.
		/// </returns>
		/// <param name='g'>
		/// The group to use as transform.
		/// </param>
		[PerformanceLevel(24)]
		public Group TransformedWith(Group a)
		{
			Group b = this;
			Group arr = new Group();
			
			int alength = a.Count;
			int blength = b.Count;
			if (alength == 0 || blength == 0)
				return arr;
			
			int i = 0, j = 0; 
			bool isA = false; 
			bool isB = false; 
			bool was = false;
			bool has = false;
			int pa, pb, min;
			int off = 0;
			int last = int.MinValue;
			while (i < alength && j < blength) {
				// Get the last value from each group.
				pa = i >= alength ? int.MaxValue : a [i];
				pb = j >= blength ? int.MaxValue : b [j];
				min = pa < pb ? pa : pb;
				
				// Advance the one with least value, both if they got the same.
				if (pa == min) {
					isA = !isA; 
					i++;

					// Add to offset to get the internal indices of 'a'.
					if (isA) off += a[i] - (i == 0 ? 0 : a[i-1]);
				}
				if (pb == min) {
					isB = !isB;
					j++;
				}
				
				// Find out if the new change should be added to the result.
				has = isA && isB;
				if (has != was) {
					// Subtract offset to get internal 'a' and check for dupcliates.
					min -= off;
					if (min == last) arr.RemoveAt(arr.Count - 1);
					else arr.Add(min);
					last = min;
				}
				
				was = has;
			}
			
			return arr;
		}
	}

}

