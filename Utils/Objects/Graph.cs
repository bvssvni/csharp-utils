using System;
using System.Collections.Generic;
using Play;

namespace Utils
{
	public class Graph
	{
		public Cheap<ObjectNode> Objects;

		// Represents a property.
		public struct PropertyNode : IComparable<PropertyNode>
		{
			public string Name;
			public object Value;
			public Group Outputs;
			public int Input;
			public bool Dirty;

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
			public Group SubNodes;

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
				foreach (var child in SubNodes.IndicesForward ()) {
					var childNode = Cheap<ObjectNode>.Items [child];
					var props = childNode.Properties;
					int start = 0;
					int end = 0;
					if (props.GetRange (ref start, ref end) &&
					    !gr.ContainsIndex (start)) {
						gr = childNode.SubBranchProperties (gr);
					}
				}

				return gr;
			}
		}

		public Graph()
		{
		}

		// Gets the input properties of a group.
		// This are all properties that this group depend on.
		public static Group InputProperties (Group properties) {
			var gr = new Group (Group.Size (properties));
			properties.ForEach ((int i) => gr += Cheap<PropertyNode>.Items [i].Input);
			return gr;
		}

		// Gets the output properties of a group.
		// This are all properties that rely on this group.
		public static Group OutputProperties (Group properties) {
			var gr = new Group ();
			properties.ForEach ((int i) => gr += Cheap<PropertyNode>.Items [i].Outputs);
			return gr;
		}

		// Gets the properties that rely on properties within the group.
		// These properties needs to redirect the 'input' value to copied group.
		public static Group InternalDependencies (Group properties) {
			var gr = properties;
			properties.ForEach ((int i) => gr -= Cheap<PropertyNode>.Items [i].Input);
			return gr;
		}
	}
}

