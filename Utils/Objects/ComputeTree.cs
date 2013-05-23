using System;
using System.Collections.Generic;
using Play;

namespace Utils
{
	public class ComputeTree
	{
		// This is the kind of function that gets called
		// when an object is marked dirty.
		public delegate void ComputeDelegate (ObjectNode node);

		public Cheap<ObjectNode> Objects;

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
		public struct ObjectNode : IComparable<ObjectNode>
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
			}

			int IComparable<ObjectNode>.CompareTo(ObjectNode other)
			{
				return Name.CompareTo (other.Name);
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

		public ComputeTree()
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
	}
}

