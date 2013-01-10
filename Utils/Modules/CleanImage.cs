using System;
using Gdk;

namespace Utils.SpriteSheetAnalyzer
{
	public class CleanImage
	{
		/// <summary>
		/// Finds the maximum alpa within an island.
		/// </summary>
		/// <returns>
		/// The max alpha value found in island.
		/// </returns>
		/// <param name='buf'>
		/// The image to containing the island.
		/// </param>
		/// <param name='rect'>
		/// A region to check for maximum alpha.
		/// </param>
		public static byte FindMaxAlpha(Pixbuf buf, SpriteIsland rect)
		{
			// Make sure not to read and write outside the image.
			var clip = SpriteIsland.Clip(rect, new SpriteIsland(0, 0, buf.Width, buf.Height));
			byte max = 0;
			unsafe {
				byte* start = (byte*)buf.Pixels;
				int stride = buf.Rowstride;
				int startX = clip.X;
				int startY = clip.Y;
				int endX = clip.X + clip.Width;
				int endY = clip.Y + clip.Height;
				for (int y = startY; y < endY; y++) {
					for (int x = startX; x < endX; x++) {
						byte* current = start + 4 * x + y * stride;
						byte alpha = current[3];
						if (alpha > max) max = alpha;
					}
				}
			}
			return max;
		}

		
		/// <summary>
		/// Erase an island from image.
		/// </summary>
		/// <param name='buf'>
		/// The image to erase the island from.
		/// </param>
		/// <param name='rect'>
		/// An island to erase all pixels.
		/// </param>
		public static byte Erase(Pixbuf buf, SpriteIsland rect)
		{
			// Make sure not to read and write outside the image.
			var clip = SpriteIsland.Clip(rect, new SpriteIsland(0, 0, buf.Width, buf.Height));
			byte max = 0;
			unsafe {
				byte* start = (byte*)buf.Pixels;
				int stride = buf.Rowstride;
				int startX = clip.X;
				int startY = clip.Y;
				int endX = clip.X + clip.Width;
				int endY = clip.Y + clip.Height;
				for (int y = startY; y < endY; y++) {
					for (int x = startX; x < endX; x++) {
						byte* current = start + 4 * x + y * stride;
						current[0] = current[1] = current[2] = current[3] = 0;
					}
				}
			}
			return max;
		}
	}
}

