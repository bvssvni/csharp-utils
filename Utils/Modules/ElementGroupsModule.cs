using System;
using System.Collections.Generic;
using Play;

namespace Utils
{
	public class ElementGroupsModule
	{
		private static int CompareBoolArrays(bool[] a, bool[] b) {
			int n = a.Length;
			int res = 0;
			for (int i = 0; i < n; i++) {
				res = a[i] == b[i] ? 0 : a[i] && !b[i] ? -1 : 1;
				if (res != 0) return res;
			}
			
			return res;
		}

		private static int CompareItems(Item aItem, Item bItem) {
			var a = aItem.Groups;
			var b = bItem.Groups;
			int n = a.Length;
			int res = 0;
			for (int i = 0; i < n; i++) {
				res = a[i] == b[i] ? 0 : !a[i] && b[i] ? 1 : -1;
				if (res != 0) return res;
			}
			
			return aItem.Index.CompareTo(bItem.Index);
		}

		private struct Item
		{
			public int Index;
			public bool[] Groups;
		}

		// This is used to access lists from dictionary of lists.
		private class EqualityComparer : IEqualityComparer<bool[]>
		{
			public bool Equals(bool[] a, bool[] b)
			{
				int n = a.Length;
				int res = 0;
				for (int i = 0; i < n; i++) {
					res = a[i] == b[i] ? 0 : a[i] && !b[i] ? -1 : 1;
					if (res != 0) return false;
				}
				
				return true;
			}
			
			public int GetHashCode(bool[] x)
			{
				int result = 29;
				foreach (bool b in x)
				{
					if (b) { result++; }

					result *= 23;
				}
				return result;
			}
		}

		public static Dictionary<bool[], Group> ElementGroups(IList<Group> groups) {
			// Find the group that contains every member.
			var g = groups[0];
			for (int i = 1; i < groups.Count; i++) {
				g += groups[i];
			}

			// For each member,
			// find all groups that it belongs to.
			var arr = new Item[Group.Size(g)];
			foreach (int i in g.IndicesForward()) {
				var id = new bool[groups.Count];
				for (int j = 0; j < groups.Count; j++) {
					id[j] = groups[j].ContainsIndex(i);
				}

				arr[i].Index = i;
				arr[i].Groups = id;
			}

			// Sort the members by which groups it belongs.
			Array.Sort<Item>(arr, CompareItems);

			var dict = new Dictionary<bool[], Group>(new EqualityComparer());
			bool[] currentId = arr[0].Groups;
			var currentList = new Group();
			currentList.AddIndex(arr[0].Index);
			dict.Add(currentId, currentList);

			for (int i = 1; i < arr.Length; i++) {
				var id = arr[i].Groups;

				int res = CompareBoolArrays(id, currentId);
				if (res != 0) {
					// Create a new list.
					currentList = new Group();
					dict.Add(id, currentList);
					currentId = id;
				}

				currentList.AddIndex(arr[i].Index);
			}

			return dict;
		}
		
	}
}

