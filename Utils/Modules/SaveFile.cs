using System;
using Gtk;

namespace Utils
{
	public class SaveFile
	{
		/// <summary>
		/// Shows file chooser dialog with filters applied.
		/// </summary>
		/// <returns>A selected file name, null if pushing 'Cancel'.</returns>
		/// <param name="window">The owner window of the dialog.</param>
		/// <param name="filters">The filters to use in the dialog.</param>
		public static string WithFilters(Window window, params string[] filters) {
			var saveFile = new FileChooserDialog ("Save", window, FileChooserAction.Save, 
			                                      "Cancel", ResponseType.Cancel,
			                                      "OK", ResponseType.Accept);
			var filter = new FileFilter ();
			for (int i = 0; i < filters.Length; i++)
				filter.AddPattern (filters[i]);
			saveFile.Filter = filter;
			saveFile.DoOverwriteConfirmation = true;
			if (saveFile.Run() == (int)ResponseType.Accept) 
			{
				var filename = saveFile.Filename;
				saveFile.Destroy ();
				return filename;
			}
			
			saveFile.Destroy();
			
			return null;
		}
	}
}

