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
		/// Setup grid using names from fields of object.
		/// </summary>
		/// <returns>The fields by type.</returns>
		/// <param name="tree">Tree.</param>
		/// <param name="type">Type.</param>
		public static ListStore SetupFieldsByType (TreeView tree, Type type) {
			var fields = type.GetFields ();
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
	}
}

