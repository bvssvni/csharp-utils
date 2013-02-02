using System;
using System.Reflection;
using Gtk;

namespace Utils
{
	/// <summary>
	/// Contains methods for dealing with TreeView easier for grid tasks.
	/// </summary>
	public class SetupGrid {
		/// <summary>
		/// Setup grid using text columns.
		/// </summary>
		/// <returns>The name of the columns.</returns>
		/// <param name="tree">The TreeView control to set up columns.</param>
		/// <param name="columns">The column names.</param>
		public static ListStore SetupTextColumns (TreeView tree, string[] columns) {
			var types = new Type[columns.Length];
			for (int i = 0; i < columns.Length; i++) {
				types[i] = typeof(string);
				tree.AppendColumn (columns[i], new CellRendererText(), "text", i);
			}

			var list = new ListStore (types);
			tree.Model = list;
			return list;
		}

		/// <summary>
		/// Setup grid using names from properties of object.
		/// </summary>
		/// <returns>A ListStore object that is linked to the tree.</returns>
		/// <param name="tree">The tree control to set up as grid.</param>
		/// <param name="type">The type to get properties from.</param>
		public static ListStore SetupPropertiesByType (TreeView tree, Type type) {
			var fields = type.GetProperties ();
			var types = new Type[fields.Length];
			int i = 0;
			foreach (var field in fields) {
				types[i] = typeof(string);
				var name = Text.CamelCaseToSpace (field.Name);
				tree.AppendColumn (name, new CellRendererText(), "text", i);
				i++;
			}

			var list = new ListStore (types);
			tree.Model = list;
			return list;
		}

		/// <summary>
		/// Adds properties of object to grid.
		/// </summary>
		/// <param name="list">The list to append a new row.</param>
		/// <param name="obj">The object to get values from.</param>
		public static void AddPropertiesByObject (ListStore list, object obj) {
			var fields = obj.GetType ().GetProperties ();
			var values = new string[fields.Length];
			for (int i = 0; i < fields.Length; i++) {
				values[i] = Convert.ToString (fields[i].GetValue (obj, null));
			}

			list.AppendValues (values);
		}
	}
}

