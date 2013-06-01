using System;
using System.Collections.Generic;
using System.ComponentModel;
using Play;

namespace Utils
{
	/// <summary>
	/// 
	/// </summary>
	public class GroupManager
	{
		private List<Controller> m_controllers;
		private List<object[]> m_items;
		private List<object[]> m_toBeAdded;
		private List<int> m_toBeRemoved;
		private List<Tuple<int, int, object>> m_toBeChanged;

		public GroupManager () {
			m_controllers = new List<Controller> ();
			m_items = new List<object[]> ();
			m_toBeAdded = new List<object[]> ();
			m_toBeRemoved = new List<int> ();
			m_toBeChanged = new List<Tuple<int, int, object>> ();
		}

		public void AddController (Controller controller) {
			controller.Manager = this;
			controller.Group = new Group ();
			m_controllers.Add (controller);
		}

		public class Controller
		{
			public Group Group;
			public GroupManager Manager;
			public Group.IsTrue<object[]> Filter;
			public MemberChangedDelegate AddMember;
			public MemberChangedDelegate RemoveMember;
			public RefreshDelegate Refresh;
			
			public delegate void MemberChangedDelegate (object[] member);
			public delegate void RefreshDelegate ();
		}

		public int Count {
			get {
				return m_items.Count;
			}
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= m_items.Count) {
				throw new IndexOutOfRangeException ();
			}

			m_toBeRemoved.Add (index);
		}

		public void Add(params object[] item)
		{
			m_toBeAdded.Add (item);
		}

		public object this [int row, int column] {
			get {
				return m_items [row][column];
			}
			set {
				m_toBeChanged.Add (new Tuple<int, int, object> (row, column, value));
			}
		}

		public void Refresh () {
			m_items.AddRange (m_toBeAdded);

			// Sort the indices to be removed in reverse order.
			m_toBeRemoved.Sort ((int x, int y) => -x.CompareTo (y));

			// Build a dictionary of removed objects.
			var removed = new Dictionary<int, object[]> ();
			int last = -1;
			foreach (var i in m_toBeRemoved) {
				if (i == last) {
					// Ignore duplicates.
					continue;
				}

				removed.Add (i, m_items [i]);
			}

			var changed = new Group ();
			foreach (var change in m_toBeChanged) {
				if (removed.ContainsKey (change.Item1)) {
					// Ignore those objects that is removed.
					continue;
				}

				m_items [change.Item1][change.Item2] = change.Item3;
				changed += change.Item1;
			}

			foreach (var i in m_toBeRemoved) {
				if (i == last) {
					// Ignore duplicates.
					continue;
				}

				last = i;
				m_items.RemoveAt (i);
			}

			foreach (var controller in m_controllers) {
				
				// Find those who belong to group that changed property.
				var insiders = Group.Filter (changed, (int index) => controller.Filter (m_items [index]));
				// Find those who are leaving the group.
				var outsiders = controller.Group * (changed - insiders);
				// Find those who are entering the group.
				insiders -= controller.Group;

				// Inform controller about members to be removed.
				var removeMembers = Group.Predicate<int> (
					(int index) => controller.Group.ContainsIndex (index),
					m_toBeRemoved);
				removeMembers += outsiders;
				if (controller.RemoveMember != null) {
					foreach (var i in removeMembers.IndicesForward ()) {
						controller.RemoveMember (removed [i]);
					}
				}

				// Inform controller about members to be added.
				var addMembers = Group.Predicate<object[]> (
					controller.Filter,
					m_toBeAdded);
				addMembers += insiders;
				if (controller.AddMember != null) {
					foreach (var i in addMembers.IndicesForward ()) {
						controller.AddMember (m_toBeAdded [i]);
					}
				}

				controller.Group += addMembers;
				controller.Group -= removeMembers;
				if (controller.Refresh != null) {
					controller.Refresh ();
				}
			}

			m_toBeRemoved.Clear ();
			m_toBeAdded.Clear ();
			m_toBeChanged.Clear ();
		}
	}
}

