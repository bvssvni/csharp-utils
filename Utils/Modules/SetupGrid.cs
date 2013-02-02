using System;
using Gtk;

namespace Utils
{
	/// <summary>
	/// Contains methods for dealing with TreeView easier for grid tasks.
	/// </summary>
	public class SetupGrid {
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
	}
}

