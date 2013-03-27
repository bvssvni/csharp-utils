using System;

namespace Utils
{
	/// <summary>
	/// A module for dealing with text in general.
	/// </summary>
	public class CamelCaseTextModule
	{
		/// <summary>
		/// Creates a string with space between upper case letters.
		/// </summary>
		/// <returns>Returns a string with space between upper case letters.</returns>
		/// <param name="text">The text to put in space between upper case letters.</param>
		public static string ToSpace (string text) {
			var strb = new System.Text.StringBuilder();
			for (int i = 0; i < text.Length; i++) {
				if (char.IsUpper (text[i]) && i != 0) {
					strb.Append (" ");
				}
				strb.Append (text[i]);
			}

			return strb.ToString ();
		}
	}
}

