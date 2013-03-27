/*
SpriteAnalyzer - GAnalyzes a horizontal sprite sheet sequence and finds offset + width.  
https://github.com/bvssvni/csharp-utils
BSD license.  
by Sven Nilsen, 2012  
http://www.cutoutpro.com  
Version: 0.000 in angular degrees version notation  
http://isprogrammingeasy.blogspot.no/2012/08/angular-degrees-versioning-notation.html  

Redistribution and use in source and binary forms, with or without  
modification, are permitted provided that the following conditions are met:  
1. Redistributions of source code must retain the above copyright notice, this  
list of conditions and the following disclaimer.  
2. Redistributions in binary form must reproduce the above copyright notice,  
this list of conditions and the following disclaimer in the documentation  
and/or other materials provided with the distribution.  
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND  
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED  
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE  
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR  
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES  
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;  
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT  
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS  
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.  
The views and conclusions contained in the software and documentation are those  
of the authors and should not be interpreted as representing official policies,  
either expressed or implied, of the FreeBSD Project.  
*/

using System;
using System.Text;
using System.Collections.Generic;
using Gdk;
using Utils;
using Play;

namespace Utils
{
	public class SpriteAnalyzer
	{
		/// <summary>
		/// Determines if an image got alpha channel.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the image has alpha channel; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='buf'>
		/// Buffer.
		/// </param>
		public static bool HasAlphaChannel(Pixbuf buf)
		{
			int w = buf.Width;
			int stride = buf.Rowstride;
			bool rgba = stride / w == 4;
			return rgba;
		}

		/// <summary>
		/// Finds transparent columns in an image.
		/// </summary>
		/// <returns>
		/// An array containing a boolean value for each transparent column.
		/// </returns>
		/// <param name='buf'>
		/// The image to look for transparent columns.
		/// </param>
		public static bool[] TransparentColumns(Pixbuf buf)
		{
			int w = buf.Width;
			int h = buf.Height;
			bool[] result = new bool[w];
			unsafe {
				byte* start = (byte*)buf.Pixels;
				int stride = buf.Rowstride;

				for (int x = 0; x < w; x++) {
					bool isAlpha = true;
					for (int y = 0; y < h; y++) {
						byte* current = start + 4 * x + y * stride;
						byte alpha = current[3];
						if (alpha != 0) {
							isAlpha = false;
							break;
						}
					}

					result[x] = isAlpha;
				}
			} // end unsafe block.

			return result;
		}

		public static bool[] TransparentRows(Pixbuf buf)
		{
			int w = buf.Width;
			int h = buf.Height;
			bool[] result = new bool[h];
			unsafe {
				byte* start = (byte*)buf.Pixels;
				int stride = buf.Rowstride;
				
				for (int y = 0; y < h; y++) {
					bool isAlpha = true;
					for (int x = 0; x < w; x++) {
						byte* current = start + 4 * x + y * stride;
						byte alpha = current[3];
						if (alpha != 0) {
							isAlpha = false;
							break;
						}
					}

					result[y] = isAlpha;
				}
			} // end unsafe block.
			
			return result;
		}

		public static List<SpriteFit> FindFit(bool[] columns)
		{
			bool verbose = false;
			var list = new List<SpriteFit>();

			bool invert = true;
			var g = Group.FromBoolSamples(columns, invert);

			if (verbose) Console.WriteLine(g.ToString());

			int n = g.Count;
			if (n == 2) {
				list.Add(new SpriteFit(g[0], g[1]-g[0]));
				return list;
			}
			if (n == 0) throw new Exception("No sprites founds.");
			if ((n % 2) != 0) throw new Exception("Only finite group is allowed.");

			// Start before the beginning in case the first sprite frame is cut.
			int offsetMin = 0;
			var maxInterval = g.MaxInterval();
			var maxSize = Group.Size(maxInterval);
			var firstSize = g[1] - g[0];
			if (firstSize < maxSize) {
				offsetMin = -(maxSize - firstSize) - 2;
				if (verbose) Console.WriteLine(offsetMin);
			}

			int offsetMax = g[0];
			int widthMin = 1;
			int widthMax = g[2] - offsetMin;

			if (verbose) Console.WriteLine(offsetMin.ToString() + " offsetMin");
			if (verbose) Console.WriteLine(offsetMax.ToString() + " offsetMax");
			if (verbose) Console.WriteLine(widthMin.ToString() + " widthMin");
			if (verbose) Console.WriteLine(widthMax.ToString() + " widthMax");

			// Try to find a combination which matches with the transparent columns.
			int halfN = n >> 1;
			for (int offset = offsetMin; offset <= offsetMax; offset++) {
				for (int width = widthMin; width <= widthMax; width++) {
					bool allTransparent = true;
					for (int i = 0; i < halfN; i++) {
						int x = offset + width * i;
						if (x < 0 || x >= columns.Length) continue;
						if (!columns[x]) {
							allTransparent = false;
							break;
						}
					}

					if (allTransparent) {
						list.Add(new SpriteFit(offset, width));
					}
				}
			}

			return list;
		}

		public static bool ExpandRectangle(SpriteIsland r, int x, int y)
		{
			if (x >= r.X && y >= r.Y && x < r.X + r.Width && y < r.Y + r.Height) {
				return true;
			}

			bool expanded = false;
			if (x >= r.X && x <= r.X + r.Width) {
				if (y == r.Y - 1) {
					r.Y--;
					r.Height++;
					expanded = true;
				}
				if (y == r.Y + r.Height) {
					r.Height++;
					expanded = true;
				}
			}
			if (y >= r.Y && y <= r.Y + r.Height) {
				if (x == r.X - 1) {
					r.X--;
					r.Width++;
					expanded = true;
				}
				if (x == r.X + r.Width) {
					r.Width++;
					expanded = true;
				}
			}

			return expanded;
		}

		public static List<SpriteIsland> FindIslands(Pixbuf buf)
		{
			var list = new List<SpriteIsland>();

			// Grow rectangles when a pixel is touching the edge.
			// As one pixel might touch two rectangles, the rectangles will overlap.
			unsafe {
				byte* start = (byte*)buf.Pixels;
				int stride = buf.Rowstride;
				int w = buf.Width;
				int h = buf.Height;

				for (int y = 0; y < h; y++) {
					for (int x = 0; x < w; x++) {
						byte* current = start + x * 4 + y * stride;
						bool transparent = current[3] == 0;
						if (transparent) continue;

						// Search through list of rectangles.
						int listCount = list.Count;
						bool found = false;
						for (int i = 0; i < listCount; i++) {
							if (ExpandRectangle(list[i], x, y)) found = true;
						} // end searching through list of rectangles.

						if (found) continue;

						// Create a new rectangle containing only that pixel.
						var newRectangle = new SpriteIsland(x, y, 1, 1);
						list.Add(newRectangle);
					}
				}
			} // end unsafe block.

			// Join the overlapping rectangles.
			SpriteIsland.JoinOverlaps(list);

			return list;
		}
	}
}

