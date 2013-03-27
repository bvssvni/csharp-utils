using System;
using Play;

namespace Play
{
	public class LinearGeneratorModule
	{
		public static Generator<int> Range (int start, int end, int step)
		{
			Generator<int>.ResetDelegate reset = () => start;
			Generator<int>.NextDelegate next = x => x < start ? start : x - (x-start) % step + step;
			Generator<int>.ShouldDelegate should = x => x >= start && x < end;
			return new Generator<int> (reset, next, should);
		}
	}
}

