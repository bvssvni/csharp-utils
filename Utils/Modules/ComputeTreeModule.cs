using System;
using System.Collections.Generic;
using Play;

namespace Utils
{
	public class ComputeTreeModule
	{
		// This is the kind of function that gets called
		// when an object is marked dirty.
		public delegate void ComputeDelegate (PropertyNode[] items, int start, int end);

		// Represents a property.
		public struct PropertyNode : IComparable<PropertyNode>
		{
			public string Name;
			public object Value;
			public int Input;
			public bool Dirty;

			public PropertyNode (string name, object value) {
				this.Name = name;
				this.Value = value;
				this.Input = -1;
				this.Dirty = false;
			}

			int IComparable<PropertyNode>.CompareTo(PropertyNode other)
			{
				return Name.CompareTo (other.Name);
			}
		}

		// Represents an object.
		public struct ObjectNode : IComparable<ObjectNode>, IDisposable
		{
			public string Name;
			public Cheap<PropertyNode> Properties;
			public Cheap<ObjectNode> SubNodes;
			public ComputeDelegate Compute;

			public ObjectNode (string name, ComputeDelegate compute, params PropertyNode[] properties) {
				this.Name = name;
				this.Properties = Cheap<PropertyNode>.FromArray (properties);
				this.SubNodes = null;
				this.Compute = compute;
				this.disposed = false;
			}

			private bool disposed;
			public void Dispose()
			{
				if (disposed) return;

				Properties.Dispose ();
				if (SubNodes != null) {
					SubNodes.Dispose ();
				}

				GC.SuppressFinalize(this);
			}

			int IComparable<ObjectNode>.CompareTo(ObjectNode other)
			{
				return Name.CompareTo (other.Name);
			}

			public T GetProperty<T> (string name) {
				Cheap<PropertyNode>.Semaphore++;
				int start = 0;
				int end = 0;
				T res = default (T);
				if (Properties.GetRange (ref start, ref end)) {
					for (int i = start; i < end; i++) {
						var prop = Cheap<PropertyNode>.Items [i];
						if (prop.Name == name) {
							res = (T)Convert.ChangeType (prop.Value, typeof (T));
						}
					}
				}

				Cheap<PropertyNode>.Semaphore--;
				return res;
			}

			public void Update () {
				Cheap<PropertyNode>.Semaphore++;
				int start = 0;
				int end = 0;
				if (this.Properties.GetRange (ref start, ref end)) {
					var items = Cheap<PropertyNode>.Items;
					for (int i = start; i < end; i++) {
						int input = items [i].Input;
						if (input == -1) {
							continue;
						}

						items [i].Value = items [input].Value;
					}

					this.Compute (items, start, end);
				}
				Cheap<PropertyNode>.Semaphore--;
			}

			// Gets the properties of sub branch, including object.
			// An object can appear multiple times in a branch.
			// This algorithm checks if that object is already contained in the group.
			// It is assumed that no objects overlap.
			public Group SubBranchProperties (Group gr = null) {
				gr += Cheap<PropertyNode>.UnionGroup (this.Properties);
				if (SubNodes == null) {
					return gr;
				}

				SubNodes.ForEach ((ref ObjectNode child) => {
					int start = 0;
					int end = 0;
					if (child.Properties.GetRange (ref start, ref end)) {
						gr = child.SubBranchProperties (gr);
					}
				});

				return gr;
			}
		}

		public ComputeTreeModule()
		{
		}

		// Gets the input properties of a group.
		// This are all properties that this group depend on.
		public static Group InputProperties (Group properties) {
			var gr = new Group (Group.Size (properties));
			properties.ForEach ((int i) => {
				var input = Cheap<PropertyNode>.Items [i].Input;
				if (input == -1) {
					return;
				}

				gr += input;
			});

			return gr;
		}

		// Gets the properties that rely on properties within the group.
		// These properties needs to redirect the 'input' value to copied group.
		public static Group InternalDependencies (Group properties) {
			var gr = properties;
			properties.ForEach ((int i) => {
				var input = Cheap<PropertyNode>.Items [i].Input;
				if (input == -1) {
					return;
				}

				gr -= input;
			});

			return gr;
		}

		public static void MarkDirty (Group properties) {
			properties.ForEach ((int i) => Cheap<PropertyNode>.Items [i].Dirty = true);
		}

		public static bool Connect (ObjectNode fromObj, string fromProp,
		                            ObjectNode toObj, string toProp) {
			Cheap<PropertyNode>.Semaphore++;
			int fromStart = 0;
			int fromEnd = 0;
			if (fromObj.Properties.GetRange (ref fromStart, ref fromEnd)) {
				int fromIndex = -1;
				for (int i = fromStart; i < fromEnd; i++) {
					if (Cheap<PropertyNode>.Items [i].Name == fromProp) {
						fromIndex = i;
						break;
					}
				}
				if (fromIndex == -1) {
					Cheap<PropertyNode>.Semaphore--;
					return false;
				}

				int toStart = 0;
				int toEnd = 0;
				if (toObj.Properties.GetRange (ref toStart, ref toEnd)) {
					for (int i = toStart; i < toEnd; i++) {
						if (Cheap<PropertyNode>.Items [i].Name == toProp) {
							Cheap<PropertyNode>.Items [fromIndex].Input = i;
							Cheap<PropertyNode>.Semaphore--;
							return true;
						}
					}
				}
			}

			Cheap<PropertyNode>.Semaphore--;
			return false;
		}
	}
}

