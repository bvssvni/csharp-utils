/*

Utils.Persistency - Classes that remember previous states.
BSD license.
by Sven Nilsen, 2013
http://www.cutoutpro.com
Version: 0.001 in angular degrees version notation
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html

0.001 - Added 'ILoaded' interface.

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
	public interface IPersistent
	{
		void Store();
		void Undo();
		void Redo();
	}

	/// <summary>
	/// Inherit from this interface if you want to save unused memory.
	/// The 'Persistent<T>' sets the flag 'Loaded' to false when
	/// the value is no longer current.
	/// The 'Loaded' flag is set to true when the value is brought back.
	/// </summary>
	public interface ILoaded
	{
		bool Loaded {get; set;}
	}

	public class Persistent<T> : IPersistent
	{
		private Stack<T> m_previous;
		private Stack<int> m_previousSteps;
		private Stack<T> m_next;
		private Stack<int> m_nextSteps;

		public T Value;

		public Persistent(T val)
		{
			m_previous = new Stack<T>();
			m_previousSteps = new Stack<int>();
			m_next = new Stack<T>();
			m_nextSteps = new Stack<int>();
			this.Value = val;
		}

		public void Store()
		{
			if (this.Value is IPersistent)
			{
				// Store sub structure.
				var persistentValue = (IPersistent)this.Value;
				persistentValue.Store();
			}

			m_next.Clear();
			m_nextSteps.Clear();
			bool previousIsZero = m_previous.Count == 0;
			bool valueEqualsPrevious = previousIsZero ? false : this.Value.Equals(m_previous.Peek());
			if (!valueEqualsPrevious)
			{
				if (!previousIsZero && m_previous.Peek() is ILoaded)
				{
					// Put objects to sleep that are no longer used.
					((ILoaded)m_previous.Peek()).Loaded = false;
				}

				m_previous.Push(this.Value);
				m_previousSteps.Push(1);
			}
			else
			{
				// The value equals the previous value.
				// Increase counter.
				m_previousSteps.Push(m_previousSteps.Pop() + 1);
			}
		}

		private void UndoRedo(Stack<T> previous,
		                      Stack<int> previousSteps,
		                      Stack<T> next,
		                      Stack<int> nextSteps)
		{
			bool nextIsZero = next.Count == 0;
			bool valueEqualsNext = nextIsZero ? false : this.Value.Equals(next.Peek());
			if (!valueEqualsNext)
			{
				next.Push(this.Value);
				nextSteps.Push(1);
			}
			else
			{
				nextSteps.Push(nextSteps.Pop() + 1);
			}
			
			if (previousSteps.Peek() > 1)
			{
				// Decrease counter.
				previousSteps.Push(previousSteps.Pop() - 1);
				this.Value = previous.Peek();
			}
			else
			{
				// Get previous value.
				this.Value = previous.Pop();
				previousSteps.Pop();
			}
		}

		public void Undo()
		{
			var old = this.Value;

			UndoRedo(m_previous, m_previousSteps, m_next, m_nextSteps);

			if (this.Value is IPersistent)
			{
				var persistentValue = (IPersistent)this.Value;
				persistentValue.Undo();
			}

			if (!this.Value.Equals(old))
			{
				if (old is ILoaded) {((ILoaded)old).Loaded = false;}
				if (this.Value is ILoaded) {((ILoaded)this.Value).Loaded = true;}
			}
		}

		public void Redo()
		{
			var old = this.Value;

			if (this.Value is IPersistent)
			{
				var persistentValue = (IPersistent)this.Value;
				persistentValue.Redo();
			}

			UndoRedo(m_next, m_nextSteps, m_previous, m_previousSteps);

			if (!this.Value.Equals(old))
			{
				if (old is ILoaded) {((ILoaded)old).Loaded = false;}
				if (this.Value is ILoaded) {((ILoaded)this.Value).Loaded = true;}
			}
		}
	}

	/// <summary>
	/// List of persistent objects.
	/// </summary>
	public class PersistentList<T> : IPersistent, IList<T>
	{
		private Stack<List<Persistent<T>>> m_previous;
		private Stack<int> m_previousSteps;
		private Stack<List<Persistent<T>>> m_next;
		private Stack<int> m_nextSteps;
		private List<Persistent<T>> m_list;

		private static List<Persistent<T>> CopyList(List<Persistent<T>> list)
		{
			var newList = new List<Persistent<T>>(list.Count);
			newList.AddRange(list);
			return newList;
		}

		public void Store()
		{
			int n = m_list.Count;
			for (int i = 0; i < n; i++) 
			{
				var persistentValue = m_list[i] as IPersistent;
				if (persistentValue != null)
				{
					persistentValue.Store();
				}
			}

			m_next.Clear();
			m_nextSteps.Clear();
			if (m_previous.Count == 0 || m_list.Count != m_previous.Peek().Count)
			{
				m_previous.Push(CopyList(m_list));
				m_previousSteps.Push(1);
			}
			else
			{
				// The value equals the previous value.
				// Increase counter.
				m_previousSteps.Push(m_previousSteps.Pop() + 1);
			}
		}

		private void UndoRedo(Stack<List<Persistent<T>>> previous,
		                      Stack<int> previousSteps,
		                      Stack<List<Persistent<T>>> next,
		                      Stack<int> nextSteps)
		{
			if (next.Count == 0 || this.m_list.Count != next.Peek().Count)
			{
				next.Push(CopyList(m_list));
				nextSteps.Push(1);
			}
			else
			{
				nextSteps.Push(nextSteps.Pop() + 1);
			}
			
			if (previousSteps.Peek() > 1)
			{
				// Decrease counter.
				previousSteps.Push(previousSteps.Pop() - 1);
				m_list = previous.Peek();
			}
			else
			{
				// Get previous value.
				m_list = previous.Pop();
				previousSteps.Pop();
			}
		}

		public void Undo()
		{
			UndoRedo(m_previous, m_previousSteps, m_next, m_nextSteps);

			int n = m_list.Count;
			for (int i = 0; i < n; i++) 
			{
				var persistentValue = m_list[i] as IPersistent;
				if (persistentValue != null)
				{
					persistentValue.Undo();
				}
			}

		}

		public void Redo()
		{
			int n = m_list.Count;
			for (int i = 0; i < n; i++) 
			{
				var persistentValue = m_list[i] as IPersistent;
				if (persistentValue != null)
				{
					persistentValue.Redo();
				}
			}

			UndoRedo(m_next, m_nextSteps, m_previous, m_previousSteps);

		}

		public PersistentList () {
			m_previous = new Stack<List<Persistent<T>>>();
			m_previousSteps = new Stack<int>();
			m_next = new Stack<List<Persistent<T>>>();
			m_nextSteps = new Stack<int>();
			m_list = new List<Persistent<T>> ();
		}
		
		public PersistentList (int capacity) {
			m_previous = new Stack<List<Persistent<T>>>();
			m_previousSteps = new Stack<int>();
			m_next = new Stack<List<Persistent<T>>>();
			m_nextSteps = new Stack<int>();
			m_list = new List<Persistent<T>> (capacity);
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
			m_list.Insert (index, new Persistent<T> (item));
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
			m_list.Add (new Persistent<T> (item));
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
		: IPersistent, 
		IDictionary<TKey, TValue>
	{
		private Dictionary<TKey, Persistent<TValue>> m_dict;

		private Stack<Dictionary<TKey, Persistent<TValue>>> m_previous;
		private Stack<int> m_previousSteps;
		private Stack<Dictionary<TKey, Persistent<TValue>>> m_next;
		private Stack<int> m_nextSteps;

		private static Dictionary<TKey, Persistent<TValue>> CopyDictionary(Dictionary<TKey, Persistent<TValue>> dict)
		{
			return new Dictionary<TKey, Persistent<TValue>>(dict);
		}

		public void Store()
		{
			foreach (var pair in m_dict)
			{
				var persistentValue = pair.Value as IPersistent;
				if (persistentValue != null)
				{
					persistentValue.Store();
				}
			}

			m_next.Clear();
			m_nextSteps.Clear();
			if (m_previous.Count == 0 || 
			    m_dict.Count != m_previous.Peek().Count ||
			    AreDifferentKeys(m_dict, m_previous.Peek()))
			{
				m_previous.Push(CopyDictionary(m_dict));
				m_previousSteps.Push(1);
			}
			else
			{
				// The value equals the previous value.
				// Increase counter.
				m_previousSteps.Push(m_previousSteps.Pop() + 1);
			}
		}
		
		private void UndoRedo(Stack<Dictionary<TKey, Persistent<TValue>>> previous,
		                      Stack<int> previousSteps,
		                      Stack<Dictionary<TKey, Persistent<TValue>>> next,
		                      Stack<int> nextSteps)
		{
			if (next.Count == 0 || 
			    this.m_dict.Count != next.Peek().Count ||
			    AreDifferentKeys(m_dict, next.Peek()))
			{
				next.Push(CopyDictionary(m_dict));
				nextSteps.Push(1);
			}
			else
			{
				nextSteps.Push(nextSteps.Pop() + 1);
			}
			
			if (previousSteps.Peek() > 1)
			{
				// Decrease counter.
				previousSteps.Push(previousSteps.Pop() - 1);
				m_dict = previous.Peek();
			}
			else
			{
				// Get previous value.
				m_dict = previous.Pop();
				previousSteps.Pop();
			}
		}

		public void Undo()
		{
			UndoRedo(m_previous, m_previousSteps, m_next, m_nextSteps);

			foreach (var pair in m_dict)
			{
				var persistentValue = pair.Value as IPersistent;
				if (persistentValue != null)
				{
					persistentValue.Undo();
				}
			}
		}

		public void Redo()
		{
			foreach (var pair in m_dict)
			{
				var persistentValue = pair.Value as IPersistent;
				if (persistentValue != null)
				{
					persistentValue.Redo();
				}
			}

			UndoRedo(m_next, m_nextSteps, m_previous, m_previousSteps);
		}

		protected T GetValue<T>(TKey key, T defaultValue) where T : TValue
		{
			if (ContainsKey(key))
			{
				return (T)this[key];
			}
			else
			{
				return defaultValue;
			}
		}
		
		protected void SetValue<T>(TKey key, T defaultValue, T val) where T : TValue
		{
			if (val == null || val.Equals(defaultValue))
			{
				Remove(key);
				return;
			}
			
			if (ContainsKey(key))
			{
				this[key] = val;
			}
			else 
			{
				Add(key, val);
			}
		}
		
		private static bool AreDifferentKeys(Dictionary<TKey, Persistent<TValue>> a,
		                                     Dictionary<TKey, Persistent<TValue>> b) {
			foreach (var pair in a) {
				if (b.ContainsKey (pair.Key)) {
					return true;
				}
			}
			
			return false;
		}
		
		public PersistentDictionary () {
			m_previous = new Stack<Dictionary<TKey, Persistent<TValue>>>();
			m_previousSteps = new Stack<int>();
			m_next = new Stack<Dictionary<TKey, Persistent<TValue>>>();
			m_nextSteps = new Stack<int>();
			m_dict = new Dictionary<TKey, Persistent<TValue>> ();
		}
		
		public PersistentDictionary (int capacity) {
			m_previous = new Stack<Dictionary<TKey, Persistent<TValue>>>();
			m_previousSteps = new Stack<int>();
			m_next = new Stack<Dictionary<TKey, Persistent<TValue>>>();
			m_nextSteps = new Stack<int>();
			m_dict = new Dictionary<TKey, Persistent<TValue>> (capacity);
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
			m_dict.Add (key, new Persistent<TValue> (value));
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
			Persistent<TValue> val;
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
			m_dict.Add (item.Key, new Persistent<TValue> (item.Value));
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

	/// <summary>
	/// Undo redo.
	/// 
	/// Supports undo and redo on a persistent object.
	/// Before each action, call 'NewAction' with a description.
	/// </summary>
	public class UndoRedo<T> where T : IPersistent
	{
		private T m_data;
		private int m_cursor = 0;
		private List<string> m_descriptions;
		
		public bool CanUndo {
			get {
				return m_cursor > 0;
			}
		}
		
		public bool CanRedo {
			get {
				return m_cursor < m_descriptions.Count;
			}
		}
		
		public int Cursor {
			get {
				return m_cursor;
			}
		}
		
		public int Count {
			get {
				return m_descriptions.Count;
			}
		}
		
		public List<string> Descriptions {
			get {
				return m_descriptions;
			}
		}
		
		public string PreviousDescription {
			get {
				if (m_cursor == 0) {
					return null;
				}
				
				return m_descriptions [m_cursor - 1];
			}
		}
		
		public UndoRedo(T data)
		{
			this.m_data = data;
			this.m_descriptions = new List<string> ();
		}
		
		public void NewAction (string description) {
			if (m_cursor < m_descriptions.Count) {
				m_descriptions.RemoveRange (m_cursor, m_descriptions.Count - m_cursor);
			}
			
			m_descriptions.Add (description);
			m_cursor++;
			m_data.Store ();
		}
		
		public void Undo () {
			m_cursor--;
			m_data.Undo();
		}
		
		public void Redo () {
			m_cursor++;
			m_data.Redo();
		}
	}
}

