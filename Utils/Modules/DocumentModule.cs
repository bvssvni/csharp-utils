/*

DocumentModule - Methods for making it easier to save and load in GTK#.
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

namespace Sketch
{
	public class DocumentModule
	{
		public static void Save
		(Gtk.Window window, string[] fileExtensions, string filename, Utils.IWrite<string> data) {
			var filter = Utils.FileExtensionModule.AddStar(fileExtensions);
			if (filename == null) filename = Utils.SaveFileModule.WithFilters(window, filter);
			if (filename == null) return;
			if (!Utils.FileExtensionModule.EndsWithAny(filename, fileExtensions)) filename += fileExtensions[0];
			
			data.Save(filename);
		}
		
		public static void SaveAs(Gtk.Window window, string[] fileExtensions, Utils.IWrite<string> data) {
			var filter = Utils.FileExtensionModule.AddStar(fileExtensions);
			var filename = Utils.SaveFileModule.WithFilters(window, filter);
			if (filename == null) return;
			if (!Utils.FileExtensionModule.EndsWithAny(filename, fileExtensions)) filename += fileExtensions[0];
			
			data.Save(filename);
		}
		
		public static void Open(Gtk.Window window, string[] fileExtensions, Utils.IRead<string> data) {
			var filter = Utils.FileExtensionModule.AddStar(fileExtensions);
			var filename = Utils.OpenFileModule.WithFilters(window, filter);
			if (filename == null) return;
			
			data.Read(filename);
		}
	}
}

