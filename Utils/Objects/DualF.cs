using System;

namespace Utils
{
	public struct DualF
	{
		public float X;
		public float Dx;
		
		public DualF (float x, float dx) {
			this.X = x;
			this.Dx = dx;
		}
	}
}

