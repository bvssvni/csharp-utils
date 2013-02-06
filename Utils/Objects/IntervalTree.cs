using System;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// Used to analyse text with nested structures, such as brackets '[]' or curly braces '{}'.
	/// The IntervalTree is similar to a group, that it contains intervals in the context of a list or array.
	/// </summary>
	public class IntervalTree
	{
		public int Start;
		public int End;
		public List<IntervalTree> Children;

		public IntervalTree (int start, int end) {
			this.Start = start;
			this.End = end;
			this.Children = new List<IntervalTree>();
		}

		/// <summary>
		/// Adds interval to tree.
		/// If the interval intersects with the structure, it is not added.
		/// </summary>
		/// <param name="start">Start of interval.</param>
		/// <param name="end">End of interval.</param>
		public bool Add (int start, int end)
		{
			if (start < this.Start || end > this.End) return false;

			// If no children, add to this node.
			if (this.Children.Count == 0) {
				this.Children.Add (new IntervalTree (start, end));
				return true;
			}

			// Try to add to children.
			int n = this.Children.Count;
			for (int i = 0; i < n; i++) {
				var child = this.Children[i];
				bool partOutside = start < child.Start || end > child.End;
				if (start < child.End && end > child.Start && partOutside) return false;
				if (partOutside) continue;

				return child.Add (start, end);
			}

			// Insert new node among children.
			for (int i = 0; i < n; i++) {
				var child = this.Children[i];
				if (child.Start < start) continue;

				this.Children.Insert(i, new IntervalTree (start, end));
				return true;
			}

			this.Children.Add (new IntervalTree (start, end));
			return true;
		}

		/// <summary>
		/// Creates an array of trees from start and end brackets.
		/// Uses similar algorithm to Boolean operations on groups.
		/// Searches for minimum value and advances the one least one.
		/// </summary>
		/// <returns>Returns an array of trees.</returns>
		/// <param name="start">Start brackets.</param>
		/// <param name="end">End brackets.</param>
		public static IntervalTree[] Nested (int[] start, int[] end)
		{
			var list = new List<IntervalTree>();
			var st = new Stack<int>();

			// Use a queue to get the child nodes in correct order.
			var childStack = new Queue<IntervalTree>();
			int i = 0;
			int j = 0;
			int a, b;
			while (i < start.Length || j < end.Length) {
				a = i < start.Length ? start[i] : int.MaxValue;
				b = j < end.Length ? end[j] : int.MaxValue;
				int min = a < b ? a : b;

				if (min == a) {
					st.Push (a);
					i++;
				}

				if (min == b) {
					var node = new IntervalTree (st.Pop(), b);
					j++;
					if (st.Count > 0) {
						childStack.Enqueue (node);
						continue;
					}

					while (childStack.Count > 0 && childStack.Peek ().Start > node.Start) {
						node.Children.Add (childStack.Dequeue ());
					}

					list.Add (node);
				}
			}

			return list.ToArray ();
		}
	}
}

