using System;
using Play;

namespace Utils
{
	public class Parsing
	{
		public static Group FromCharInUnicodeArray (char ch, char[] text)
		{
			return Group.Predicate<char>(delegate(char item) {
				return item == ch;
			}, text);
		}
		
		public static Group FromNonWhitespaceInUnicodeArray (char[] text)
		{
			return Group.Predicate<char>(delegate(char item) {
				return !char.IsWhiteSpace (item);
			}, text);
		}
	}
}

