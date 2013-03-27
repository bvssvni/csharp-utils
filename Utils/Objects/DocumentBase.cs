using System;
using System.Collections.Generic;
using Play;

namespace Utils
{
	/// <summary>
	/// 
	/// A generic object for storing document data, using group oriented approach.
	/// In normal datasets, one uses a unique identifier to connect one object to another.
	/// Instead, an object is associated with others by using a Group object.
	/// The programmer never has to deal with unique identifiers directly.
	/// Because all objects are stored in same dictionary, a group can contain different types of objects.
	/// Groups are not updated when objects are removed from the dictionary.
	/// Always check for null values before using an object.
	/// You can use the "List" function to obtain an array with the objects of a group.
	/// 
	/// Examples of Group fields as part of objects:
	/// 	Kid.Parents
	/// 	Car.Wheels
	/// 	Owner.Properties
	/// 	Customer.Cases
	/// 
	/// </summary>
	public class DocumentBase : Dictionary<int, object>
	{
		public int LastId = 0;

		public DocumentBase ()
		{
		}

		/// <summary>
		/// Returns a new id and increments the last id.
		/// </summary>
		/// <returns>
		/// The new id.
		/// </returns>
		public int NewId ()
		{
			return LastId++;
		}

		/// <summary>
		/// Finds out whether a group is synchronized.
		/// A group that contains deleted objects is not synchronized.
		/// </summary>
		/// <returns>
		/// <c>true</c> if group is synchronized; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='g'>
		/// The group to check for synchronization.
		/// </param>
		public bool IsSynced (Group g)
		{
			foreach (var id in g.IndicesForward ())
			{
				if (!this.ContainsKey (id)) return false;
			}
			
			return true;
		}

		/// <summary>
		/// Creates a new group that does not contain deleted objects.
		/// </summary>
		/// <param name='g'>
		/// The group to create a synchronized group from.
		/// </param>
		public Group Sync (Group g)
		{
			return Group.Filter (g, delegate (int index) {
				return this.ContainsKey (index);
			});
		}

		/// <summary>
		/// Adds object by incrementing last id.
		/// </summary>
		/// <returns>
		/// The id of the new object.
		/// </returns>
		/// <param name='obj'>
		/// The object to add to dictionary.
		/// </param>
		/// <typeparam name='T'>
		/// The type of object.
		/// </typeparam>
		public int AddObject (object obj)
		{
			int id = NewId ();
			this.Add (id, obj);
			return id;
		}

		/// <summary>
		/// Creates an array filled with objects of a group.
		/// </summary>
		/// <param name='g'>
		/// The group to create array from.
		/// </param>
		/// <typeparam name='T'>
		/// The type of items in the array.
		/// Items of a different kind is not added to the array.
		/// </typeparam>
		public T[] List<T> (Group g) where T : class
		{
			if (g == null) return new T[]{};
			
			var list = new List<T> ();
			foreach (var id in g.IndicesForward ())
			{
				if (!this.ContainsKey (id)) {continue;}
				
				var obj = this [id] as T;
				if (obj == null) {continue;}
				
				list.Add (obj);
			}
			
			return list.ToArray();
		}
	}
}

