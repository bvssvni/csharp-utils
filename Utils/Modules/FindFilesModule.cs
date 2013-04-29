using System;
using System.Collections.Generic;

namespace Utils
{
	public class FindFilesModule
	{
		public static string[] FindFiles (string dir, bool recursive, params string[] searchPatterns) {
			var list = new List<string> ();
			if (recursive) {
				var st = new Stack<string> ();
				st.Push (dir);
				while (st.Count > 0) {
					dir = st.Pop ();
					foreach (var searchPattern in searchPatterns) {
						var files = System.IO.Directory.GetFiles (dir, searchPattern);
						list.AddRange (files);
					}
					
					var subDirectories = System.IO.Directory.GetDirectories (dir);
					foreach (var subDirectory in subDirectories) {
						st.Push (subDirectory);
					}
					
				}
			} else {
				foreach (var searchPattern in searchPatterns) {
					var files = System.IO.Directory.GetFiles (dir, searchPattern);
					list.AddRange (files);
				}
			}
			
			return list.ToArray ();
		}
	}
}

