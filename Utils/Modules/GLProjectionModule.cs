using System;
using OpenTK.Graphics.ES11;

namespace Utils
{
	public class GLProjectionModule
	{
		public static void UpperLeftOut2D(float width, float height) {
			GL.MatrixMode (All.Projection);
			GL.Ortho (0, width, height, 0, 1, -1);
		}
		
		public static void LowerLeftIn2D(float width, float height) {
			GL.MatrixMode (All.Projection);
			GL.Ortho (0, width, 0, height, -1, 1);
		}
	}
}

