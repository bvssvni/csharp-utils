/*

OpenFileModule - Makes it easier to get file name from file chooser.
BSD license.
by Sven Nilsen, 2013
http://www.cutoutpro.com
Version: 0.000 in angular degrees version notation
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html

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
using Gtk;

namespace Utils
{
	public class OpenFileModule
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

