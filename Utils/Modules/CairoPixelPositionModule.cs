using System;

namespace Utils
{
	public class CairoPixelPositionModule
	{
		/// <summary>
		/// First the first occurence of a color in image.
		/// </summary>
		/// <param name='surface'>
		/// The image to search for color.
		/// </param>
		/// <param name='c'>
		/// The color to search for in image.
		/// </param>
		public static Cairo.Point First(Cairo.ImageSurface surface, Cairo.Color c) {
			int w = surface.Width;
			int h = surface.Height;
			int stride = surface.Stride;
			byte r = (byte)(c.R * 255);
			byte g = (byte)(c.G * 255);
			byte b = (byte)(c.B * 255);
			byte a = (byte)(c.A * 255);
			byte[] data = surface.Data;
			for (int i = 0; i < w; i++) {
				for (int j = 0; j < h; j++) {
					int p = i * 4 + j * stride;
					if (b == data[p++] && 
					    g == data[p++] &&
					    r == data[p++] &&
					    a == data[p++])
						return new Cairo.Point(i, j);
				}
			}
			
			return new Cairo.Point(-1, -1);
		}
	}
}

