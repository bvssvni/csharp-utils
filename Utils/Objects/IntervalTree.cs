using System;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// Used to analyse text with nested structures, such as brackets '[]' or curly braces '{}'.
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
		/// Creates an array of trees from start and end brackets.
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

