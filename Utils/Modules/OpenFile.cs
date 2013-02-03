using System;
using Gtk;

namespace Utils
{
	public class OpenFile
	{
		/// <summary>
		/// Shows file chooser dialog with filters applied.
		/// </summary>
		/// <returns>A selected file name, null if pushing 'Cancel'.</returns>
		/// <param name="window">The owner window of the dialog.</param>
		/// <param name="filters">The filters to use in the dialog.</param>
		public static string WithFilters(Window window, params string[] filters) {
			var openFile = new FileChooserDialog ("Open", window, FileChooserAction.Open, 
			                                      "Cancel", ResponseType.Cancel,
			                                      "OK", ResponseType.Accept);
			var filter = new FileFilter ();
			for (int i = 0; i < filters.Length; i++)
				filter.AddPattern (filters[i]);
			openFile.Filter = filter;
			if (openFile.Run() == (int)ResponseType.Accept) 
			{
				var filename = openFile.Filename;
				openFile.Destroy ();
				return filename;
			}

			openFile.Destroy();

			return null;
		}
	}
}

