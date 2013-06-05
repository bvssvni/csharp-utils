/*
GroupManager - Transactional update of groups using predicates.  
BSD license.  
by Sven Nilsen, 2013
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
using System.ComponentModel;
using Play;

namespace Utils
{
	/// <summary>
	/// GroupManager is a transactional table data type with groups.
	/// Unlike list data types, it does not update the data before you call 'Commit'.
	/// If you want to cancel the changes, you can call 'Rollback'.
	/// 
	/// Groups are accessed through 'Controller' objects.
	/// The controller has also a filter function that determine which rows belongs to the group.
	/// The filter function is called by need of GroupManager.
	/// 
	/// A controller also contains 3 delegates:
	/// 	
	/// 	AddMember - Is called when a new row is added to group.
	/// 	RemoveMemeber - Is called when a row is removed from a group.
	/// 	Refresh - Is called after changes to group is done.
	///
	/// Updates will trigger call to filter function even values are not different.
	/// This decision is based on the assumption that checking for equality is about
	/// equal expensive as checking the filter.
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
				var r = m_items [row];
				if (column >= r.Length) {
					return null;
				}

				return r [column];
			}
			set {
				var r = m_items [row];
				if (column >= r.Length) {
					return;
				}

				m_toBeChanged.Add (new Tuple<int, int, object> (row, column, value));
			}
		}

		public void Rollback () {
			m_toBeRemoved.Clear ();
			m_toBeAdded.Clear ();
			m_toBeChanged.Clear ();
		}

		public void Commit () {
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
				if (controller.Refresh != null && addMembers.Count + removeMembers.Count > 0) {
					controller.Refresh ();
				}
			}

			m_toBeRemoved.Clear ();
			m_toBeAdded.Clear ();
			m_toBeChanged.Clear ();
		}
	}
}

