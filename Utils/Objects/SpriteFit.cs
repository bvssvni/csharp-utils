using System;

namespace Utils
{
	/// <summary>
	/// Contains information about a sprite sheet sequence fit.
	/// </summary>
	public class SpriteFit
	{
		public int Offset;
		public int Width;
		
		public SpriteFit(int offset, int width)
		{
			this.Offset = offset;
			this.Width = width;
		}
		
		public override string ToString()
		{
			return "offset = " + Offset.ToString() + ", width = " + Width.ToString();
		}
	}
}

