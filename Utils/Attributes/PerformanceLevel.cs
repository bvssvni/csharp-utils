using System;
using System.Reflection;

namespace Utils
{
	/*
	var start = DateTime.Now.ToFileTimeUtc();
	int end = 1 << 18;
	for (int i = 0; i < end; i++) {
	}
	var seconds = (DateTime.Now.ToFileTimeUtc() - start) / 10000000.0;
	Console.WriteLine (seconds);
	*/

	/// <summary>
	/// The approximate number of 2^X iterations between 7.5 and 15 seconds.
	/// </summary>
	public class PerformanceLevel : Attribute
	{
		public int Level;

		public PerformanceLevel (int level)
		{
			this.Level = level;
		}
	}
}

