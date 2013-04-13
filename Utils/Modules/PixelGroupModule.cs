using System;
using Play;

namespace Utils
{
	public class PixelGroupModule
	{
		/// <summary>
		/// Creates a group of pixels with same color in an image.
		/// </summary>
		/// <returns>
		/// The group containing the pixels of same color.
		/// </returns>
		/// <param name='surface'>
		/// The surface to search for pixels of same color.
		/// </param>
		/// <param name='c'>
		/// The color to create group from.
		/// </param>
		public static Group FromColor(Cairo.ImageSurface surface, Cairo.Color c) {
			var gr = new Group();
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
						gr += i + j * w;
				}
			}
			
			return gr;
		}
	}
}
