/*

Utils.Persistency - Classes that remember previous states.
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

namespace Utils.Persistency
{
	/// <summary>
	/// Persistent object.
	/// 
	/// The 'Store' function stores the current state.
	/// The 'Restore' function restores the previous state.
	/// 
	/// It is important that 'Store' and 'Restore' is called equal number of times.
	/// </summary>
	public abstract class PersistentBase<T> where T : PersistentBase<T>
	{
		private T m_previous;
		private int m_noChange = 0;
		
		public abstract T Copy (T to = default (T));
		public abstract bool HasChanged (T obj);
		
		public abstract void StoreMembers ();
		public abstract void RestoreMembers ();
		
		/// <summary>
		/// Returns the previous state, null if unchanged.
		/// </summary>
		public void Store () {
			StoreMembers ();
			
			if (HasChanged (m_previous)) {
				var copy = Copy ();
				copy.m_previous = m_previous;
				copy.m_noChange = m_noChange;
				m_previous = copy;
			} else {
				m_noChange++;
			}
		}
		
		public void Restore () {
			if (m_noChange > 0) {
				m_noChange--;
			} else {
				if (m_previous == null) {
					throw new Exception ("Restoring persistent object failed:\r\nNo previous state.\r\nDid you forgot a call to 'Store'?");
				}
				
				m_previous.Copy ((T)this);
				this.m_noChange = m_previous.m_noChange;
				this.m_previous = m_previous.m_previous;
			}
			
			RestoreMembers ();
		}
	}

	/// <summary>
	/// Persistent value.
	/// 
	/// Used internally in PersistentList and PersistentDictionary.
	/// </summary>
	internal class PersistentValue<T> : PersistentBase<PersistentValue<T>>
	{
		public T Value;
		
		public PersistentValue(T value)
		{
			this.Value = value;
		}
		
		public override PersistentValue<T> Copy(PersistentValue<T> to = null)
		{
			if (to == null) {
				to = new PersistentValue<T> (Value);
				return to;
			}
			
			to.Value = Value;
			return to;
		}
		
		public override bool HasChanged(PersistentValue<T> obj)
		{
			return obj == null || !this.Value.Equals (obj.Value);
		}
		
		public override void StoreMembers()
		{
			
		}
		
		public override void RestoreMembers()
		{
			
		}
	}

	/// <summary>
	/// List of persistent objects.
	/// </summary>
	public class PersistentList<T> : PersistentBase<PersistentList<T>>, IList<T>
	{
		private List<PersistentValue<T>> m_list;
		
		public override PersistentList<T> Copy(PersistentList<T> to)
		{
			if (to == null) {
				to = new PersistentList<T> (m_list.Count);
			}
			
			to.m_list.Clear ();
			to.m_list.AddRange (m_list);
			return to;
		}
		
		public override bool HasChanged (PersistentList<T> obj) {
			return obj == null || m_list.Count != obj.Count;
		}
		
		public override void StoreMembers()
		{
			foreach (var item in m_list) {
				item.Store ();
			}
		}
		
		public override void RestoreMembers()
		{
			foreach (var item in m_list) {
				item.Restore ();
			}
		}
		
		public PersistentList () {
			m_list = new List<PersistentValue<T>> ();
		}
		
		public PersistentList (int capacity) {
			m_list = new List<PersistentValue<T>> (capacity);
		}
		
		public int Count {
			get {
				return m_list.Count;
			}
		}
		
		public bool IsReadOnly {
			get {
				return false;
			}
		}
		
		#region IList implementation
		public int IndexOf(T item)
		{
			int n = this.Count;
			for (var i = 0; i < n; i++) {
				if (m_list [i].Value.Equals (item)) {
					return i;
				}
			}
			
			return -1;
		}
		public void Insert(int index, T item)
		{
			m_list.Insert (index, new PersistentValue<T> (item));
		}
		public void RemoveAt(int index)
		{
			m_list.RemoveAt (index);
		}
		public T this[int index] {
			get {
				return m_list [index].Value;
			}
			set {
				m_list [index].Value = value;
			}
		}
#endregion
		
		#region ICollection implementation
		public void Add(T item)
		{
			m_list.Add (new PersistentValue<T> (item));
		}
		public void Clear()
		{
			m_list.Clear ();
		}
		public bool Contains(T item)
		{
			return IndexOf (item) != -1;
		}
		public void CopyTo(T[] array, int arrayIndex)
		{
			int n = this.Count;
			for (int i = 0; i < n; i++) {
				array [arrayIndex + i] = m_list [i].Value;
			}
		}
		public bool Remove(T item)
		{
			int index = IndexOf (item);
			if (index == -1) {return false;}

			m_list.RemoveAt (index);
			return true;
		}
#endregion
		#region IEnumerable implementation
		public IEnumerator<T> GetEnumerator()
		{
			foreach (var item in m_list) {
				yield return item.Value;
			}
		}
#endregion
		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			foreach (var item in m_list) {
				yield return item.Value;
			}
		}
#endregion
	}

	/// <summary>
	/// Persistent dictionary.
	/// </summary>
	public class PersistentDictionary<TKey, TValue>
		: PersistentBase<PersistentDictionary<TKey, TValue>>, 
		IDictionary<TKey, TValue>
	{
		private Dictionary<TKey, PersistentValue<TValue>> m_dict;
		
		public override PersistentDictionary<TKey, TValue> Copy(PersistentDictionary<TKey, TValue> to = null)
		{
			if (to == null) {
				to = new PersistentDictionary<TKey, TValue> (m_dict.Count);
			}
			
			to.m_dict.Clear ();
			foreach (var pair in m_dict) {
				to.m_dict.Add (pair.Key, pair.Value);
			}
			
			return to;
		}

		private static bool AreDifferentKeys(Dictionary<TKey, PersistentValue<TValue>> a,
		                                 Dictionary<TKey, PersistentValue<TValue>> b) {
			foreach (var pair in a) {
				if (b.ContainsKey (pair.Key)) {
					return true;
				}
			}

			return false;
		}

		public override bool HasChanged(PersistentDictionary<TKey, TValue> obj)
		{
			return obj == null 
				|| m_dict.Count != obj.m_dict.Count 
				|| AreDifferentKeys (this.m_dict, obj.m_dict);
		}
		
		public override void RestoreMembers()
		{
			foreach (var val in m_dict.Values) {
				val.Restore ();
			}
		}
		
		public override void StoreMembers()
		{
			foreach (var val in m_dict.Values) {
				val.Store ();
			}
		}
		
		public PersistentDictionary () {
			m_dict = new Dictionary<TKey, PersistentValue<TValue>> ();
		}
		
		public PersistentDictionary (int capacity) {
			m_dict = new Dictionary<TKey, PersistentValue<TValue>> (capacity);
		}
		
		public int Count {
			get {
				return m_dict.Count;
			}
		}
		
		public bool IsReadOnly {
			get {
				return false;
			}
		}
		
		#region IDictionary implementation		
		public void Add(TKey key, TValue value)
		{
			m_dict.Add (key, new PersistentValue<TValue> (value));
		}		
		public bool ContainsKey(TKey key)
		{
			return m_dict.ContainsKey (key);
		}		
		public bool Remove(TKey key)
		{
			return m_dict.Remove (key);
		}		
		public bool TryGetValue(TKey key, out TValue value)
		{
			PersistentValue<TValue> val;
			var res = m_dict.TryGetValue (key, out val);
			value = val.Value;
			return res;
		}		
		public TValue this[TKey key] {
			get {
				return m_dict [key].Value;
			}
			set {
				m_dict [key].Value = value;
			}
		}		
		ICollection<TKey> IDictionary<TKey, TValue>.Keys {
			get {
				return m_dict.Keys;
			}
		}		
		ICollection<TValue> IDictionary<TKey, TValue>.Values {
			get {
				throw new NotImplementedException ();
			}
		}		
		#endregion		
		#region ICollection implementation		
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			m_dict.Add (item.Key, new PersistentValue<TValue> (item.Value));
		}
		public void Clear()
		{
			m_dict.Clear ();
		}		
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return m_dict.ContainsKey (item.Key);
		}		
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			int i = arrayIndex;
			foreach (var pair in m_dict) {
				array [i++] = new KeyValuePair<TKey, TValue> (pair.Key, pair.Value.Value);
			}
		}		
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			return m_dict.Remove (item.Key);
		}		
		#endregion		
		#region IEnumerable implementation		
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			foreach (var pair in m_dict) {
				yield return new KeyValuePair<TKey, TValue> (pair.Key, pair.Value.Value);
			}
		}		
		#endregion		
		#region IEnumerable implementation		
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			foreach (var pair in m_dict) {
				yield return new KeyValuePair<TKey, TValue> (pair.Key, pair.Value.Value);
			}
		}		
#endregion
	}
}

