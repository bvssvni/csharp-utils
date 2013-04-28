using System;
using OpenTK.Graphics.ES11;

namespace Utils
{
	public class GLTextureModule
	{
		public static uint CreateRGBATexture(IntPtr data, int width, int height) {
			uint tex = 0;
			
			GL.GenTextures(1, ref tex);
			GL.BindTexture(All.Texture2D, tex);
			GL.TexImage2D (All.Texture2D, 0, (int) All.Rgba, width, height, 0, All.Rgba, All.UnsignedByte, data);
			
			return tex;
		}
	}
}

